using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderEntity : LDtkBuilderLayer
    {
        private readonly WorldLayout _layout;
        private readonly LDtkAssetProcessorActionCache _assetProcess;
        private readonly LDtkLinearLevelVector _linearVector;
        
        private EntityInstance _entity;
        private GameObject _entityObj;
        private LDtkComponentEntity _entityComponent;
        private LDtkFields _fieldsComponent;
        private LDtkIid _iidComponent;
        
        public LDtkBuilderEntity(LDtkProjectImporter project, Level level, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkLinearLevelVector linearVector, WorldLayout layout, LDtkAssetProcessorActionCache assetProcess, LDtkJsonImporter importer) 
            : base(project, level, layerComponent, sortingOrder, importer)
        {
            _linearVector = linearVector;
            _layout = layout;
            _assetProcess = assetProcess;
        }
        
        public LDtkComponentEntity[] BuildEntityLayerInstances()
        {
            SortingOrder.Next();

            LDtkFieldParser.CacheRecentBuilder(this);

            var entities = new LDtkComponentEntity[Layer.EntityInstances.Length];
            for (int i = 0; i < Layer.EntityInstances.Length; i++)
            {
                _entity = Layer.EntityInstances[i];

                LDtkProfiler.BeginSample($"BuildEntityInstance {_entity.Identifier}");
                BuildEntityInstance();
                LDtkProfiler.EndSample();

                entities[i] = _entityComponent;
            }

            return entities;
        }

        private void BuildEntityInstance()
        {
            CreateEntityInstance();
            
            // Reason to give them unique names is to add them to the importer correctly. The importer requires unique identifiers
            _entityObj.name = $"{_entity.Identifier}_{_entity.Iid}";

            AddEntityComponent();
            AddIidComponent();
            
            PositionEntity();
            
            LDtkProfiler.BeginSample("AddFieldData");
            AddFieldData();
            LDtkProfiler.EndSample();

            PopulateEntityComponent();
            
            ScaleEntity();
        }

        private void PopulateEntityComponent()
        {
            Vector2 size = (Vector2)_entity.UnityPxSize / Project.PixelsPerUnit;
            _entityComponent.OnImport(Importer.DefinitionObjects, _entity, LayerComponent, _fieldsComponent, _iidComponent, size);
        }

        private void CreateEntityInstance()
        {
            GameObject entityPrefab = Project.GetEntity(_entity.Identifier);
            _entityObj = entityPrefab ? LDtkPrefabFactory.Instantiate(entityPrefab) : new GameObject();
        }

        private void AddIidComponent()
        {
            _iidComponent = _entityObj.AddComponent<LDtkIid>();
            _iidComponent.SetIid(_entity);
        }
        private void AddEntityComponent()
        {
            _entityComponent = _entityObj.AddComponent<LDtkComponentEntity>();
        }

        private void AddFieldData()
        {
            LDtkProfiler.BeginSample("SetEntityFieldsComponent");
            LDtkFieldsFactory fieldsFactory = new LDtkFieldsFactory(_entityObj, _entity.FieldInstances, Project, Importer);
            fieldsFactory.SetEntityFieldsComponent();
            _fieldsComponent = fieldsFactory.FieldsComponent;
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("InterfaceEvents");
            InterfaceEvents();
            LDtkProfiler.EndSample();
        }

        private void InterfaceEvents()
        {
            //leaving it like this instead of getting children because a level could get all the children of entities.
            MonoBehaviour[] behaviors = _entityObj.GetComponents<MonoBehaviour>();

            //caching here to make the import context work properly.
            int sortingOrder = SortingOrder.SortingOrderValue;
            LayerInstance layer = Layer;
            EntityInstance entity = _entity;
            LDtkFields fieldsComponent = _fieldsComponent;

            _assetProcess.TryAddInterfaceEvent<ILDtkImportedLayer>(behaviors, e => e.OnLDtkImportLayer(layer));
            if (fieldsComponent != null)
            {
                _assetProcess.TryAddInterfaceEvent<ILDtkImportedFields>(behaviors, e => e.OnLDtkImportFields(fieldsComponent));
            }
            _assetProcess.TryAddInterfaceEvent<ILDtkImportedEntity>(behaviors, e => e.OnLDtkImportEntity(entity));
            _assetProcess.TryAddInterfaceEvent<ILDtkImportedSortingOrder>(behaviors, e => e.OnLDtkImportSortingOrder(sortingOrder));
        }

        private void ScaleEntity()
        {
            if (!Project.ScaleEntities) return;
            
            //modify by the resized entity scaling from LDtk
            Vector3 newScale = _entityObj.transform.localScale;
            newScale.Scale(_entityComponent.ScaleFactor);
            _entityObj.transform.localScale = newScale;
        }

        private void PositionEntity()
        {
            _entityObj.transform.parent = LayerGameObject.transform;
            
            Vector2 localPos = LDtkCoordConverter.EntityLocalPosition(_entity.UnityPx, Layer.LevelReference.PxHei, Project.PixelsPerUnit);
            localPos += Layer.UnityWorldTotalOffset;
            
            _entityObj.transform.localPosition = localPos;
        }
        
        public PointParseData GetParsedPointData()
        {
            if (Project == null)
            {
                LDtkDebug.LogError("Failed to parse point data; the Importer was null");
                return new PointParseData();
            }
            if (Layer == null)
            {
                LDtkDebug.LogError("Failed to parse point data; the layer was null");
                return new PointParseData();
            }

            int levelOffset = 0;
            if (_linearVector != null)
            {
                levelOffset = _linearVector.Scaler;
            }
            
            return new PointParseData()
            {
                LvlCellHeight = Layer.CHei,
                PixelsPerUnit = Project.PixelsPerUnit,
                GridSize = Layer.GridSize,
                LevelPosition = Layer.LevelReference.UnityWorldSpaceCoord(_layout, Project.PixelsPerUnit, levelOffset) //todo could be a better way to work with this. could even be the LayerObject position to save on calculating
            };
        }
    }
}