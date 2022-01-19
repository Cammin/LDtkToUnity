using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkSectionEntities : LDtkSectionDataDrawer<EntityDefinition>
    {
        protected override string PropertyName => LDtkProjectImporter.ENTITIES;
        protected override string GuiText => "Entities";
        protected override string GuiTooltip => "Assign GameObjects that would be spawned as entities.\n" +
                                                "They will gain a component which stores LDtk fields if defined.\n" +
                                                "They will spawn at their pivot point, and have their scale adjusted if resized in LDtk.";
        protected override Texture GuiImage => LDtkIconUtility.LoadEntityIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_ENTITIES;

        public LDtkSectionEntities(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void GetDrawers(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers)
        {
            //if any entity tags are used in the project
            if (defs.Any(def => def.Tags.Any()))
            {
                AddTaggedGroups(defs, drawers);
                return;
            }

            AddSimpleList(defs, drawers);
        }

        private void AddSimpleList(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                LDtkDrawerEntity drawer = GetDrawerForEntity(defs, i);
                drawers.Add(drawer);
            }
        }

        private LDtkDrawerEntity GetDrawerForEntity(EntityDefinition[] defs, int i)
        {
            EntityDefinition entityData = defs[i];
            SerializedProperty entityProp = ArrayProp.GetArrayElementAtIndex(i);
            LDtkDrawerEntity drawer = new LDtkDrawerEntity(entityData, entityProp, entityData.Identifier);
            return drawer;
        }

        private delegate bool TagBouncer(string[] tags);
        
        private void AddTaggedGroups(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers)
        {
            //Debug.Log("tagged"); //todo this costs some performace for the UI. make better later or if possible
            
            //add the "untagged" group, only if there exists untagged entities
            if (defs.Any(p => p.Tags.IsNullOrEmpty()))
            {
                AddGroup(defs, drawers, "Untagged", tags => !tags.IsNullOrEmpty());
            }

            //add all other tagged groups
            List<string> uniqueTags = defs.SelectMany(p => p.Tags).Distinct().OrderBy(p => p).ToList();
            foreach (string tag in uniqueTags)
            {
                AddGroup(defs, drawers, tag, tags => !tags.Contains(tag));
            }
        }

        private void AddGroup(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers, string tag, TagBouncer bouncer)
        {
            List<LDtkDrawerEntity> groupDrawers = new List<LDtkDrawerEntity>();
            for (int i = 0; i < defs.Length; i++)
            {
                if (bouncer.Invoke(defs[i].Tags))
                {
                    continue;
                }

                LDtkDrawerEntity drawer = GetDrawerForEntity(defs, i);
                groupDrawers.Add(drawer);
            }

            LDtkDrawerEntityGroup group = new LDtkDrawerEntityGroup(null, ArrayProp, tag, groupDrawers);
            drawers.Add(group);
        }
    }
}