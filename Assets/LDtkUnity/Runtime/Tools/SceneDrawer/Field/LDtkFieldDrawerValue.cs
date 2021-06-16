using System;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkFieldDrawerValue : ILDtkGizmoDrawer
    {
        private readonly Vector3 _pos;
        private readonly string _text;

        public LDtkFieldDrawerValue(Vector3 pos, string text)
        {
            _pos = pos;
            _text = text;
        }


        public void OnDrawGizmos()
        {
            GizmoUtil.DrawText(_pos, _text);
        }
    }
}