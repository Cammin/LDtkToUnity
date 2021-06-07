using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        public LDtkDrawerEntity(EntityDefinition def, SerializedProperty obj, string key) : base(def, obj, key)
        {
        }

        public override bool HasProblem()
        {
            return false;
        }

        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();

            GUIContent objectContent = new GUIContent()
            {
                text = _data.Identifier,
            };

            if (!_data.FieldDefs.IsNullOrEmpty())
            {
                IEnumerable<string> identifiers = _data.FieldDefs.Select(p => p.Identifier);
                objectContent.tooltip = $"Fields:\n{string.Join(", ", identifiers)}";
            }
            
            EditorGUI.PropertyField(controlRect, Value, objectContent);
            LDtkSectionDrawer.DenyPotentialResursiveGameObjects(Value);
            
            if (HasProblem())
            {
                DrawCachedProblem(controlRect);
            }

        }

        protected override string AssetUnassignedText => "No prefab assigned; Entity instance won't show up in the import result";
    }
}