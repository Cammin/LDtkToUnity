using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkLevelDrawer : ILDtkHandleDrawer
    {
        private readonly Vector3 _position;
        private readonly Vector2 _size;
        private readonly string _identifier;

        public LDtkLevelDrawer(Vector3 position, Vector2 size, string identifier)
        {
            _position = position;
            _size = size;
            _identifier = identifier;
        }

        public void OnDrawHandles()
        {
            if (LDtkPrefs.ShowLevelIdentifier)
            {
                HandleUtil.DrawText(_position, _identifier);
            }

            if (LDtkPrefs.ShowLevelBorder)
            {
                Vector3 halfSize = _size / 2;
                Vector3 pos = _position + halfSize;
                HandleAAUtil.DrawAABox(pos, _size, LDtkPrefs.ShowLevelBorderThickness, 0);
            }
        }
    }
}