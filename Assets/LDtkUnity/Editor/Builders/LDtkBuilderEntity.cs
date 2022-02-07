using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkBuilderEntity : LDtkBuilderLayer
    {
        public LDtkBuilderEntity(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder) : base(importer, layerGameObject, sortingOrder)
        {
        }

        //this is to maintain uniqueness in the import process
        private readonly Dictionary<string, int> _entitiesBuilt = new Dictionary<string, int>();
        
        public void BuildEntityLayerInstances()
        {
            if (Importer.DeparentInRuntime)
            {
                LayerGameObject.AddComponent<LDtkDetachChildren>();
            }
            
            SortingOrder.Next();

            LDtkFieldParser.CacheRecentBuilder(this);

            foreach (EntityInstance entityData in Layer.EntityInstances)
            {
                GameObject entityPrefab = Importer.GetEntity(entityData.Identifier);
                BuildEntityInstance(entityData, entityPrefab);
            }
        }

        private void BuildEntityInstance(EntityInstance entityData, GameObject entityPrefab)
        {
            GameObject entityObj;
            if (entityPrefab)
            {
                entityObj = LDtkPrefabFactory.Instantiate(entityPrefab);
                entityObj.name = GetEntityGameObjectName(entityPrefab.name);
            }
            else
            {
                entityObj = new GameObject();
                entityObj.name = GetEntityGameObjectName(entityData.Identifier);
            }

            PositionEntity(entityData, entityObj);
            ScaleEntity(entityData, entityObj);
            AddFieldData(entityData, entityObj);
        }

        private static Texture2D GetEntityImageAndRect(EntityDefinition entityDef, string assetPath, out Rect rect)
        {
            rect = Rect.zero;

            AtlasTileRectangle tile = entityDef.TileRect;
            if (tile == null)
            {
                return null;
            }
            
            LDtkRelativeGetterTilesetTexture textureGetter = new LDtkRelativeGetterTilesetTexture();
            Texture2D tex = textureGetter.GetRelativeAsset(entityDef.Tileset, assetPath);
            if (tex == null)
            {
                return null;
            }

            Rect src = tile.UnityRect;
            
            Vector2Int pos = new Vector2Int((int) src.position.x, (int) src.position.y);
            Vector2Int correctPos = LDtkCoordConverter.ImageSliceCoord(pos, tex.height, (int) src.height);
            
            Rect actualRect = new Rect(src)
            {
                position = correctPos,
            };

            rect = actualRect;
            return tex;
        }
        
        //could still be used in the future
        /*private Texture2D GetEnumImageAndRect(EnumDefinition enumDefinition, in Rect src, out Rect rect)
        {
            rect = Rect.zero;

            LDtkRelativeGetterTilesetTexture textureGetter = new LDtkRelativeGetterTilesetTexture();
            Texture2D tex = textureGetter.GetRelativeAsset(enumDefinition.IconTileset, Importer.assetPath);
            if (tex == null)
            {
                return null;
            }

            Vector2Int pos = new Vector2Int((int) src.position.x, (int) src.position.y);
            Vector2Int correctPos = LDtkCoordConverter.ImageSliceCoord(pos, tex.height, (int) src.height);
            
            Rect actualRect = new Rect(src)
            {
                position = correctPos,
            };

            rect = actualRect;
            return tex;
        }*/

        private void AddFieldData(EntityInstance entityData, GameObject entityObj)
        {
            LDtkFieldsFactory fieldsFactory = new LDtkFieldsFactory(entityObj, entityData.FieldInstances);
            fieldsFactory.SetEntityFieldsComponent();
            
            AddHandleDrawers(entityObj, fieldsFactory.FieldsComponent, entityData, (int)Layer.GridSize);
            
            InterfaceEvents(entityData, entityObj, fieldsFactory.FieldsComponent);
        }

        private void InterfaceEvents(EntityInstance entityData, GameObject entityObj, LDtkFields fields)
        {
            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            LDtkInterfaceEvent.TryEvent<ILDtkImportedLayer>(behaviors, e => e.OnLDtkImportLayer(Layer));

            if (fields != null)
            {
                LDtkInterfaceEvent.TryEvent<ILDtkImportedFields>(behaviors, e => e.OnLDtkImportFields(fields));
            }

            LDtkInterfaceEvent.TryEvent<ILDtkImportedEntity>(behaviors, e => e.OnLDtkImportEntity(entityData));
            LDtkInterfaceEvent.TryEvent<ILDtkImportedSortingOrder>(behaviors, e => e.OnLDtkImportSortingOrder(SortingOrder.SortingOrderValue));
        }

        private static void ScaleEntity(EntityInstance entityData, GameObject entityObj)
        {
            //modify by the resized entity scaling from LDtk
            Vector3 newScale = entityObj.transform.localScale;
            newScale.Scale(entityData.UnityScale);
            entityObj.transform.localScale = newScale;
        }

        private void PositionEntity(EntityInstance entityData, GameObject entityObj)
        {
            entityObj.transform.parent = LayerGameObject.transform;
            
            Vector2 localPos = LDtkCoordConverter.EntityLocalPosition(entityData.UnityPx, (int) Layer.LevelReference.PxHei, Importer.PixelsPerUnit);
            localPos += Layer.UnityWorldTotalOffset;
            
            entityObj.transform.localPosition = localPos;
        }

        /// <summary>
        /// Reason to give them unique names is to add them to the importer correctly. The importer requires unique identifiers 
        /// </summary>
        private string GetEntityGameObjectName(string identifier)
        {
            if (!_entitiesBuilt.ContainsKey(identifier))
            {
                _entitiesBuilt.Add(identifier, 0);
            }
            
            int amount = _entitiesBuilt[identifier];
            string name = $"{identifier}_{amount}";
            _entitiesBuilt[identifier]++;
            return name;
        }

        /// <summary>
        /// Only doing this for importer performance. an early cut
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
                    
                case EditorDisplayMode.EntityTile: //enum/enum array
                    return field.IsEnum;

                case EditorDisplayMode.RadiusGrid: //int, float
                case EditorDisplayMode.RadiusPx: //int, float
                    return field.IsInt || field.IsFloat;

                case EditorDisplayMode.PointStar: //point, point array
                    return field.IsPoint;
                    
                case EditorDisplayMode.Points: //point array only
                case EditorDisplayMode.PointPath: //point array only
                case EditorDisplayMode.PointPathLoop: //point array only
                    return field.IsPoint && field.Definition.IsArray;
                
                
                case EditorDisplayMode.ArrayCountNoLabel: //todo this is new and may need attention
                case EditorDisplayMode.ArrayCountWithLabel: //todo this is new and may need attention
                case EditorDisplayMode.RefLinkBetweenCenters: //todo this is new and may need attention
                case EditorDisplayMode.RefLinkBetweenPivots: //todo this is new and may need attention
                    return false;
                    
                default:
                    Debug.LogError("LDtk: No Drawer eligibility found!");
                    return false;
            }
        }
        
        private void AddHandleDrawers(GameObject gameObject, LDtkFields fields, EntityInstance entityData, int gridSize)
        {
            LDtkEntityDrawerComponent drawerComponent = gameObject.gameObject.AddComponent<LDtkEntityDrawerComponent>();
            EntityDefinition entityDef = entityData.Definition;

            Texture2D entityImage = GetEntityImageAndRect(entityDef, Importer.assetPath, out Rect entityIconRect);
            Vector2 size = (Vector2)entityData.UnitySize / (int)Layer.GridSize;

            Color handlesColor = fields != null && fields.GetSmartColor(out Color firstColor) ? firstColor : entityDef.UnityColor; 
            
            //entity handle data
            LDtkEntityDrawerData entityDrawerData = new LDtkEntityDrawerData(drawerComponent.transform, entityDef, entityImage, entityIconRect, size, handlesColor);
            drawerComponent.AddEntityDrawer(entityDrawerData);

            foreach (FieldInstance fieldInstance in entityData.FieldInstances)
            {
                if (!DrawerEligibility(fieldInstance))
                {
                    continue;
                }
                
                /*Texture2D iconTex = null;
                Rect rect = Rect.zero;
                
                if (displayMode == EditorDisplayMode.ValueOnly || displayMode == EditorDisplayMode.NameAndValue)
                {
                    //iconTex = GetEnumImageAndRect(fieldInstance., entityData.Tile.UnitySourceRect, out Rect iconRect); //todo
                }*/

                EditorDisplayMode displayMode = fieldInstance.Definition.EditorDisplayMode;
                Vector2 pivotOffset = LDtkCoordConverter.EntityPivotOffset(entityDef.UnityPivot, size);
                Vector3 middleCenter = gameObject.transform.position + (Vector3)pivotOffset;
                
                LDtkFieldDrawerData data = new LDtkFieldDrawerData(fields, handlesColor, displayMode, fieldInstance.Identifier, gridSize, middleCenter);
                drawerComponent.AddReference(data);
            }
        }
        
        public PointParseData GetParsedPointData()
        {
            return new PointParseData()
            {
                LvlCellHeight = (int)Layer.CHei,
                PixelsPerUnit = Importer.PixelsPerUnit,
                GridSize = (int)Layer.GridSize,
                LevelPosition = Layer.LevelReference.UnityWorldSpaceCoord(Importer.PixelsPerUnit)
            };
        }
    }
}