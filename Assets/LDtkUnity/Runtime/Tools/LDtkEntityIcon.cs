using System;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkEntityIcon : MonoBehaviour
    {
        public Texture tex;
        public Rect rect;

        private void OnDrawGizmos()
        {
            GizmoUtil.DrawGUITextureInWorld(tex, transform.position, rect);
        }
    }
}