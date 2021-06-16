using System;
using UnityEngine;

namespace LDtkUnity
{
    public class EllipseTest : MonoBehaviour
    {
        [SerializeField, Range(0,10)] private float thickness;
        [SerializeField, Range(0,1)] private float alpha;
        [SerializeField, Range(0,100)] private int pointCount;
        
        private void OnDrawGizmos()
        {
            GizmoUtil.DrawAAEllipse(transform.position, transform.localScale, pointCount, thickness, alpha);
            //GizmoUtil.DrawEllipse(transform.position, transform.localScale);
        }
    }
}