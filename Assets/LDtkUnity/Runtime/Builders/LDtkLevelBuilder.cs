using System;
using System.Diagnostics;
using System.Linq;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Colliders;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.Settings;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkLevelBuilder
    {
        public static event Action<LDtkDataLevel> OnLevelBuilt;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            OnLevelBuilt = null;
        }

        public static void BuildLevel(LDtkDataProject project, LDtkLevelIdentifier levelToBuild, LDtkProjectAssets assets)
        {
            if (assets == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return;
            }
            if (levelToBuild == null)
            {
                Debug.LogError("LDtk: LDtkLevelIdentifier object is null; not building level.");
                return;
            }
            
            bool success = GetProjectLevelByID(project.levels, levelToBuild, out LDtkDataLevel level);
            if (!success) return;
            
            string debugLvlName = $"\"{level.identifier}\"";
            Debug.Log($"LDtk: Building level: {debugLvlName}");
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            BuildProcess(project, level, assets);
            
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

        private static void BuildProcess(LDtkDataProject project, LDtkDataLevel level, LDtkProjectAssets assets)
        {
            InitStaticTools(project);
            BuildLayerInstances(level, assets);
            DisposeStaticTools();
        }

        private static void InitStaticTools(LDtkDataProject project)
        {
            LDtkProviderTile.Init();     
            LDtkProviderTilesetSprite.Init();
            LDtkProviderEnum.Init();
            LDtkProviderUid.CacheUidData(project);
            LDtkProviderErrorIdentifiers.Init();
        }
        private static void DisposeStaticTools()
        {
            LDtkProviderTile.Dispose();
            LDtkProviderTilesetSprite.Dispose();
            LDtkProviderEnum.Dispose();
            LDtkProviderUid.Dispose(); 
            LDtkProviderErrorIdentifiers.Dispose();
        }

        private static void BuildLayerInstances(LDtkDataLevel level, LDtkProjectAssets assets)
        {
            foreach (LDtkDataLayer layer in level.layerInstances)
            {
                BuildLayerInstance(layer, assets);
            }
        }

        private static void BuildLayerInstance(LDtkDataLayer layer, LDtkProjectAssets assets)
        {
            if (layer.IsIntGridLayer)
            {
                BuildIntGridLayer(layer, assets.CollisionValues, assets.TilemapPrefab);
            }

            if (layer.IsAutoTilesLayer)
            {
                BuildTilesetLayer(layer, layer.autoLayerTiles, assets.Tilesets, assets.TilemapPrefab);
            }

            if (layer.IsGridTilesLayer)
            {
                BuildTilesetLayer(layer, layer.gridTiles, assets.Tilesets, assets.TilemapPrefab);
            }

            if (layer.IsEntityInstancesLayer)
            {
                BuildEntityInstanceLayer(layer, assets.EntityInstances);
            }
        }


        
        private static void BuildIntGridLayer(LDtkDataLayer layer, LDtkIntGridValueAssetCollection assets, Grid tilemapPrefab)
        {
            if (IsAssetNull(assets)) return;

            if (IsAssetNull(tilemapPrefab)) return;
            
            Grid grid = InstantiateTilemap(tilemapPrefab, layer.__identifier);
            Tilemap tilemap = grid.GetComponentInChildren<Tilemap>();
            
            LDtkBuilderIntGridValue.BuildIntGridValues(layer, assets, tilemap);
        }

        private static void BuildTilesetLayer(LDtkDataLayer layer, LDtkDataTile[] tiles, LDtkTilesetAssetCollection assets, Grid tilemapPrefab)
        {
            //todo duplicate code in 2 functions (BuildIntGridLayer), merge em
            if (IsAssetNull(assets)) return;
            
            if (IsAssetNull(tilemapPrefab)) return;

            string objName = layer.__identifier + (layer.IsIntGridLayer ? "_AutoLayer" : "");
            Grid grid = InstantiateTilemap(tilemapPrefab, objName);
            Tilemap tilemap = grid.GetComponentInChildren<Tilemap>();
            
            LDtkBuilderTileset.BuildTileset(layer, tiles, assets, tilemap);
        }
        
        private static void BuildEntityInstanceLayer(LDtkDataLayer layer, LDtkEntityAssetCollection assets)
        {
            if (IsAssetNull(assets)) return;
            
            LDtkBuilderEntityInstance.BuildEntityLayerInstances(layer, assets);
        }
        
        private static bool IsAssetNull<T>(T assets) where T : Object
        {
            if (assets != null) return false;
            
            Debug.LogError($"LDtk: {typeof(T).Name} is null; not building layer. Check the {nameof(LDtkProjectAssets)} that was used to build the level.");
            return true;
        }

        private static Grid InstantiateTilemap(Grid prefab, string objName)
        {
            Grid grid = Object.Instantiate(prefab);
            grid.transform.position = Vector3.zero;
            grid.gameObject.name = objName;
            return grid;
        }
    }
}
