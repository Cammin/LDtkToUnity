using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity
{
    public class LDtkLevelBuilder
    {
        public static event Action<Level> OnLevelBuilt; //todo make a monobehaviour UnityEvent listener for this
        public static event Action<Color> OnLevelBackgroundColorSet; //todo make a monobehaviour UnityEvent listener for this

        private int _layerSortingOrder;
        private Transform _currentLevelBuildRoot;

        private readonly LDtkProject _project;
        private readonly LdtkJson _projectData;
        private readonly Level _level;

        public LDtkLevelBuilder(LDtkProject project, LdtkJson projectData, Level level)
        {
            _project = project;
            _projectData = projectData;
            _level = level;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            OnLevelBuilt = null;
            OnLevelBackgroundColorSet = null;
        }

        /// <summary>
        /// Returns the root of the object hierarchy of the layers
        /// </summary>
        public GameObject BuildLevel()
        {
            if (_project == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return null;
            }
            if (_projectData == null)
            {
                Debug.LogError("LDtk: project data null; not building level.");
                return null;
            }
            if (_level == null)
            {
                Debug.LogError("LDtk: level null; not building level.");
                return null;
            }

            if (!LDtkUnityTilesetBuilder.ValidateTilemapPrefabRequirements(_project.GetTilemapPrefab()))
            {
                Debug.LogError("LDtk: tilemap requirements not fulfilled; not building level.");
                return null;
            }
            
            if (!DoesLevelsContainLevel(_projectData.Levels, _level))
            {
                Debug.LogError("LDtk: level not contained within these levels in the project; not building level.");
                return null;
            }
            
            string debugLvlName = $"\"{_level.Identifier}\"";
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            BuildProcess();
            
            levelBuildTimer.Stop();
            double ms = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LDtk: Built level {debugLvlName} in {ms}ms ({ms/1000}s)");
            
            OnLevelBuilt?.Invoke(_level);

            return _currentLevelBuildRoot.gameObject;
        }
        
        private bool DoesLevelsContainLevel(Level[] levels, Level levelToBuild)
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

        private void BuildProcess()
        {
            InitStaticTools();
            
            OnLevelBackgroundColorSet?.Invoke(_level.UnityBgColor);
            BuildLayerInstances();
            
            DisposeStaticTools();
        }

        public void InitStaticTools()
        {
            LDtkProviderUid.CacheUidData(_projectData);
            LDtkProviderErrorIdentifiers.Init();
        }
        public void DisposeStaticTools()
        {
            LDtkProviderUid.Dispose(); 
            LDtkProviderErrorIdentifiers.Dispose();
        }

        private void BuildLayerInstances()
        {
            _layerSortingOrder = 0;
            _currentLevelBuildRoot = new GameObject(_level.Identifier).transform;
            
            foreach (LayerInstance layer in _level.LayerInstances)
            {
                BuildLayerInstance(layer);
            }
        }

        private void BuildLayerInstance(LayerInstance layer)
        {
            if (layer.IsIntGridLayer) BuildIntGridLayer(layer);
            if (layer.IsAutoLayer) BuildTilesetLayer(layer, layer.AutoLayerTiles);
            if (layer.IsTilesLayer) BuildTilesetLayer(layer, layer.GridTiles);
            if (layer.IsEntitiesLayer) BuildEntityInstanceLayer(layer);
            
            SetLevelPosition(layer);
        }

        private void SetLevelPosition(LayerInstance layer)
        {
            Transform levelTransform = _currentLevelBuildRoot.transform;
            
            levelTransform.position = layer.UnityWorldPosition;
            
            LDtkEditorUtil.Dirty(levelTransform);
        }

        //todo these 2 functions below are very common, split 'em
        private void BuildIntGridLayer(LayerInstance layer)
        {
            Grid tilemapPrefab = _project.GetTilemapPrefab();
            
            if (IsAssetNull(tilemapPrefab))
            {
                return;
            }

            DecrementLayer();
            
            Tilemap tilemap = MakeTilemap(layer, layer.Identifier, tilemapPrefab);
            if (tilemap == null)
            {
                return;
            }

            LDtkBuilderIntGridValue builder = new LDtkBuilderIntGridValue(layer, _project);
            builder.BuildIntGridValues(tilemap);
            new LDtkUnityTilesetBuilder().SetTilesetOpacity(tilemap, layer.Opacity);
        }

        private void BuildTilesetLayer(LayerInstance layer, TileInstance[] tiles)
        {
            Grid tilemapPrefab = _project.GetTilemapPrefab();
            
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


            for (int i = 0; i < maxRepetitions; i++)
            {
                DecrementLayer();
                
                string name = gameObjectName;
                name += $"_{i}";

                Tilemap tilemap = MakeTilemap(layer, name, tilemapPrefab);
                
                if (tilemap == null)
                {
                    return;
                }
                
                tilemaps[i] = tilemap;
            }
            
            new LDtkBuilderTileset(layer, _project).BuildTileset(tiles, tilemaps);

            //set each layer's alpha
            foreach (Tilemap tilemap in tilemaps)
            {
                new LDtkUnityTilesetBuilder().SetTilesetOpacity(tilemap, layer.Opacity);
            }
        }

        private Tilemap MakeTilemap(LayerInstance layer, string name, Grid tilemapPrefab)
        {
            Tilemap tilemap = new LDtkUnityTilesetBuilder().BuildUnityTileset(name, tilemapPrefab, _layerSortingOrder, _project.PixelsPerUnit, (int)layer.GridSize);

            if (tilemap == null)
            {
                return null;
            }

            Transform tilemapTrans = tilemap.transform.parent;
            tilemapTrans.parent = _currentLevelBuildRoot;
            tilemapTrans.localPosition = Vector3.zero;
            return tilemap;
        }

        private void BuildEntityInstanceLayer(LayerInstance layer)
        {
            DecrementLayer();
            LDtkBuilderEntity entityBuilder = new LDtkBuilderEntity(layer, _project);
            GameObject root = entityBuilder.BuildEntityLayerInstances(_layerSortingOrder);
            
            root.transform.parent = _currentLevelBuildRoot;
            root.transform.localPosition = Vector3.zero;
        }

        private bool IsAssetNull<T>(T assets) where T : Object
        {
            if (assets != null) return false;
            
            Debug.LogError($"LDtk: {typeof(T).Name} is null; not building layer. Check the {nameof(T)} that was used to build the level.");
            return true;
        }
        
        public void DecrementLayer()
        {
            _layerSortingOrder--;
            //Debug.Log(_layerSortingOrder);
        }
    }
}
