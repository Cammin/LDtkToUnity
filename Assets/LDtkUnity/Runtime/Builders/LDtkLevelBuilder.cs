using System;
using System.Diagnostics;
using System.Linq;
using LDtkUnity.Data;
using LDtkUnity.Providers;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Builders
{
    public static class LDtkLevelBuilder
    {
        public static event Action<LDtkDataLevel> OnLevelBuilt; //todo make a monobehaviour UnityEvent listener for this
        public static event Action<Color> OnLevelBackgroundColorSet; //todo make a monobehaviour UnityEvent listener for this

        
        
        private static int _layerSortingOrder;
        private static Transform _currentLevelBuildRoot;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            OnLevelBuilt = null;
            OnLevelBackgroundColorSet = null;
            _currentLevelBuildRoot = null;
            _layerSortingOrder = 0;
        }

        public static void BuildLevel(LDtkProject project, LDtkDataProject projectData, string levelToBuild, bool disposeAfterBuilt = false)
        {
            if (project == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return;
            }
            if (string.IsNullOrEmpty(levelToBuild))
            {
                Debug.LogError("LDtk: level string is null or empty; not building level.");
                return;
            }

            if (!LDtkUnityTilesetBuilder.ValidateTilemapPrefabRequirements(project.GetTilemapPrefab()))
            {
                return;
            }

            bool success = GetProjectLevelByID(projectData.levels, levelToBuild, out LDtkDataLevel level);
            if (!success) return;
            
            string debugLvlName = $"\"{level.identifier}\"";
            //Debug.Log($"LDtk: Building level: {debugLvlName}");
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            BuildProcess(projectData, level, project, disposeAfterBuilt);
            
            levelBuildTimer.Stop();
            double ms = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LDtk: Built level {debugLvlName} in {ms}ms ({ms/1000}s)");
            
            OnLevelBuilt?.Invoke(level);
        }
        
        private static bool GetProjectLevelByID(LDtkDataLevel[] levels, string levelToBuild, out LDtkDataLevel level)
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

        private static void BuildProcess(LDtkDataProject projectData, LDtkDataLevel level, LDtkProject project, bool disposeAfterBuilt)
        {
            InitStaticTools(projectData);

            
            
            OnLevelBackgroundColorSet?.Invoke(level.BgColor());
            BuildLayerInstances(level, project);
            
            if (disposeAfterBuilt)
            {
                DisposeStaticTools();
            }
        }

        public static void InitStaticTools(LDtkDataProject project)
        {
            LDtkProviderTileBasicColor.Init();     
            LDtkProviderTilesetSprite.Init();
            LDtkProviderUid.CacheUidData(project);
            LDtkProviderErrorIdentifiers.Init();
        }
        public static void DisposeStaticTools()
        {
            LDtkProviderTileBasicColor.Dispose();
            LDtkProviderTilesetSprite.Dispose();
            LDtkProviderUid.Dispose(); 
            LDtkProviderErrorIdentifiers.Dispose();
        }

        private static void BuildLayerInstances(LDtkDataLevel level, LDtkProject project)
        {
            _layerSortingOrder = 0;
            _currentLevelBuildRoot = new GameObject(level.identifier).transform;
            
            foreach (LDtkDataLayer layer in level.layerInstances)
            {
                BuildLayerInstance(layer, project);
            }
        }

        private static void BuildLayerInstance(LDtkDataLayer layer, LDtkProject project)
        {
            if (layer.IsIntGridLayer()) BuildIntGridLayer(layer, project, project.GetTilemapPrefab());
            if (layer.IsAutoTilesLayer()) BuildTilesetLayer(layer, layer.autoLayerTiles, project, project.GetTilemapPrefab());
            if (layer.IsGridTilesLayer()) BuildTilesetLayer(layer, layer.gridTiles, project, project.GetTilemapPrefab());
            if (layer.IsEntityInstancesLayer()) BuildEntityInstanceLayer(layer, project);
        }
        
        //todo these 2 functions below are very common, split 'em
        private static void BuildIntGridLayer(LDtkDataLayer layer, LDtkProject project,
            Grid tilemapPrefab)
        {
            if (IsAssetNull(tilemapPrefab))
            {
                return;
            }

            DecrementLayer();
            
            Tilemap tilemap = MakeTilemap(layer, project.PixelsPerUnit, layer.__identifier, tilemapPrefab);
            if (tilemap == null)
            {
                return;
            }
            
            LDtkBuilderIntGridValue.BuildIntGridValues(layer, project, tilemap);
            LDtkUnityTilesetBuilder.SetTilesetOpacity(tilemap, layer.__opacity);
        }

        private static void BuildTilesetLayer(LDtkDataLayer layer, LDtkDataTile[] tiles,
            LDtkProject project, Grid tilemapPrefab)
        {
            if (IsAssetNull(tilemapPrefab))
            {
                return;
            }
            
            var grouped = tiles.Select(p => p.px.ToVector2Int()).ToLookup(x => x);
            int maxRepetitions = grouped.Max(x => x.Count());

            string gameObjectName = layer.__identifier;

            if (layer.IsIntGridLayer())
            {
                gameObjectName += "_AutoLayer";
            }

            //this is for the potential of having multiple rules apply to the same tile. make multiple Unity Tilemaps.
            Tilemap[] tilemaps = new Tilemap[maxRepetitions];


            _currentLevelBuildRoot.transform.position = layer.WorldAdjustedPosition();
            for (int i = 0; i < maxRepetitions; i++)
            {
                DecrementLayer();
                
                string name = gameObjectName;
                name += $"_{i}";

                Tilemap tilemap = MakeTilemap(layer, project.PixelsPerUnit, name, tilemapPrefab);
                
                if (tilemap == null)
                {
                    return;
                }
                
                tilemaps[i] = tilemap;
            }
            
            LDtkBuilderTileset.BuildTileset(layer, tiles, project, tilemaps);

            //set each layer's alpha
            foreach (Tilemap tilemap in tilemaps)
            {
                LDtkUnityTilesetBuilder.SetTilesetOpacity(tilemap, layer.__opacity);
            }
        }

        private static Tilemap MakeTilemap(LDtkDataLayer layer, int pixelsPerUnit, string name, Grid tilemapPrefab)
        {
            Tilemap tilemap = LDtkUnityTilesetBuilder.BuildUnityTileset(name, tilemapPrefab, _layerSortingOrder, pixelsPerUnit, layer.__gridSize);

            if (tilemap == null)
            {
                return null;
            }

            Transform tilemapTrans = tilemap.transform.parent;
            tilemapTrans.parent = _currentLevelBuildRoot;
            tilemapTrans.localPosition = Vector3.zero;
            return tilemap;
        }

        private static void BuildEntityInstanceLayer(LDtkDataLayer layer, LDtkProject project)
        {
            DecrementLayer();
            GameObject root = LDtkBuilderEntityInstance.BuildEntityLayerInstances(layer, project, _layerSortingOrder);
            
            root.transform.parent = _currentLevelBuildRoot;
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
