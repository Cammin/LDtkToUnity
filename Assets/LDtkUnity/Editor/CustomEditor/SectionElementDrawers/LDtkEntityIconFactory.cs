using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkEntityIconFactory
    {
        private readonly EntityDefinition _data;
        
        public LDtkEntityIconFactory(EntityDefinition def)
        {
            _data = def;
        }

        public Texture2D GetIcon()
        {
            Texture2D srcIcon = GetIconForRenderType();
            if (srcIcon == null)
            {
                Debug.LogError("LDtk: Did not get source icon for entity icon");
                return null;
            }
            
            return ProcessImage(srcIcon, _data.UnityColor);
        }
        
        private Texture2D GetIconForRenderType()
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
                    //todo get the cool icon that is used for the scene drawer (when entities can eventually get its tile through definition, not just instance)
                    icon = LDtkIconUtility.LoadSquareIcon();
                    break;
            }
            return icon;
        }
        
        private Texture2D ProcessImage(Texture2D srcTex, Color tint)
        {
            Vector2Int srcSize = new Vector2Int(srcTex.width, srcTex.height);
            Vector2Int newSize = ResizeTextureToScale(srcSize);
            Texture2D newTex = new Texture2D(newSize.x, newSize.y);

            int pixelAmount = newSize.x * newSize.y;
            Color[] clearColors = GetClearPixels(pixelAmount);

            Color[] pixels = GetTintColorForTexture(srcTex, tint);
            try
            {
                newTex.SetPixels(clearColors);
                
                Vector2Int padding = (newSize - srcSize) / 2;
                newTex.SetPixels(padding.x, padding.y, srcSize.x, srcSize.y, pixels);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            newTex.Apply();
            return newTex;
        }

        private static Color[] GetTintColorForTexture(Texture2D srcTex, Color tint)
        {
            Color[] pixels = srcTex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                Color color = pixels[i];
                Color newColor = new Color(tint.r, tint.g, tint.b, color.a);
                pixels[i] = newColor;
            }

            return pixels;
        }

        private static Color[] GetClearPixels(int pixelAmount)
        {
            Color[] clearColors = new Color[pixelAmount];
            for (int i = 0; i < clearColors.Length; i++)
            {
                clearColors[i] = Color.clear;
            }

            return clearColors;
        }

        private Vector2Int ResizeTextureToScale(Vector2Int initial)
        {
            int finalX = initial.x;
            int finalY = initial.y;

            //expand width
            if (_data.Height > _data.Width)
            {
                float scaleUp = (float)_data.Height / _data.Width;
                finalX = Mathf.FloorToInt(finalX * scaleUp);
            }
            
            //expand height
            else if (_data.Height < _data.Width)
            {
                float scaleUp = _data.Width / (float)_data.Height;
                finalY = Mathf.FloorToInt(finalY * scaleUp);
            }

            return new Vector2Int(finalX, finalY);
        }
    }
}