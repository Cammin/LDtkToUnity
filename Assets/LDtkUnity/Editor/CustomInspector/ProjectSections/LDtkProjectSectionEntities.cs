using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionEntities  : LDtkProjectSectionDrawer<EntityDefinition>
    {
        protected override string PropertyName => LDtkProject.ENTITIES;
        protected override string GuiText => "Entities";
        protected override string GuiTooltip => "Assign GameObjects that would be spawned as entities";
        protected override Texture2D GuiImage => LDtkIconLoader.LoadEntityIcon();
        
        
        
        public LDtkProjectSectionEntities(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void DrawDropdownContent(EntityDefinition[] datas)
        {
            HasProblem = !DrawEntities(datas);
        }

        private bool DrawEntities(EntityDefinition[] entities)
        {
            bool passed = true;
            for (int i = 0; i < entities.Length; i++)
            {
                EntityDefinition entityData = entities[i];
                SerializedProperty entityObj = ArrayProp.GetArrayElementAtIndex(i);

                LDtkDrawerEntity drawer = new LDtkDrawerEntity(entityObj, entityData.Identifier);
                
                if (drawer.HasError(entityData))
                {
                    passed = false;
                }
                
                drawer.Draw(entityData);

            }

            return passed;
        }
    }
}