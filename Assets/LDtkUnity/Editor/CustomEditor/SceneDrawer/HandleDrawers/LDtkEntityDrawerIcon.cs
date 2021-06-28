using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerIcon : ILDtkHandleDrawer
    {
        private readonly Transform _transform;
        private readonly Texture _tex;
        private readonly Rect _srcPx;
        
        private bool _canDraw;
        private Rect _texCoords;
        private Rect _imageArea;

        public Vector2 OffsetToNextUI { get; private set; } = Vector2.zero;
        
        public LDtkEntityDrawerIcon(Transform transform, Texture tex, Rect srcPx)
        {
            _transform = transform;
            _tex = tex;
            _srcPx = srcPx;
        }

        public void PrecalculateValues()
        {
            if (_tex == null)
            {
                return;
            }

            if (_transform == null)
            {
                return;
            }

            if (!LDtkPrefs.ShowEntityIcon)
            {
                return;
            }
            
            Handles.BeginGUI();
            Vector3 worldPosition = _transform.position;
            Vector3 guiPoint = HandleUtility.WorldToGUIPointWithDepth(worldPosition);
            //if camera is in front of the point, then don't draw it
            if (guiPoint.z < 0)
            {
                return;
            }
            
            Vector2 guiSize = Vector2.one * HandleUtil.GetIconGUISize(worldPosition, _srcPx.size);
            _imageArea = new Rect
            {
                position = (Vector2)guiPoint - (guiSize/2),
                size = guiSize
            };

            _texCoords = HandleUtil.GetNormalizedTextureCoords(_tex, _srcPx);
            
            OffsetToNextUI = new Vector2()
            {
                x = _imageArea.x - guiPoint.x,
                y = _imageArea.height / 2 + 2
            };

            _canDraw = true;
            
            Handles.EndGUI();
        }

        public void OnDrawHandles()
        {
            if (!_canDraw)
            {
                return;
            }
            
            Handles.BeginGUI();
            
            if (GUI.Button(_imageArea, GUIContent.none, GUIStyle.none))
            {
                Selection.activeGameObject = _transform.gameObject;;
            }
            
            GUI.DrawTextureWithTexCoords(_imageArea, _tex, _texCoords);

            Handles.EndGUI();
        }
    }
}