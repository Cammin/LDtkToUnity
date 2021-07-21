using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkSectionEntities : LDtkSectionDataDrawer<EntityDefinition>
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
            for (int i = 0; i < defs.Length; i++)
            {
                EntityDefinition entityData = defs[i];
                SerializedProperty entityObj = ArrayProp.GetArrayElementAtIndex(i);

                LDtkDrawerEntity drawer = new LDtkDrawerEntity(entityData, entityObj, entityData.Identifier);
                
                drawers.Add(drawer);
            }
        }
    }
}