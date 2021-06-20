using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    public class LDtkLevelBuilder
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _json;
        private readonly Level _level;
        
        private GameObject _levelGameObject;
        private GameObject _layerGameObject;
        private Grid _layerGrid;
        
        private LDtkSortingOrder _sortingOrder;
        private LDtkBuilderTileset _builderTileset;
        private LDtkBuilderIntGridValue _builderIntGrid;
        private LDtkBuilderEntity _entityBuilder;
        private LDtkLevelBackgroundBuilder _backgroundBuilder;
        
        public LDtkLevelBuilder(LDtkProjectImporter importer, LdtkJson json, Level level)
        {
            _importer = importer;
            _json = json;
            _level = level;
        }
        
        /// <summary>
        /// Returns the root of the object hierarchy of the layers
        /// </summary>
        public GameObject BuildLevel()
        {
            if (!CanTryBuildLevel())
            {
                return null;
            }
            
            InvokeWithinTimer(BuildLayerInstances);

            return _levelGameObject.gameObject;
        }
        
        private void InvokeWithinTimer(Action action)
        {
            Stopwatch levelBuildTimer = Stopwatch.StartNew();
            action.Invoke();
            levelBuildTimer.Stop();

            if (!LDtkPrefs.LogBuildTimes)
            {
                return;
            }
            
            double ms = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LDtk: Built level \"{_level.Identifier}\" in {ms}ms ({ms/1000}s)");
        }

        private bool CanTryBuildLevel()
        {
            if (_importer == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return false;
            }

            if (_json == null)
            {
                Debug.LogError("LDtk: project data null; not building level.");
                return false;
            }

            if (_level == null)
            {
                Debug.LogError("LDtk: level null; not building level.");
                return false;
            }

            if (!DoesLevelsContainLevel(_json.Levels, _level))
            {
                Debug.LogError("LDtk: level not contained within these levels in the project; not building level.");
                return false;
            }

            return true;
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


        
        private void BuildLayerInstances()
        {
            LDtkComponentLevel levelComponent = CreateLevelGameObject();

            _sortingOrder = new LDtkSortingOrder();
            
            //build layers and background from front to back in terms of ordering 
            foreach (LayerInstance layer in _level.LayerInstances)
            {
                BuildLayerInstance(layer);
            }

            _backgroundBuilder = new LDtkLevelBackgroundBuilder(_importer, _levelGameObject, _sortingOrder, _level, levelComponent.Size);
            _backgroundBuilder.BuildBackground();
        }

        private LDtkComponentLevel CreateLevelGameObject()
        {
            _levelGameObject = _importer.CustomLevelPrefab ? LDtkPrefabFactory.Instantiate(_importer.CustomLevelPrefab) : new GameObject();
            _levelGameObject.name = _level.Identifier;
            
            _levelGameObject.transform.position = _level.UnityWorldSpaceCoord(_importer.PixelsPerUnit);

            if (_importer.DeparentInRuntime)
            {
                _levelGameObject.AddComponent<LDtkDetachChildren>();
            }



            LDtkComponentLevel levelComponent = _levelGameObject.AddComponent<LDtkComponentLevel>();
            levelComponent.SetIdentifier(_level.Identifier);
            levelComponent.SetSize((Vector2)_level.UnityPxSize / _importer.PixelsPerUnit);
            levelComponent.SetBgColor(_level.UnityBgColor);
            
            //interface events
            MonoBehaviour[] behaviors = _levelGameObject.GetComponents<MonoBehaviour>();
            
            if (!_json.Defs.LevelFields.IsNullOrEmpty())
            {
                LDtkFieldInjector fieldInjector = new LDtkFieldInjector(_levelGameObject, _level.FieldInstances);
                fieldInjector.InjectEntityFields();
                LDtkInterfaceEvent.TryEvent<ILDtkImportedFields>(behaviors, level => level.OnLDtkImportFields(fieldInjector.FieldsComponent));
            }
            
            LDtkInterfaceEvent.TryEvent<ILDtkImportedLevel>(behaviors, level => level.OnLDtkImportLevel(_level));

            return levelComponent;
        }

        private void BuildLayerInstance(LayerInstance layer)
        {
            _layerGameObject = _levelGameObject.CreateChildGameObject(layer.Identifier);
            
            //entities layer is different from the other three types
            if (layer.IsEntitiesLayer)
            {
                _entityBuilder = new LDtkBuilderEntity(_importer, _layerGameObject, _sortingOrder);
                _entityBuilder.SetLayer(layer);
                _entityBuilder.BuildEntityLayerInstances();
                return;
            }

            _layerGrid = _layerGameObject.AddComponent<Grid>();
            _builderTileset = new LDtkBuilderTileset(_importer, _layerGameObject, _sortingOrder);
            
            if (layer.IsTilesLayer)
            {
                _builderTileset.SetLayer(layer);
                _builderTileset.BuildTileset(layer.GridTiles);
            }
            
            //an int grid layer could also be an auto layer
            if (layer.IsAutoLayer)
            {
                _builderTileset.SetLayer(layer);
                _builderTileset.BuildTileset(layer.AutoLayerTiles);
            }
            
            if (layer.IsIntGridLayer)
            {
                _builderIntGrid = new LDtkBuilderIntGridValue(_importer, _layerGameObject, _sortingOrder);
                _builderIntGrid.SetLayer(layer);
                _builderIntGrid.BuildIntGridValues();
            }
            
            
            
            float size = (float) layer.GridSize / _importer.PixelsPerUnit;
            Vector3 scale = new Vector3(size, size, 1);
            _layerGrid.transform.localScale = scale;
        }
    }
}
