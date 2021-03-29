using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionEntities : LDtkProjectSectionDrawer<EntityDefinition>
    {
        protected override string PropertyName => LDtkProject.ENTITIES;
        protected override string GuiText => "Entities";
        protected override string GuiTooltip => "Assign GameObjects that would be spawned as entities";
        protected override Texture GuiImage => LDtkIconLoader.LoadEntityIcon();
        
        public LDtkProjectSectionEntities(SerializedObject serializedObject) : base(serializedObject)
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