using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkEntityIcon : MonoBehaviour
    {
        [SerializeField] private Texture _tex;
        [SerializeField] private Rect _rect;

        public void SetValue(Texture tex, Rect rect)
        {
            _tex = tex;
            _rect = rect;
        }
        
        private void OnDrawGizmos()
        {
            GizmoUtil.DrawGUITextureInWorld(_tex, transform.position, _rect);
        }
    }
}