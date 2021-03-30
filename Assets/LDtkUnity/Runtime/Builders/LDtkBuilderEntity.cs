using System;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkBuilderEntity : LDtkLayerBuilder
    {
        public LDtkBuilderEntity(LayerInstance layer, LDtkProject project) : base(layer, project)
        {
        }
        
        public GameObject BuildEntityLayerInstances(int layerSortingOrder)
        {
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(Layer.UnityWorldPosition, (int)Layer.CHei);
            GameObject layerObj = new GameObject(Layer.Identifier);
            
            foreach (EntityInstance entityData in Layer.EntityInstances)
            {
                GameObject entityPrefab = Project.GetEntity(entityData.Identifier);
                if (entityPrefab == null)
                {
                    continue;
                }

                BuildEntityInstance(entityData, entityPrefab, layerObj, layerSortingOrder);
            }

            return layerObj;
        }

        private GameObject BuildEntityInstance(EntityInstance entityData, GameObject entityPrefab, GameObject layerObj, int layerSortingOrder)
        {
            GameObject entityObj = LDtkPrefabFactory.Instantiate(entityPrefab);
            entityObj.name = entityPrefab.name;
            
            entityObj.transform.parent = layerObj.transform;
            entityObj.transform.localPosition = LDtkToolOriginCoordConverter.EntityLocalPosition(entityData.UnityPx, (int)Layer.LevelReference.PxHei, (int)Layer.GridSize);
            
            //modify by the resized entity scaling from LDtk
            Vector3 newScale = entityObj.transform.localScale;
            newScale.Scale(entityData.UnityScale);
            entityObj.transform.localScale = newScale;

            LDtkFieldInjector fieldInjector = new LDtkFieldInjector(entityObj, entityData.FieldInstances);
            fieldInjector.InjectEntityFields();


            foreach (InjectorDataPair injectorData in fieldInjector.InjectorData)
            {
                TryAddPointDrawer(injectorData.Data, injectorData.Field, entityData, (int)Layer.GridSize);
            }
            
            
            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(layerSortingOrder));
            PostEntityInterfaceEvent<ILDtkSettableColor>(behaviors, e => e.OnLDtkSetEntityColor(entityData.Definition.UnityColor));
            PostEntityInterfaceEvent<ILDtkSettableOpacity>(behaviors, e => e.OnLDtkSetOpacity((float)Layer.Opacity));

            foreach (MonoBehaviour monoBehaviour in behaviors)
            {
                LDtkEditorUtil.Dirty(monoBehaviour);
            }
            
            //record transform for the object's scale
            LDtkEditorUtil.Dirty(entityObj.transform);
            LDtkEditorUtil.Dirty(entityObj);
            
            return entityObj;
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
        
        private static void TryAddPointDrawer(FieldInstance fieldData, LDtkFieldInjectorData fieldToInjectInto, EntityInstance entityData, int gridSize)
        {
            if (!DrawerEligibility(fieldData.Definition.EditorDisplayMode, fieldToInjectInto.Info.FieldType))
            {
                return;
            }

            Component component = (Component)fieldToInjectInto.ObjectRef;
            
            LDtkSceneDrawer drawer = component.gameObject.AddComponent<LDtkSceneDrawer>();
            
            EditorDisplayMode displayMode = fieldData.Definition.EditorDisplayMode;
            drawer.SetReference(component, fieldToInjectInto.Info, entityData, displayMode, gridSize);

            LDtkEditorUtil.Dirty(drawer);
        }
    }
}