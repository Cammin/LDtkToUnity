﻿using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerShapes : ILDtkHandleDrawer
    {
        private readonly Transform _transform;
        private readonly Data _data;

        public struct Data
        {
            public RenderMode EntityMode;
            public Vector2 Size;
            public Vector2 Pivot;
            public bool Hollow;
            public float FillOpacity;
            public float LineOpacity;
        } 

        public LDtkEntityDrawerShapes(Transform transform, Data data)
        {
            _data = data;
            _transform = transform;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowEntityShape)
            {
                return;
            }
            
            float lineAlpha = _data.LineOpacity;
            float fillAlpha = _data.Hollow ? 0 : _data.FillOpacity;
            
            Vector2 size = _data.Size;
            Vector2 halfUnit = Vector2.one * 0.5f;
            Vector2 properPivot = _data.Pivot - halfUnit;
            Vector2 pivotSize = size * properPivot;
            Vector2 offset = Vector2.right * pivotSize.x * -2;
            Vector2 totalOffset = pivotSize + offset;
            Vector2 pos = (Vector2)_transform.position + totalOffset;

            switch (_data.EntityMode)
            {
                case RenderMode.Cross:
                    HandleAAUtil.DrawAACross(pos, size, 4); //todo fix this to support any pivot point
                    break;
                
                case RenderMode.Ellipse:
                    HandleAAUtil.DrawAAEllipse(pos, size, fillAlpha: fillAlpha, lineAlpha: lineAlpha); //todo fix this to support any pivot point
                    break;
                
                case RenderMode.Rectangle:
                    HandleAAUtil.DrawAABox(pos, size, fillAlpha: fillAlpha, lineAlpha: lineAlpha); //todo fix this to support any pivot point
                    break;
            }
        }
    }
}