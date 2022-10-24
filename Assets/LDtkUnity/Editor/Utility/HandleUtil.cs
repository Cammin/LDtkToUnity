using UnityEditor;
using UnityEngine;
using Rect = UnityEngine.Rect;

namespace LDtkUnity.Editor
{
    internal static class HandleUtil
    {
        public static Rect GetNormalizedTextureCoords(Texture tex, Rect srcPx)
        {
            return new Rect
            {
                x = srcPx.x / tex.width,
                y = srcPx.y / tex.height,
                width = srcPx.width / tex.width,
                height = srcPx.height / tex.height,
            };
        }

        public static float GetIconGUISize(Vector3 worldPosition, Vector2 pxSize)
        {
            const int maxResolution = 32;
            
            int maxVector = Mathf.Max((int)pxSize.x, (int)pxSize.y, 1);
            if (maxVector >= maxResolution)
            {
                return maxResolution;
            }

            int scale = 1 + Mathf.FloorToInt(((float)maxResolution-9) / maxVector);
            return maxVector * scale;
        }

        public static Vector2 GetPositionForWorldPointSizedRect(Vector2 textArea, bool forTextArea = true)
        {
            float origX = textArea.x;
            
#if UNITY_2021_3_OR_NEWER
            textArea.x += 1;
            textArea.y -= 1;
#elif UNITY_2021_2_OR_NEWER //todo this might be completely wrong. I've noticed that a bad offset is applied when i try using the searchbar in the hierarchy window
            textArea.x += 1;
            textArea.y += -45;
#else
            textArea.x += 1;
            textArea.y += -3;
#endif

            if (!forTextArea)
            {
                textArea.x = origX;
            }
            
            return textArea;
        }
        
        public static void DrawText(string text, Vector3 pos, Color color, Vector2 guiOffset = default)
        {
            Vector3 guiPoint = HandleUtility.WorldToGUIPointWithDepth(pos);
            //if camera is in front of the point, then don't draw it
            if (guiPoint.z < 0)
            {
                return;
            }
            
            Handles.BeginGUI();
            
            GUIContent content = new GUIContent(text);
            GUIStyle style = new GUIStyle(EditorStyles.whiteMiniLabel) { alignment = TextAnchor.UpperLeft };
            Rect textArea = HandleUtility.WorldPointToSizedRect(pos, content, style);

            textArea.position = GetPositionForWorldPointSizedRect(textArea.position);
            textArea.position += guiOffset;

            Rect backdropArea = new Rect(textArea);
            backdropArea.x += 1;
            backdropArea.y += 4;
            backdropArea.width -= 6;
            backdropArea.height -= 10;
                
            //don't draw the text at all if it manages to be offscreen in the scene view
            SceneView view = SceneView.currentDrawingSceneView;
            if (view != null)
            {
                Vector2 size = new Vector2(view.camera.pixelWidth, view.camera.pixelHeight);
                Rect sceneViewRect = new Rect(Vector2.zero, size);

                if (
                    backdropArea.xMin > sceneViewRect.xMax ||
                    backdropArea.xMax < sceneViewRect.xMin ||
                    backdropArea.yMin > sceneViewRect.yMax ||
                    backdropArea.yMax < sceneViewRect.yMin
                    )
                {
                    Handles.EndGUI();
                    return;
                }
            }

            float a = GetAlphaForDistance();
            if (a <= 0)
            {
                Handles.EndGUI();
                return;
            }
            color.a = a;

            Color backdropColor = color;
            backdropColor.a *= 0.75f;
            
            Color textColor = GetTextColorForSceneText(backdropColor);
            textColor.a *= a;
            style.normal = new GUIStyleState()
            {
                textColor = textColor
            };
            
            EditorGUI.DrawRect(backdropArea, backdropColor);
            GUI.Label(textArea, content, style);
            Handles.EndGUI();
        }

        public static float GetAlphaForDistance()
        {
            SceneView view = SceneView.currentDrawingSceneView;
            if (view == null)
            {
                return 0;
            }
            
            float drawDistance = LDtkPrefs.DrawDistance;
            if (drawDistance >= LDtkPrefs.DISTANCE_MAX)
            {
                return 1;
            }
            
            float transitionGap = 0.5f * drawDistance;
            float alphaForDistanceThreshold = drawDistance - transitionGap;

            return Mathf.InverseLerp(alphaForDistanceThreshold + transitionGap, alphaForDistanceThreshold, view.cameraDistance);
        }


        public static Color GetTextColorForSceneText(Color backdropColor)
        {
            return GetTextColorForBackdrop(backdropColor, 149);
        }

        public static Color GetTextColorForIntGridValueNumber(Color backdropColor)
        {
            return GetTextColorForBackdrop(backdropColor, 75);
        }
        
        private static Color GetTextColorForBackdrop(Color backdropColor, float threshold)
        {
            const float colorValue = 0.1f;

            float red = backdropColor.r * 255;
            float green = backdropColor.g * 255;
            float blue = backdropColor.b * 255;
            float luminosity = red * 0.299f + green * 0.587f + blue * 0.114f;
            
            //credit https://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color
            return luminosity > threshold 
                ? new Color(colorValue, colorValue, colorValue) 
                : Color.white;
        }

        public static void SelectIfNotAlreadySelected(GameObject obj)
        {
            if (Selection.activeGameObject != obj)
            {
                Selection.activeGameObject = obj;
            }
        }


    }
}





 
