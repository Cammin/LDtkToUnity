using System;
using UnityEngine;

namespace LDtkUnity
{
    public class EllipseTest : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            GizmoUtil.DrawEllipse(transform.position, transform.localScale);
        }
    }
}