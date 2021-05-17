using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkBuilderEntity : LDtkLayerBuilder
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
            
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(Layer.UnityWorldPosition, (int)Layer.CHei);

            foreach (EntityInstance entityData in Layer.EntityInstances)
            {
                GameObject entityPrefab = Importer.GetEntity(entityData.Identifier);
                if (entityPrefab == null)
                {
                    continue;
                }

                BuildEntityInstance(entityData, entityPrefab);
            }
        }

        private void BuildEntityInstance(EntityInstance entityData, GameObject entityPrefab)
        {
            GameObject entityObj = LDtkPrefabFactory.Instantiate(entityPrefab);
            entityObj.name = GetEntityGameObjectName(entityPrefab.name);

            PositionEntity(entityData, entityObj);
            ScaleEntity(entityData, entityObj);
            AddFieldData(entityData, entityObj);
        }

        private void AddFieldData(EntityInstance entityData, GameObject entityObj)
        {
            LDtkFieldInjector fieldInjector = new LDtkFieldInjector(entityObj, entityData.FieldInstances);
            fieldInjector.InjectEntityFields();

            //TODO add drawers back. probably make it a scene drawer section of the component drawer of LDtkFields
            /*foreach (InjectorDataPair injectorData in fieldInjector.InjectorData)
            {
                TryAddPointDrawer(injectorData.Data, injectorData.Field, entityData, (int)Layer.GridSize);
            }*/


            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();

            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(SortingOrder.SortingOrderValue));
            PostEntityInterfaceEvent<ILDtkSettableColor>(behaviors, e => e.OnLDtkSetEntityColor(entityData.Definition.UnityColor));
            PostEntityInterfaceEvent<ILDtkSettableOpacity>(behaviors, e => e.OnLDtkSetOpacity((float) Layer.Opacity));
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
            entityObj.transform.localPosition = LDtkToolOriginCoordConverter.EntityLocalPosition(entityData.UnityPx, (int) Layer.LevelReference.PxHei, (int) Layer.GridSize);
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



        private void PostEntityInterfaceEvent<T>(MonoBehaviour[] behaviors, Action<T> action)
        {
            foreach (MonoBehaviour component in behaviors)
            {
                if (!(component is T thing)) continue;
                
                try
                {
                    action.Invoke(thing);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
        
        private static bool DrawerEligibility(EditorDisplayMode? mode, Type type)
        {
            switch (mode)
            {
                case null:
                    return false;
                case EditorDisplayMode.RadiusGrid:
                case EditorDisplayMode.RadiusPx:
                {
                    if (type == typeof(int) || type == typeof(float))
                    {
                        return true;
                    }

                    break;
                }
                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                {
                    if (type == typeof(Vector2) || type == typeof(Vector2[]))
                    {
                        return true;
                    }

                    break;
                }
            }

            return false;
        }
        
        /*private static void TryAddPointDrawer(FieldInstance fieldData, LDtkFieldInjectorData fieldToInjectInto, EntityInstance entityData, int gridSize)
        {
            if (!DrawerEligibility(fieldData.Definition.EditorDisplayMode, fieldToInjectInto.Info.FieldType))
            {
                return;
            }

            Component component = (Component)fieldToInjectInto.ObjectRef;
            
            LDtkSceneDrawer drawer = component.gameObject.GetComponent<LDtkSceneDrawer>();
            if (drawer == null)
            {
                drawer = component.gameObject.AddComponent<LDtkSceneDrawer>();
            }
            
            EditorDisplayMode displayMode = fieldData.Definition.EditorDisplayMode;
            
            LDtkSceneDrawerData data = new LDtkSceneDrawerData(component, fieldToInjectInto.Info, entityData, displayMode, gridSize);
            
            drawer.AddReference(data);
        }*/


    }
}