using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSectionEntities : LDtkSectionDataDrawer<EntityDefinition>
    {
        protected override string PropertyName => LDtkProjectImporter.ENTITIES;
        protected override string GuiText => "Entities";
        protected override string GuiTooltip => "Assign GameObjects that would be spawned as entities.\n" +
                                                "They will gain a component which stores LDtk fields if defined.\n" +
                                                "They will spawn at their pivot point, and have their scale adjusted if resized in LDtk.";
        protected override Texture GuiImage => LDtkIconUtility.LoadEntityIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_ENTITIES;

        public LDtkSectionEntities(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
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
        
        private void AddTaggedGroups(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers)
        {
            TryAddUntaggedGroup(defs, drawers);
            TryAddTaggedGroup(defs, drawers);
        }
        

        private LDtkDrawerEntity GetDrawerForEntity(EntityDefinition[] defs, int i)
        {
            if (i >= ArrayProp.arraySize)
            {
                LDtkDebug.LogError("Array index out of bounds, the serialized array likely wasn't constructed properly");
                return null;
            }
            
            EntityDefinition entityData = defs[i];
            SerializedProperty entityProp = ArrayProp.GetArrayElementAtIndex(i);
            LDtkDrawerEntity drawer = new LDtkDrawerEntity(entityData, entityProp);
            return drawer;
        }

        private delegate bool TagBouncer(string[] tags);
        
        private void TryAddTaggedGroup(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers)
        {
            //add all other tagged groups
            List<string> uniqueTags = defs.SelectMany(p => p.Tags).Distinct().OrderBy(p => p).ToList();
            foreach (string tag in uniqueTags)
            {
                List<LDtkDrawerEntity> groupDrawers = CreateGroup(defs, tags => !tags.Contains(tag));
                AddGroup(drawers, tag, groupDrawers);
            }
        }

        private void TryAddUntaggedGroup(EntityDefinition[] defs, List<LDtkContentDrawer<EntityDefinition>> drawers)
        {
            //add the "untagged" group, only if there exists untagged entities
            if (defs.Any(p => p.Tags.IsNullOrEmpty()))
            {
                List<LDtkDrawerEntity> groupDrawers = CreateGroup(defs, tags => !tags.IsNullOrEmpty());
                AddGroup(drawers, "Untagged", groupDrawers);
            }
        }

        private void AddGroup(List<LDtkContentDrawer<EntityDefinition>> drawers, string tag, List<LDtkDrawerEntity> groupDrawers)
        {
            LDtkGroupDrawerEntity group = new LDtkGroupDrawerEntity(null, ArrayProp, tag, groupDrawers);
            group.InitDrawers();
            drawers.Add(group);
        }

        private List<LDtkDrawerEntity> CreateGroup(EntityDefinition[] defs, TagBouncer bouncer)
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
            return groupDrawers;
        }
    }
}