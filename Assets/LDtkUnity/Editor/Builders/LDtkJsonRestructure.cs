using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //editing the json structure 
    internal static class LDtkJsonRestructure
    {
        public static void Restructure(LdtkJson json, string assetPath)
        {
            RestructureDeprecatedLevelsIntoWorld(json);
            RestructureForExternalLevels(json, assetPath);
            
            ReorderFieldInstances(json);
        }
        
        private static void RestructureDeprecatedLevelsIntoWorld(LdtkJson json)
        {
            if (!json.Worlds.IsNullOrEmpty())
            {
                return;
            }
            
            json.Worlds = new World[] { json.DeprecatedWorld };
            json.Levels = Array.Empty<Level>();
        }

        //requires that the worlds are using the new levels array
        private static void RestructureForExternalLevels(LdtkJson project, string assetPath)
        {
            if (!project.ExternalLevels)
            {
                return;
            }
            
            foreach (World world in project.Worlds)
            {
                RestructureWorldWithExternalLevels(world, assetPath);
            }
        }

        private static void RestructureWorldWithExternalLevels(World world, string assetPath)
        {
            List<Level> newLevels = new List<Level>();
            LDtkRelativeGetterLevels finderLevels = new LDtkRelativeGetterLevels();
            
            string[] levelFiles = world.Levels.Select(lvl => finderLevels.ReadRelativeText(lvl, assetPath)).ToArray();
            
            foreach (string json in levelFiles)
            {
                if (json == null)
                {
                    Debug.LogError("LDtk: Level file was null, ignored. May cause problems?");
                    continue;
                }
                
                Level level = Level.FromJson(json);
                Assert.IsNotNull(level);
                newLevels.Add(level);
            }
            
            world.Levels = newLevels.ToArray();
        }
        
        /// <summary>
        /// this is a hack to fix a field definition/instance ordering disparity.
        /// This function requires that the external levels are set back into the json first to get the layer instances correctly.
        /// https://github.com/deepnight/ldtk/issues/609
        /// todo promptly remove this once the issue is resolved
        /// </summary>
        private static void ReorderFieldInstances(LdtkJson json)
        {
            LDtkUidBank.CacheUidData(json);

            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels)
                {
                    level.FieldInstances = GetReordered(level.FieldInstances, json.Defs.LevelFields);

                    LayerInstance[] layers = level.LayerInstances;
                    if (layers == null)
                    {
                        Debug.LogError($"layer instances were null? for level: {level.Identifier}");
                        continue;
                    }
                
                    IEnumerable<LayerInstance> entityLayers = layers.Where(p => p.IsEntitiesLayer);
                    IEnumerable<EntityInstance> entities = entityLayers.SelectMany(p => p.EntityInstances);

                    foreach (EntityInstance entity in entities)
                    {
                        entity.FieldInstances = GetReordered(entity.FieldInstances, entity.Definition.FieldDefs);
                    }
                }
            }
            
            LDtkUidBank.ReleaseDefinitions();
        }

        private static FieldInstance[] GetReordered(FieldInstance[] formerInstances, FieldDefinition[] defs)
        {
            Dictionary<string, FieldInstance> instances = new Dictionary<string, FieldInstance>();
            foreach (FieldInstance fieldInst in formerInstances)
            {
                instances.Add(fieldInst.Identifier, fieldInst);
            }
            
            FieldInstance[] newInstances = new FieldInstance[instances.Values.Count];

            for (int i = 0; i < defs.Length; i++)
            {
                FieldDefinition fieldDef = defs[i];
                if (!instances.ContainsKey(fieldDef.Identifier))
                {
                    Debug.LogError("LDtk: Could not reorder field instances to match definition order");
                    return formerInstances;
                }

                FieldInstance instance = instances[fieldDef.Identifier];
                newInstances[i] = instance;
            }

            return newInstances;
        }
    }
}