using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

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
        
        
        public LDtkBuilderEntity(LDtkProjectImporter project, LDtkComponentLayer layerComponent, LDtkSortingOrder sortingOrder, LDtkLinearLevelVector linearVector, WorldLayout layout, LDtkAssetProcessorActionCache assetProcess, LDtkJsonImporter importer) 
            : base(project, layerComponent, sortingOrder, importer)
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

                Profiler.BeginSample($"BuildEntityInstance {_entity.Identifier}");
                BuildEntityInstance();
                Profiler.EndSample();

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
            ScaleEntity();
            
            Profiler.BeginSample("AddFieldData");
            AddFieldData();
            Profiler.EndSample();

            PopulateEntityComponent();
        }

        private void PopulateEntityComponent()
        {
            _entityComponent.OnImport(Importer.DefinitionObjects, _entity, LayerComponent, _fieldsComponent, _iidComponent);
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
            Profiler.BeginSample("SetEntityFieldsComponent");
            LDtkFieldsFactory fieldsFactory = new LDtkFieldsFactory(_entityObj, _entity.FieldInstances, Project, Importer);
            fieldsFactory.SetEntityFieldsComponent();
            _fieldsComponent = fieldsFactory.FieldsComponent;
            Profiler.EndSample();
            
            Profiler.BeginSample("AddHandleDrawers");
            AddHandleDrawers(_entityObj, _entity, Layer.GridSize);
            Profiler.EndSample();
            
            Profiler.BeginSample("InterfaceEvents");
            InterfaceEvents();
            Profiler.EndSample();
        }

        private void InterfaceEvents()
        {
            //leaving it like this instead of getting children because a level could get all the children of entities.
            MonoBehaviour[] behaviors = _entityObj.GetComponents<MonoBehaviour>();

            int sortingOrder = SortingOrder.SortingOrderValue;
            LayerInstance layer = Layer;
            EntityInstance entity = _entity;

            _assetProcess.TryAddInterfaceEvent<ILDtkImportedLayer>(behaviors, e => e.OnLDtkImportLayer(layer));
            if (_fieldsComponent != null)
            {
                _assetProcess.TryAddInterfaceEvent<ILDtkImportedFields>(behaviors, e => e.OnLDtkImportFields(_fieldsComponent));
            }
            _assetProcess.TryAddInterfaceEvent<ILDtkImportedEntity>(behaviors, e => e.OnLDtkImportEntity(entity));
            _assetProcess.TryAddInterfaceEvent<ILDtkImportedSortingOrder>(behaviors, e => e.OnLDtkImportSortingOrder(sortingOrder));
        }

        private void ScaleEntity()
        {
            //modify by the resized entity scaling from LDtk
            Vector3 newScale = _entityObj.transform.localScale;
            newScale.Scale(_entity.UnityScale);
            _entityObj.transform.localScale = newScale;
        }

        private void PositionEntity()
        {
            _entityObj.transform.parent = LayerGameObject.transform;
            
            Vector2 localPos = LDtkCoordConverter.EntityLocalPosition(_entity.UnityPx, Layer.LevelReference.PxHei, Project.PixelsPerUnit);
            localPos += Layer.UnityWorldTotalOffset;
            
            _entityObj.transform.localPosition = localPos;
        }

        /// <summary>
        /// Only doing this for importer performance. an early return to not build the rest
        /// </summary>
        private static bool DrawerEligibility(FieldInstance field)
        {
            EditorDisplayMode? mode = field.Definition.EditorDisplayMode;
            
            switch (mode)
            {
                case EditorDisplayMode.Hidden: //do not show
                    return false;
                
                case EditorDisplayMode.ValueOnly: //all but point/point array
                    return !field.IsPoint;
                    
                case EditorDisplayMode.NameAndValue: //all
                    return true;
                    
                case EditorDisplayMode.EntityTile: //enum/enum array, tile/tile array
                    return field.IsEnum || field.IsTile;

                case EditorDisplayMode.RadiusGrid: //int, float
                case EditorDisplayMode.RadiusPx: //int, float
                    return field.IsInt || field.IsFloat;

                case EditorDisplayMode.PointStar: //point, point array
                case EditorDisplayMode.Points: //point, point array
                    return field.IsPoint;
                    
                case EditorDisplayMode.PointPath: //point array only
                case EditorDisplayMode.PointPathLoop: //point array only
                    return field.IsPoint && field.Definition.IsArray;
                
                case EditorDisplayMode.ArrayCountNoLabel: //any arrays
                case EditorDisplayMode.ArrayCountWithLabel: //any arrays
                    return field.Definition.IsArray;
                    
                case EditorDisplayMode.RefLinkBetweenCenters: //entity ref, entity ref array
                case EditorDisplayMode.RefLinkBetweenPivots: //entity ref, entity ref array
                    return field.IsEntityRef;
                    
                default:
                    LDtkDebug.LogError("No Drawer eligibility found!");
                    return false;
            }
        }
        
        private void AddHandleDrawers(GameObject gameObject, EntityInstance entityInstance, int gridSize)
        {
            LDtkEntityDrawerComponent drawerComponent = gameObject.gameObject.AddComponent<LDtkEntityDrawerComponent>();
            EntityDefinition entityDef = entityInstance.Definition;

            string entityPath = GetEntityImageAndRect(entityInstance, Project.assetPath, out Rect entityIconRect);
            Vector2 size = (Vector2)entityInstance.UnitySize / Project.PixelsPerUnit;

            Color smartColor = entityInstance.UnitySmartColor;

            //entity handle data
            LDtkEntityDrawerData entityDrawerData = new LDtkEntityDrawerData(drawerComponent.transform, entityDef, entityPath, entityIconRect, size, smartColor);
            drawerComponent.AddEntityDrawer(entityDrawerData);

            foreach (FieldInstance fieldInstance in entityInstance.FieldInstances)
            {
                if (!DrawerEligibility(fieldInstance))
                {
                    continue;
                }

                EditorDisplayMode displayMode = fieldInstance.Definition.EditorDisplayMode;
                Vector2 pivotOffset = LDtkCoordConverter.EntityPivotOffset(entityDef.UnityPivot, size);
                Vector3 middleCenter = gameObject.transform.position + (Vector3)pivotOffset;
                
                LDtkFieldDrawerData data = new LDtkFieldDrawerData(_fieldsComponent, smartColor, displayMode, fieldInstance.Identifier, gridSize, Project.PixelsPerUnit, middleCenter);
                drawerComponent.AddReference(data);
            }
        }
        
        //this would be used instead in the entity drawer for getting the texture that way
        private string GetEntityImageAndRect(EntityInstance entityInstance, string assetPath, out Rect rect)
        {
            rect = new Rect();
            
            TilesetRectangle tile = entityInstance.Tile;
            if (tile == null)
            {
                return null;
            }

            LDtkRelativeGetterTilesetTexture textureGetter = new LDtkRelativeGetterTilesetTexture();
            Texture2D tex = textureGetter.GetRelativeAsset(tile.Tileset, assetPath);
            if (tex == null)
            {
                return null;
            }

            Rect src = tile.UnityRect;
            rect = LDtkCoordConverter.ImageSlice(src, tex.height);

            string texPath = AssetDatabase.GetAssetPath(tex);
            return texPath;
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