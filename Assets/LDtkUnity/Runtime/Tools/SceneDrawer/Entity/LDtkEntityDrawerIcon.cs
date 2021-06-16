using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerIcon : ILDtkGizmoDrawer
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

        public void OnDrawGizmos()
        {
            if (_tex == null)
            {
                return;
            }
            
            GizmoUtil.DrawGUITextureInWorld(_tex, _transform.position, _rect);
        }
    }
}