using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionLevelFields  : LDtkProjectSectionDrawer<FieldDefinition>
    {
        protected override string PropertyName => "";
        protected override string GuiText => "Level Properties";
        protected override string GuiTooltip => "Level Properties";
        protected override Texture GuiImage => LDtkIconLoader.LoadEnumIcon();
        
        protected override void GetDrawers(FieldDefinition[] defs, List<LDtkContentDrawer<FieldDefinition>> drawers)
        {
              
        }

        protected override void DrawDropdownContent(FieldDefinition[] datas)
        {
            foreach (FieldDefinition definition in datas)
            {
                EditorGUILayout.LabelField(definition.Identifier);
                EditorGUILayout.LabelField(definition.Type);
                //EditorGUILayout.LabelField(definition.DefaultOverride.ToString());
            }
        }


        public LDtkProjectSectionLevelFields(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        
    }
}