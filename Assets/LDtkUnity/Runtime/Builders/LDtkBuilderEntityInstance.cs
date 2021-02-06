using System;
using LDtkUnity.BuildEvents.EntityEvents;
using LDtkUnity.FieldInjection;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Builders
{
    public static class LDtkBuilderEntityInstance
    {
        public static GameObject BuildEntityLayerInstances(LayerInstance layerData, LDtkProject project, int layerSortingOrder)
        {
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(layerData.UnityWorldPosition, (int)layerData.CHei);
            GameObject layerObj = new GameObject(layerData.Identifier);
            
            foreach (EntityInstance entityData in layerData.EntityInstances)
            {
                LDtkEntityAsset entityAsset = project.GetEntity(entityData.Identifier);
                if (entityAsset == null) continue;
                
                BuildEntityInstance(layerData, entityData, entityAsset, layerObj, layerSortingOrder);
            }

            return layerObj;
        }

        private static void BuildEntityInstance(LayerInstance layerData, EntityInstance entityData,
            LDtkEntityAsset entityAsset, GameObject layerObj, int layerSortingOrder)
        {
            Vector2 localPos = LDtkToolOriginCoordConverter.EntityLocalPosition(entityData.UnityPx, (int)layerData.LevelReference.PxHei, (int)layerData.GridSize);
            //Debug.Log(localPos);

            GameObject entityObj = Object.Instantiate(entityAsset.ReferencedAsset, layerObj.transform, false);
            entityObj.transform.localPosition = localPos;
            entityObj.name = entityAsset.ReferencedAsset.name;
            
            LDtkFieldInjector.InjectEntityFields(entityData, entityObj, (int)layerData.GridSize);

            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(layerSortingOrder));
            PostEntityInterfaceEvent<ILDtkSettableColor>(behaviors, e => e.OnLDtkSetEntityColor(entityData.Definition.UnityColor));
            PostEntityInterfaceEvent<ILDtkSettableOpacity>(behaviors, e => e.OnLDtkSetOpacity((float)layerData.Opacity));
        }

        private static void PostEntityInterfaceEvent<T>(MonoBehaviour[] behaviors, Action<T> action)
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