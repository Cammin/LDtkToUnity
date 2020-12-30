using System;
using LDtkUnity.BuildEvents.EntityEvents;
using LDtkUnity.Data;
using LDtkUnity.FieldInjection;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Builders
{
    public static class LDtkBuilderEntityInstance
    {
        public static GameObject BuildEntityLayerInstances(LDtkDataLayer layerData, LDtkProject project, int layerSortingOrder)
        {
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(layerData.__cHei);
            GameObject layerObj = new GameObject(layerData.__identifier);
            
            foreach (LDtkDataEntity entityData in layerData.entityInstances)
            {
                LDtkEntityAsset entityAsset = project.GetEntity(entityData.__identifier);
                if (entityAsset == null) continue;
                
                BuildEntityInstance(layerData, entityData, entityAsset, layerObj, layerSortingOrder);
            }

            return layerObj;
        }

        private static void BuildEntityInstance(LDtkDataLayer layerData, LDtkDataEntity entityData,
            LDtkEntityAsset entityAsset, GameObject layerObj, int layerSortingOrder)
        {
            int pixelsPerUnit = layerData.__gridSize;
            Vector2Int pixelPos = entityData.px.ToVector2Int();
            Vector2 spawnPos = (LDtkToolOriginCoordConverter.ConvertPosition(pixelPos, layerData.__cHei * pixelsPerUnit, pixelsPerUnit) / pixelsPerUnit) + Vector2.up;
            
            GameObject entityObj = InstantiateEntity(entityAsset.ReferencedAsset, spawnPos, layerObj);
            
            LDtkFieldInjector.InjectEntityFields(entityData, entityObj, layerData.__gridSize);

            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(layerSortingOrder));
            PostEntityInterfaceEvent<ILDtkSettableColor>(behaviors, e => e.OnLDtkSetEntityColor(entityData.Definition().Color()));
            PostEntityInterfaceEvent<ILDtkSettableOpacity>(behaviors, e => e.OnLDtkSetOpacity(layerData.__opacity));
        }

        private static GameObject InstantiateEntity(GameObject assetPrefab, Vector2 spawnPos, GameObject parentObj)
        {
            return Object.Instantiate(assetPrefab, spawnPos, Quaternion.identity, parentObj.transform);
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