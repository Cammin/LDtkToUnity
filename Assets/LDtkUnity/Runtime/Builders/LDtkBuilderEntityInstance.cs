using System;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.EntityCallbacks;
using LDtkUnity.Runtime.FieldInjection;
using LDtkUnity.Runtime.FieldInjection.ParsedField;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Entity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderEntityInstance
    {
        public static void BuildEntityLayerInstances(LDtkDataLayer layerData, LDtkEntityAssetCollection entityAssets,
            int layerSortingOrder)
        {
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(layerData.__cHei);
            GameObject layerObj = new GameObject(layerData.__identifier);
            
            foreach (LDtkDataEntity entityData in layerData.entityInstances)
            {
                BuildEntityInstance(layerData, entityData, entityAssets, layerObj, layerSortingOrder);
            }
        }

        private static void BuildEntityInstance(LDtkDataLayer layerData, LDtkDataEntity entityData,
            LDtkEntityAssetCollection entityAssets, GameObject layerObj, int layerSortingOrder)
        {
            LDtkEntityAsset entityAsset = entityAssets.GetAssetByIdentifier(entityData.__identifier);
            if (entityAsset == null) return;

            int pixelsPerUnit = layerData.__gridSize;
            Vector2Int pixelPos = entityData.px.ToVector2Int();
            Vector2 spawnPos = (LDtkToolOriginCoordConverter.ConvertPosition(pixelPos, layerData.__cHei * pixelsPerUnit, pixelsPerUnit) / pixelsPerUnit) + Vector2.up;
            
            GameObject entityObj = InstantiateEntity(entityAsset.ReferencedAsset, spawnPos, layerObj);
            
            LDtkFieldInjector.InjectInstanceFields(entityData, entityObj);

            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(layerSortingOrder));
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