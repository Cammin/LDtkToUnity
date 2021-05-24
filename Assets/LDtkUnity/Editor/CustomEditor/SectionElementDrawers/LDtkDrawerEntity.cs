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

        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();

            GUIContent objectContent = new GUIContent()
            {
                text = _data.Identifier,
            };

            if (!_data.FieldDefs.IsNullOrEmpty())
            {
                objectContent.tooltip = string.Join("\n", _data.FieldDefs.Select(p => p.Identifier));
            }
            
            EditorGUI.PropertyField(controlRect, Value, objectContent);
            
            
            if (HasProblem())
            {
                DrawCachedProblem(controlRect);
            }

        }

        protected override string AssetUnassignedText => "No prefab assigned; Entity instance won't show up in the import result";
    }
}