using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        public LDtkDrawerEntity(EntityDefinition def, SerializedProperty prop, string key) : base(def, prop, key)
        {
        }

        public override bool HasProblem()
        {
            return false;
        }

        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            EditorGUI.PropertyField(controlRect, Value, ObjectContent());

            LDtkSectionDrawer.DenyPotentialResursiveGameObjects(Value);
            
            if (HasProblem())
            {
                DrawCachedProblem(controlRect);
            }

        }

        private GUIContent ObjectContent()
        {
            Texture2D copyTexture = new LDtkEntityIconFactory(_data).GetIcon();

            GUIContent objectContent = new GUIContent()
            {
                text = _data.Identifier,
                image = copyTexture
            };

            if (_data.FieldDefs.IsNullOrEmpty())
            {
                return objectContent;
            }
            
            IEnumerable<string> identifiers = _data.FieldDefs.Select(p => p.Identifier);
            objectContent.tooltip = $"Fields:\n{string.Join(", ", identifiers)}";
            return objectContent;
        }

        private void DrawValueColorBox(EntityDefinition data, Rect iconRect)
        {
            Color valueColor = data.UnityColor;
            EditorGUI.DrawRect(iconRect, valueColor);
        }

        

        protected override string AssetUnassignedText => "No prefab assigned; Entity instance won't show up in the import result";
    }
}