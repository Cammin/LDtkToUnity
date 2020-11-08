using System.Collections.Generic;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection;
using LDtkUnity.Runtime.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    public static class LDtkEntityInstanceBuilder
    {
        public static void BuildEntityLayerInstances(IEnumerable<LDtkDataEntityInstance> entityInstances, LDtkEntityInstanceCollection suppliedInstanceArray, Vector2Int layerSize, int pixelsPerUnit)
        {
            Debug.Assert(suppliedInstanceArray != null, "EntityInstancePrefabCollection null");
            
            foreach (LDtkDataEntityInstance entity in entityInstances)
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

        private static void BuildEntityLayerInstance(LDtkDataEntityInstance entity, GameObject instancePrefab, Vector2Int layerSize, int pixelsPerUnit)
        {
            Vector2Int pixelPos = entity.px.ToVector2();
            Vector2 spawnPos = LDtkTileCoordTool.GetCorrectPixelCoord(pixelPos, layerSize, pixelsPerUnit);
            GameObject instance = Object.Instantiate(instancePrefab, spawnPos, Quaternion.identity);
            
            //Debug.Log($"LEd Spawned Instance {instancePrefab.name} at {spawnPos}");

            LDtkEntityInstanceFieldInjector.InjectInstanceFields(entity, instance);
        }
    }
}