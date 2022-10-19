using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkLevelDrawer : ILDtkHandleDrawer
    {
        private readonly Vector3 _position;
        private readonly Vector2 _size;
        private readonly string _identifier;
        private readonly Color _bgColor;
        private readonly Color _smartColor;

        public LDtkLevelDrawer(LDtkComponentLevel level)
        {
            _bgColor = level.BgColor;
            _smartColor = level.SmartColor;
            _position = level.BorderBounds.min;
            _size = level.BorderBounds.size;
            _identifier = level.Identifier;
        }

        public void OnDrawHandles()
        {
            //borders, then labels, so that borders are never in front of labels

            if (LDtkPrefs.ShowLevelBorder)
            {
                Handles.color = _smartColor;
                Vector3 halfSize = _size / 2;
                Vector3 pos = _position + halfSize;
                HandleAAUtil.DrawAABox(pos, _size, LDtkPrefs.LevelBorderThickness, 0);
            }
            
            if (LDtkPrefs.ShowLevelIdentifier)
            {
                HandleUtil.DrawText(_identifier, _position, _bgColor);
            }
        }
    }
}