using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkBuilderEntity : LDtkBuilderLayer
    {
        private readonly WorldLayout _layout;
        private readonly LDtkLinearLevelVector _linearVector;
        
        private EntityInstance _entity;
        private GameObject _entityObj;
        
        
        public LDtkBuilderEntity(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder, LDtkLinearLevelVector linearVector, WorldLayout layout) : base(importer, layerGameObject, sortingOrder)
        {
            _linearVector = linearVector;
            _layout = layout;
        }

        //this is to maintain uniqueness in the import process
        //private readonly Dictionary<string, int> _entitiesBuilt = new Dictionary<string, int>();
        
        public void BuildEntityLayerInstances()
        {
            if (Importer.DeparentInRuntime)
            {
                LayerGameObject.AddComponent<LDtkDetachChildren>();
            }
            
            SortingOrder.Next();

            LDtkFieldParser.CacheRecentBuilder(this);

            foreach (EntityInstance entityInstance in Layer.EntityInstances)
            {
                _entity = entityInstance;
                BuildEntityInstance();
            }
        }

        private void BuildEntityInstance()
        {
            GameObject entityPrefab = Importer.GetEntity(_entity.Identifier);
            _entityObj = entityPrefab ? LDtkPrefabFactory.Instantiate(entityPrefab) : new GameObject();
            
            // Reason to give them unique names is to add them to the importer correctly. The importer requires unique identifiers 
            _entityObj.name = $"{_entity.Identifier}_{_entity.Iid}";

            AddIidComponent();
            
            PositionEntity();
            ScaleEntity();
            AddFieldData();
        }

        private void AddIidComponent()
        {
            LDtkIid iid = _entityObj.AddComponent<LDtkIid>();
            iid.SetIid(_entity);
        }

        private void AddFieldData()
        {
            LDtkFieldsFactory fieldsFactory = new LDtkFieldsFactory(_entityObj, _entity.FieldInstances);
            fieldsFactory.SetEntityFieldsComponent();
            
            AddHandleDrawers(_entityObj, fieldsFactory.FieldsComponent, _entity, (int)Layer.GridSize);
            
            InterfaceEvents(fieldsFactory.FieldsComponent);
        }

        private void InterfaceEvents(LDtkFields fields)
        {
            MonoBehaviour[] behaviors = _entityObj.GetComponents<MonoBehaviour>();

            int sortingOrder = SortingOrder.SortingOrderValue;
            LayerInstance layer = Layer;
            EntityInstance entity = _entity;

            LDtkPostProcessorCache.AddPostProcessAction(() =>
            {
                LDtkInterfaceEvent.TryEvent<ILDtkImportedLayer>(behaviors, e => e.OnLDtkImportLayer(layer));
                
                if (fields != null)
                {
                    LDtkInterfaceEvent.TryEvent<ILDtkImportedFields>(behaviors, e => e.OnLDtkImportFields(fields));
                }

                LDtkInterfaceEvent.TryEvent<ILDtkImportedEntity>(behaviors, e => e.OnLDtkImportEntity(entity));
                LDtkInterfaceEvent.TryEvent<ILDtkImportedSortingOrder>(behaviors, e => e.OnLDtkImportSortingOrder(sortingOrder));
            });
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
            
            Vector2 localPos = LDtkCoordConverter.EntityLocalPosition(_entity.UnityPx, (int) Layer.LevelReference.PxHei, Importer.PixelsPerUnit);
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
                    Debug.LogError("LDtk: No Drawer eligibility found!");
                    return false;
            }
        }
        
        private void AddHandleDrawers(GameObject gameObject, LDtkFields fields, EntityInstance entityInstance, int gridSize)
        {
            LDtkEntityDrawerComponent drawerComponent = gameObject.gameObject.AddComponent<LDtkEntityDrawerComponent>();
            EntityDefinition entityDef = entityInstance.Definition;

            string entityPath = GetEntityImageAndRect(entityInstance, Importer.assetPath, out RectInt entityIconRect);
            Vector2 size = (Vector2)entityInstance.UnitySize / (int)Layer.GridSize;

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
                
                LDtkFieldDrawerData data = new LDtkFieldDrawerData(fields, smartColor, displayMode, fieldInstance.Identifier, gridSize, middleCenter);
                drawerComponent.AddReference(data);
            }
        }
        
        //this would be used instead in the entity drawer for getting the texture that way
        private static string GetEntityImageAndRect(EntityInstance entityInstance, string assetPath, out RectInt rect)
        {
            rect = new RectInt();
            
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

            RectInt src = tile.UnityRect;
            rect = LDtkCoordConverter.ImageSlice(src, tex.height);

            string texPath = AssetDatabase.GetAssetPath(tex);
            return texPath;
        }
        
        public PointParseData GetParsedPointData()
        {
            return new PointParseData()
            {
                LvlCellHeight = (int)Layer.CHei,
                PixelsPerUnit = Importer.PixelsPerUnit,
                GridSize = (int)Layer.GridSize,
                LevelPosition = Layer.LevelReference.UnityWorldSpaceCoord(_layout, Importer.PixelsPerUnit, _linearVector.Scaler)
            };
        }
    }
}