using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.FieldInjection;
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
            foreach (LDtkDataEntity entityData in layerData.entityInstances)
            {
                BuildEntityInstance(layerData, entityData, entityAssets);
            }
        }

        private static void BuildEntityInstance(LDtkDataLayer layerData, LDtkDataEntity entityData, LDtkEntityAssetCollection entityAssets)
        {
            LDtkEntityAsset entityAsset = entityAssets.GetAssetByIdentifier(entityData.__identifier);
            if (entityAsset == null) return;

            Vector2Int pixelPos = entityData.px.ToVector2Int();
            Vector2 spawnPos = LDtkToolTileCoord.GetCorrectPixelCoord(pixelPos, layerData.CellSize, layerData.__gridSize);
            InstantiateEntity(entityData, entityAsset.ReferencedAsset, spawnPos, entityAssets.InjectFields);
        }

        private static void InstantiateEntity(LDtkDataEntity entityData, GameObject assetPrefab, Vector2 spawnPos, bool injectFields)
        {
            GameObject instance = Object.Instantiate(assetPrefab, spawnPos, Quaternion.identity);
            
            if (injectFields)
            {
                LDtkFieldInjector.InjectInstanceFields(entityData, instance);
            }
        }
    }
}