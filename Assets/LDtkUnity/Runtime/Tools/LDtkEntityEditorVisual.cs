using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkEntityEditorVisual : MonoBehaviour
    {
        
        //todo delete this soon
        [SerializeField] private RenderMode _renderMode;
        [SerializeField] private Texture _tex;
        [SerializeField] private Rect _rect;

        /*public void SetValue(Texture tex, Rect rect)
        {
            _tex = tex;
            _rect = rect;
        }
        
        private void OnDrawGizmos()
        {
            GizmoUtil.DrawGUITextureInWorld(_tex, transform.position, _rect);
        }*/
    }
}