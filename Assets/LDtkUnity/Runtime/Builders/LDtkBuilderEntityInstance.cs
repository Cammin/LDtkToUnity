using System.Collections.Generic;
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
        public static void BuildEntityLayerInstances(IEnumerable<LDtkDataEntity> entityInstances, LDtkEntityInstanceCollection suppliedInstanceArray, Vector2Int layerSize, int pixelsPerUnit)
        {
            Debug.Assert(suppliedInstanceArray != null, "EntityInstancePrefabCollection null");
            
            foreach (LDtkDataEntity entity in entityInstances)
            {
                UnityAssets.Entity.LDtkEntityInstance lDtkEntity = suppliedInstanceArray.GetAssetByIdentifier(entity.__identifier);
                
                if (lDtkEntity == null)
                {
                    Debug.LogError($"Could not get the requested prefab named \"{entity.__identifier}\". Is the asset available or the identifier wrong?");
                    continue;
                }
                if (lDtkEntity.Asset == null)
                {
                    Debug.LogError($"{entity.__identifier}'s prefab was null. Asset unassigned?");
                    continue;
                }
                
                BuildEntityLayerInstance(entity, lDtkEntity.Asset, layerSize, pixelsPerUnit);
            }
        }

        private static void BuildEntityLayerInstance(LDtkDataEntity entity, GameObject instancePrefab, Vector2Int layerSize, int pixelsPerUnit)
        {
            Vector2Int pixelPos = entity.px.ToVector2Int();
            Vector2 spawnPos = LDtkToolTileCoord.GetCorrectPixelCoord(pixelPos, layerSize, pixelsPerUnit);
            GameObject instance = Object.Instantiate(instancePrefab, spawnPos, Quaternion.identity);
            
            //Debug.Log($"LDtk Spawned Instance {instancePrefab.name} at {spawnPos}");

            LDtkFieldInjector.InjectInstanceFields(entity, instance);
        }
    }
}