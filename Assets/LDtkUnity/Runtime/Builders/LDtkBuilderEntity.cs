using System;
using LDtkUnity.BuildEvents.EntityEvents;
using LDtkUnity.FieldInjection;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Builders
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

        private void BuildEntityInstance(EntityInstance entityData, GameObject entityPrefab, GameObject layerObj, int layerSortingOrder)
        {
            Vector2 localPos = LDtkToolOriginCoordConverter.EntityLocalPosition(entityData.UnityPx, (int)Layer.LevelReference.PxHei, (int)Layer.GridSize);
            

            GameObject entityObj = InstantiateEntity(entityPrefab, layerObj, localPos);

            LDtkFieldInjector.InjectEntityFields(entityData, entityObj, (int)Layer.GridSize);

            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(layerSortingOrder));
            PostEntityInterfaceEvent<ILDtkSettableColor>(behaviors, e => e.OnLDtkSetEntityColor(entityData.Definition.UnityColor));
            PostEntityInterfaceEvent<ILDtkSettableOpacity>(behaviors, e => e.OnLDtkSetOpacity((float)Layer.Opacity));
        }

        private GameObject InstantiateEntity(GameObject entityPrefab, GameObject layerObj, Vector2 localPos)
        {
            GameObject entityObj = LDtkPrefabFactory.Instantiate(entityPrefab);

            entityObj.transform.parent = layerObj.transform;
            entityObj.transform.localPosition = localPos;
            entityObj.name = entityPrefab.name;
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

        
    }
}