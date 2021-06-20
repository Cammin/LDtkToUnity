using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkLevelDrawer : ILDtkHandleDrawer
    {
        private readonly GameObject _obj;
        private readonly Vector3 _position;
        private readonly Vector2 _size;
        private readonly string _identifier;

        public LDtkLevelDrawer(LDtkComponentLevel level)
        {
            _position = level.transform.position;
            _obj = level.gameObject;
            _size = level.Size;
            _identifier = level.Identifier;
        }

        public void OnDrawHandles()
        {
            if (LDtkPrefs.ShowLevelIdentifier)
            {
                HandleUtil.DrawText(_identifier, _position, default, () => HandleUtil.SelectIfNotAlreadySelected(_obj));
            }

            if (LDtkPrefs.ShowLevelBorder)
            {
                Vector3 halfSize = _size / 2;
                Vector3 pos = _position + halfSize;
                HandleAAUtil.DrawAABox(pos, _size, LDtkPrefs.LevelBorderThickness, 0);
            }
        }
    }
}