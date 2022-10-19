using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkEntityDrawerIcon : ILDtkHandleDrawer
    {
        private readonly Transform _transform;
        private readonly Texture _tex;
        private readonly Rect _srcPx;
        
        private bool _canDraw;
        private Vector3 _guiPoint;
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
            _guiPoint = HandleUtility.WorldToGUIPointWithDepth(worldPosition);
            //if camera is in front of the point, then don't draw it
            if (_guiPoint.z < 0)
            {
                return;
            }
            
            Vector2 guiSize = Vector2.one * HandleUtil.GetIconGUISize(worldPosition, _srcPx.size);
            _imageArea = new Rect
            {
                position = (Vector2)_guiPoint - (guiSize/2),
                size = guiSize
            };

            _texCoords = HandleUtil.GetNormalizedTextureCoords(_tex, _srcPx);
            
            float x = _imageArea.x - _guiPoint.x;
            float y = _imageArea.height / 2;
            Vector2 pos = new Vector2(x, y);
            
#if !UNITY_2021_2_OR_NEWER
            pos.y += 2;
#endif
            _imageArea.position = HandleUtil.GetPositionForWorldPointSizedRect(_imageArea.position, false);
            
            OffsetToNextUI = pos;
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
            
            
            Color prev = GUI.color;
            Color color = GUI.color;
            color.a = HandleUtil.GetAlphaForDistance();
            GUI.color = color;
            
            GUI.DrawTextureWithTexCoords(_imageArea, _tex, _texCoords, true);
            
            GUI.color = prev;

            Handles.EndGUI();
        }
    }
}