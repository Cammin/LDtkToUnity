using System;
using System.Collections.Generic;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkIntGridValueAsset))]
    public class LDtkIntGridValueAssetEditor : UnityEditor.Editor
    {
        private const int MAX_PREVIEW_IMAGE_SIZE_X = 100;
        private const int MAX_PREVIEW_IMAGE_SIZE_Y = 100;
        private const int SIZE_SCALAR = 10;
        
        private Texture2D _physicsPreview;
        

 
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_physicsPreview == null) TryCreatePreview();
            if (_physicsPreview == null) return;

            DrawPreview();
        }

        private void DrawPreview()
        {
            Rect rect = new Rect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight*2, _physicsPreview.width, _physicsPreview.height);
            //EditorGUI.DrawRect(rect, Color.white);
            EditorGUI.DrawTextureTransparent(rect, _physicsPreview);
        }

        private void TryCreatePreview()
        {
            LDtkIntGridValueAsset asset = target as LDtkIntGridValueAsset;
            Sprite sprite = asset.ReferencedAsset;

            //make dimensions, but only as big as specified
            int width = Mathf.Min(sprite.texture.width * SIZE_SCALAR, MAX_PREVIEW_IMAGE_SIZE_X);
            int height = Mathf.Min(sprite.texture.height * SIZE_SCALAR, MAX_PREVIEW_IMAGE_SIZE_Y);
            
            //coords array setup
            bool[][] coords = new bool[width][];
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = new bool[height];
            }
            
            int shapeCount = sprite.GetPhysicsShapeCount();
            for (int shapeIdx = 0; shapeIdx < shapeCount; shapeIdx++)
            {
                List<Vector2> points = new List<Vector2>();
                int pointCount = sprite.GetPhysicsShape(shapeIdx, points);

                if (pointCount <= 1)
                {
                    continue;
                }

                //set lines for start to end
                for (int pointIndex = 0; pointIndex < points.Count-1; pointIndex++)
                {
                    Vector2Int point = ToVector2Int(points[pointIndex]);
                    Vector2Int nextPoint = ToVector2Int(points[pointIndex+1]);

                    SetLine(point, nextPoint);
                }
                //wrap start to end
                {
                    Vector2Int start = ToVector2Int(points[0]);
                    Vector2Int end = ToVector2Int(points[points.Count-1]);
                    SetLine(start, end);
                }

                void SetLine(Vector2Int start, Vector2Int end)
                {
                    float diagLength = (float)Math.Sqrt(width * height);
                    for (int j = 0; j < diagLength; j++)
                    {
                        float interpolation = (j / diagLength);
                        Vector2 point = Vector2.Lerp(start, end, interpolation);
                        Vector2Int roundedPoint = ToVector2Int(point);

                        coords[roundedPoint.x][roundedPoint.y] = true;
                    }
                }
            }

            //make sprite
            Texture2D previewTex = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isLinePixel = coords[x][y];
                    Color color = isLinePixel ? Color.black : Color.white;
                    previewTex.SetPixel(x, y, color);
                }
            }
            _physicsPreview = previewTex;
        }

        private Vector2Int ToVector2Int(Vector2 vector)
        {
            return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        }
    }
}
