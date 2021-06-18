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
#if UNITY_EDITOR
            UnityEditor.Handles.color = _bgColor;
#endif
            GizmoUtil.DrawText(transform.position, name);
            GizmoAAUtil.DrawAABox(pos, _size, fillAlpha: 0);
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
