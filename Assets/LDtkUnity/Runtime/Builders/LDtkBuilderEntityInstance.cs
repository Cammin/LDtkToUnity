using LDtkUnity.Runtime.Data.Level;
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
        public static void BuildEntityLayerInstances(LDtkDataLayer layerData, LDtkEntityAssetCollection entityAssets)
        {
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(layerData.__cHei);
            GameObject layerObj = new GameObject(layerData.__identifier);
            
            foreach (LDtkDataEntity entityData in layerData.entityInstances)
            {
                BuildEntityInstance(layerData, entityData, entityAssets, layerObj);
            }
        }

        private static void BuildEntityInstance(LDtkDataLayer layerData, LDtkDataEntity entityData, LDtkEntityAssetCollection entityAssets, GameObject layerObj)
        {
            LDtkEntityAsset entityAsset = entityAssets.GetAssetByIdentifier(entityData.__identifier);
            if (entityAsset == null) return;

            int pixelsPerUnit = layerData.__gridSize;
            Vector2Int pixelPos = entityData.px.ToVector2Int();
            Vector2 spawnPos = (LDtkToolOriginCoordConverter.ConvertPosition(pixelPos, layerData.__cHei * pixelsPerUnit, pixelsPerUnit) / pixelsPerUnit) + Vector2.up;
            InstantiateEntity(entityData, entityAsset.ReferencedAsset, spawnPos, layerObj);
        }

        private static void InstantiateEntity(LDtkDataEntity entityData, GameObject assetPrefab, Vector2 spawnPos,
            GameObject layerObj)
        {
            GameObject instance = Object.Instantiate(assetPrefab, spawnPos, Quaternion.identity, layerObj.transform);
            LDtkFieldInjector.InjectInstanceFields(entityData, instance);
        }
    }
}