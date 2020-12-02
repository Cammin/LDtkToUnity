using System;
using System.Diagnostics;
using System.Linq;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkLevelBuilder
    {
        public static event Action<LDtkDataLevel> OnLevelBuilt;

        private static int _layerSortingOrder;

#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        private static void ResetStatics()
        {
            OnLevelBuilt = null;
            _layerSortingOrder = 0;
        }

        public static void BuildLevel(LDtkDataProject projectData, LDtkLevelIdentifier levelToBuild, LDtkProject project)
        {
            if (project == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return;
            }
            if (levelToBuild == null)
            {
                Debug.LogError("LDtk: LDtkLevelIdentifier object is null; not building level.");
                return;
            }

            if (!LDtkUnityTilesetBuilder.ValidateTilemapPrefabRequirements(project.TilemapPrefab))
            {
                return;
            }
            
            
            bool success = GetProjectLevelByID(projectData.levels, levelToBuild, out LDtkDataLevel level);
            if (!success) return;
            
            string debugLvlName = $"\"{level.identifier}\"";
            //Debug.Log($"LDtk: Building level: {debugLvlName}");
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            BuildProcess(projectData, level, project);
            
            levelBuildTimer.Stop();
            double ms = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LDtk: Built level {debugLvlName} in {ms}ms ({ms/1000}s)");
            
            OnLevelBuilt?.Invoke(level);
        }
        
        private static bool GetProjectLevelByID(LDtkDataLevel[] levels, LDtkLevelIdentifier levelToBuild, out LDtkDataLevel level)
        {
            level = default;

            if (levelToBuild == null)
            {
                Debug.LogError($"LDtk: LevelToBuild null, not assigned?");
                return false;
            }

            bool IdentifierMatchesLevelToBuild(LDtkDataLevel lvl) => string.Equals(lvl.identifier, levelToBuild);
            
            if (!levels.Any(IdentifierMatchesLevelToBuild))
            {
                Debug.LogError($"LDtk: No level named \"{levelToBuild}\" exists in the LDtk Project");
                return false;
            }

            level = levels.First(IdentifierMatchesLevelToBuild);
            return true;
        }

        private static void BuildProcess(LDtkDataProject projectData, LDtkDataLevel level, LDtkProject project)
        {
            InitStaticTools(projectData);
            BuildLayerInstances(level, project);
            DisposeStaticTools();
        }

        private static void InitStaticTools(LDtkDataProject project)
        {
            LDtkProviderTileBasicColor.Init();     
            LDtkProviderTilesetSprite.Init();
            LDtkProviderUid.CacheUidData(project);
            LDtkProviderErrorIdentifiers.Init();
        }
        private static void DisposeStaticTools()
        {
            LDtkProviderTileBasicColor.Dispose();
            LDtkProviderTilesetSprite.Dispose();
            LDtkProviderUid.Dispose(); 
            LDtkProviderErrorIdentifiers.Dispose();
        }

        private static void BuildLayerInstances(LDtkDataLevel level, LDtkProject project)
        {
            _layerSortingOrder = 0;
            
            foreach (LDtkDataLayer layer in level.layerInstances)
            {
                BuildLayerInstance(layer, project);
            }
        }

        private static void BuildLayerInstance(LDtkDataLayer layer, LDtkProject project)
        {
            if (layer.IsIntGridLayer) BuildIntGridLayer(layer, project, project.TilemapPrefab);
            if (layer.IsAutoTilesLayer) BuildTilesetLayer(layer, layer.autoLayerTiles, project, project.TilemapPrefab);
            if (layer.IsGridTilesLayer) BuildTilesetLayer(layer, layer.gridTiles, project, project.TilemapPrefab);
            if (layer.IsEntityInstancesLayer) BuildEntityInstanceLayer(layer, project);
        }
        
        private static void BuildIntGridLayer(LDtkDataLayer layer, LDtkProject project,
            Grid tilemapPrefab)
        {
            if (IsAssetNull(tilemapPrefab)) return;

            DecrementLayer();
            
            Tilemap tilemap = LDtkUnityTilesetBuilder.BuildUnityTileset(layer.__identifier, tilemapPrefab, _layerSortingOrder);
            if (tilemap == null) return;
            
            LDtkBuilderIntGridValue.BuildIntGridValues(layer, project, tilemap);
            
            LDtkUnityTilesetBuilder.SetTilesetOpacity(tilemap, layer.__opacity);
        }

        private static void BuildTilesetLayer(LDtkDataLayer layer, LDtkDataTile[] tiles,
            LDtkProject project, Grid tilemapPrefab)
        {
            if (IsAssetNull(tilemapPrefab)) return;
            
            var grouped = tiles.Select(p => p.px.ToVector2Int()).ToLookup(x => x);
            int maxRepetitions = grouped.Max(x => x.Count());

            
            string gameObjectName = layer.__identifier;

            if (layer.IsIntGridLayer)
            {
                gameObjectName += "_AutoLayer";
            }

            //this is for the potential of having multiple rules apply to the same tile. make multiple Unity Tilemaps.
            Tilemap[] tilemaps = new Tilemap[maxRepetitions];
            for (int i = 0; i < maxRepetitions; i++)
            {
                DecrementLayer();
                
                string name = gameObjectName;
                name += $"_{i}";
                Tilemap tilemap = LDtkUnityTilesetBuilder.BuildUnityTileset(name, tilemapPrefab, _layerSortingOrder);
                if (tilemap == null) return;
                
                tilemaps[i] = tilemap;
                
            }
            
            LDtkBuilderTileset.BuildTileset(layer, tiles, project, tilemaps);

            //set each layer's alpha
            foreach (Tilemap tilemap in tilemaps)
            {
                LDtkUnityTilesetBuilder.SetTilesetOpacity(tilemap, layer.__opacity);
            }
        }

        private static void BuildEntityInstanceLayer(LDtkDataLayer layer, LDtkProject project)
        {
            DecrementLayer();
            LDtkBuilderEntityInstance.BuildEntityLayerInstances(layer, project, _layerSortingOrder);
        }


        
        private static bool IsAssetNull<T>(T assets) where T : Object
        {
            if (assets != null) return false;
            
            Debug.LogError($"LDtk: {typeof(T).Name} is null; not building layer. Check the {nameof(T)} that was used to build the level.");
            return true;
        }
        
        public static void DecrementLayer()
        {
            _layerSortingOrder--;
            //Debug.Log(_layerSortingOrder);
        }


    }
}
