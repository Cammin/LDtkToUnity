using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL)]
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + "Level Data")]
    [ExcludeFromDocs]
    public class LDtkComponentLevel : MonoBehaviour
    {
        [SerializeField] private Vector2 _size = Vector2.zero;
        [SerializeField] private Color _bgColor = Color.white;

        public Vector2 Size => _size;
        public Color BgColor => _bgColor;

        private void OnDrawGizmos()
        {
            Vector3 halfSize = _size / 2;
            Vector3 pos = transform.position + halfSize;
            
            Vector3 bottomLeft = pos - halfSize;
            Vector3 topRight = pos + halfSize;
            Vector3 topLeft = new Vector2(bottomLeft.x, topRight.y);
            Vector3 bottomRight = new Vector2(topRight.x, bottomLeft.y);

            Vector3[] points = 
            {
                bottomLeft,
                topLeft,
                topRight,
                bottomRight,
                bottomLeft,
            };
            
#if UNITY_EDITOR
            const float width = 3;
            UnityEditor.Handles.color = _bgColor;
            UnityEditor.Handles.DrawAAPolyLine(width, points);
#endif
        }

        public void SetSize(Vector2 size)
        {
            _size = size;
        }

        public void SetBgColor(Color color)
        {
            _bgColor = color;
        }
    }
}
