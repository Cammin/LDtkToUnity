using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
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
            Texture2D icon = null;
            switch (_data.RenderMode)
            {
                case RenderMode.Cross:
                    icon = LDtkIconUtility.LoadCrossIcon();
                    break;
                case RenderMode.Ellipse:
                    icon = LDtkIconUtility.LoadCircleIcon();
                    break;
                case RenderMode.Rectangle:
                    icon = LDtkIconUtility.LoadSquareIcon();
                    break;
                case RenderMode.Tile:
                    //todo get the cool icon that is used for the scene drawer
                    icon = LDtkIconUtility.LoadSquareIcon();
                    break;
            }
            
            Texture2D copyTexture = null;
            if (icon != null)
            {
                copyTexture = TintImage(icon, _data.UnityColor);
            }

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

        private Texture2D TintImage(Texture2D texture, Color tint)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                Color color = pixels[i];
                Color newColor = new Color(tint.r, tint.g, tint.b, color.a);
                pixels[i] = newColor;
            }
            
            Texture2D copyTexture = new Texture2D(texture.width, texture.height);
            copyTexture.SetPixels(pixels);
            copyTexture.Apply();
            return copyTexture;
        }

        protected override string AssetUnassignedText => "No prefab assigned; Entity instance won't show up in the import result";
    }
}