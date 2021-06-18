using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerIcon : ILDtkHandleDrawer
    {
        private readonly Transform _transform;
        private readonly Texture _tex;
        private readonly Rect _rect;

        public LDtkEntityDrawerIcon(Transform transform, Texture tex, Rect rect)
        {
            _transform = transform;
            _tex = tex;
            _rect = rect;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowEntityIcon)
            {
                return;
            }
            
            if (_tex == null)
            {
                return;
            }

            if (_transform == null)
            {
                return;
            }
            
            HandleUtil.DrawGUITextureInWorld(_tex, _transform.position, _rect, _transform.gameObject);
        }
    }
}