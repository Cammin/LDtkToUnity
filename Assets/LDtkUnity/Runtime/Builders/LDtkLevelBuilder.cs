using System;
using System.Diagnostics;
using System.Linq;
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
        public static event Action<Level> OnLevelBuilt; //todo make a monobehaviour UnityEvent listener for this
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

        /// <summary>
        /// Returns the root of the object hierarchy of the layers
        /// </summary>
        public static GameObject BuildLevel(LDtkProject project, LdtkJson projectData, Level levelToBuild, bool disposeAfterBuilt = false)
        {
            if (project == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return null;
            }
            if (projectData == null)
            {
                Debug.LogError("LDtk: project data null; not building level.");
                return null;
            }
            if (levelToBuild == null)
            {
                Debug.LogError("LDtk: level null; not building level.");
                return null;
            }

            if (!LDtkUnityTilesetBuilder.ValidateTilemapPrefabRequirements(project.GetTilemapPrefab()))
            {
                return null;
            }

            bool success = DoesLevelsContainLevel(projectData.Levels, levelToBuild);
            if (!success)
            {
                return null;
            }
            
            string debugLvlName = $"\"{levelToBuild.Identifier}\"";
            //Debug.Log($"LDtk: Building level: {debugLvlName}");
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            BuildProcess(projectData, levelToBuild, project, disposeAfterBuilt);
            
            levelBuildTimer.Stop();
            double ms = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LDtk: Built level {debugLvlName} in {ms}ms ({ms/1000}s)");
            
            OnLevelBuilt?.Invoke(levelToBuild);

            return _currentLevelBuildRoot.gameObject;
        }
        
        private static bool DoesLevelsContainLevel(Level[] levels, Level levelToBuild)
        {
            if (levelToBuild == null)
            {
                Debug.LogError($"LDtk: LevelToBuild null, not assigned?");
                return false;
            }
            
            if (levels.Any(lvl => string.Equals(lvl.Identifier, levelToBuild.Identifier)))
            {
                return true;
            }
            
            Debug.LogError($"LDtk: No level named \"{levelToBuild}\" exists in the LDtk Project");
            return false;

        }

        private static void BuildProcess(LdtkJson projectData, Level level, LDtkProject project, bool disposeAfterBuilt)
        {
            InitStaticTools(projectData);

            
            OnLevelBackgroundColorSet?.Invoke(level.UnityBgColor);
            BuildLayerInstances(level, project);
            
            if (disposeAfterBuilt)
            {
                DisposeStaticTools();
            }
        }

        public static void InitStaticTools(LdtkJson project)
        {
            LDtkIntGridValueFactory.Init();
            LDtkProviderUid.CacheUidData(project);
            LDtkProviderErrorIdentifiers.Init();
        }
        public static void DisposeStaticTools()
        {
            LDtkIntGridValueFactory.Dispose();
            LDtkProviderUid.Dispose(); 
            LDtkProviderErrorIdentifiers.Dispose();
        }

        private static void BuildLayerInstances(Level level, LDtkProject project)
        {
            _layerSortingOrder = 0;
            _currentLevelBuildRoot = new GameObject(level.Identifier).transform;
            
            foreach (LayerInstance layer in level.LayerInstances)
            {
                BuildLayerInstance(layer, project);
            }
        }

        private static void BuildLayerInstance(LayerInstance layer, LDtkProject project)
        {
            if (layer.IsIntGridLayer) BuildIntGridLayer(layer, project, project.GetTilemapPrefab());
            if (layer.IsAutoTilesLayer) BuildTilesetLayer(layer, layer.AutoLayerTiles, project, project.GetTilemapPrefab());
            if (layer.IsGridTilesLayer) BuildTilesetLayer(layer, layer.GridTiles, project, project.GetTilemapPrefab());
            if (layer.IsEntityInstancesLayer) BuildEntityInstanceLayer(layer, project);
        }
        
        //todo these 2 functions below are very common, split 'em
        private static void BuildIntGridLayer(LayerInstance layer, LDtkProject project,
            Grid tilemapPrefab)
        {
            if (IsAssetNull(tilemapPrefab))
            {
                return;
            }

            DecrementLayer();
            
            Tilemap tilemap = MakeTilemap(layer, project.PixelsPerUnit, layer.Identifier, tilemapPrefab);
            if (tilemap == null)
            {
                return;
            }
            
            LDtkBuilderIntGridValue.BuildIntGridValues(layer, project, tilemap);
            LDtkUnityTilesetBuilder.SetTilesetOpacity(tilemap, layer.Opacity);
        }

        private static void BuildTilesetLayer(LayerInstance layer, TileInstance[] tiles,
            LDtkProject project, Grid tilemapPrefab)
        {
            if (IsAssetNull(tilemapPrefab))
            {
                return;
            }
            
            var grouped = tiles.Select(p => p.Px.ToVector2Int()).ToLookup(x => x);
            int maxRepetitions = grouped.Max(x => x.Count());

            string gameObjectName = layer.Identifier;

            if (layer.IsIntGridLayer)
            {
                gameObjectName += "_AutoLayer";
            }

            //this is for the potential of having multiple rules apply to the same tile. make multiple Unity Tilemaps.
            Tilemap[] tilemaps = new Tilemap[maxRepetitions];


            _currentLevelBuildRoot.transform.position = layer.UnityWorldPosition;
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
                LDtkUnityTilesetBuilder.SetTilesetOpacity(tilemap, layer.Opacity);
            }
        }

        private static Tilemap MakeTilemap(LayerInstance layer, int pixelsPerUnit, string name, Grid tilemapPrefab)
        {
            Tilemap tilemap = LDtkUnityTilesetBuilder.BuildUnityTileset(name, tilemapPrefab, _layerSortingOrder, pixelsPerUnit, (int)layer.GridSize);

            if (tilemap == null)
            {
                return null;
            }

            Transform tilemapTrans = tilemap.transform.parent;
            tilemapTrans.parent = _currentLevelBuildRoot;
            tilemapTrans.localPosition = Vector3.zero;
            return tilemap;
        }

        private static void BuildEntityInstanceLayer(LayerInstance layer, LDtkProject project)
        {
            DecrementLayer();
            GameObject root = LDtkBuilderEntityInstance.BuildEntityLayerInstances(layer, project, _layerSortingOrder);
            
            root.transform.parent = _currentLevelBuildRoot;
            root.transform.localPosition = Vector3.zero;
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
