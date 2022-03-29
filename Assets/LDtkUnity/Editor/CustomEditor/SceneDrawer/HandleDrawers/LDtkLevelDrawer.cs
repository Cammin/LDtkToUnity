using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkLevelDrawer : ILDtkHandleDrawer
    {
        private readonly GameObject _obj;
        private readonly Vector3 _position;
        private readonly Vector2 _size;
        private readonly string _identifier;
        private readonly Color _bgColor;
        private readonly Color _smartColor;

        public LDtkLevelDrawer(LDtkComponentLevel level)
        {
            _bgColor = level.BgColor;
            _smartColor = level.SmartColor;
            _position = level.transform.position;
            _obj = level.gameObject;
            _size = level.Size;
            _identifier = level.Identifier;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowLevelBorder)
            {
                return;
            }

            DrawBorder();
        }

        private void DrawBorder()
        {
            Handles.color = _smartColor;
            Vector3 halfSize = _size / 2;
            Vector3 pos = _position + halfSize;
            HandleAAUtil.DrawAABox(pos, _size, LDtkPrefs.LevelBorderThickness, 0);
        }

        public void DrawLabel()
        {
            if (LDtkPrefs.ShowLevelIdentifier)
            {
                HandleUtil.DrawText(_identifier, _position, _bgColor, default, () => HandleUtil.SelectIfNotAlreadySelected(_obj)); 
            }
        }
    }
}