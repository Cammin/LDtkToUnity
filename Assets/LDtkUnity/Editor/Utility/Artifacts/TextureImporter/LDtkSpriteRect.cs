using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [Serializable]
    internal sealed class LDtkSpriteRect : SpriteRect
    {
        [SerializeField] private List<Outline> spriteOutline = new List<Outline>();
        public float tessellationDetail;

        [Serializable]
        public class Outline
        {
            public Vector2[] shape = Array.Empty<Vector2>();
        }
        public LDtkSpriteRect()
        {
        }
        public LDtkSpriteRect(SpriteRect sr)
        {
            alignment = sr.alignment;
            border = sr.border;
            name = sr.name;
            pivot = GetPivotValue(sr.alignment, sr.pivot);
            rect = sr.rect;
            spriteID = sr.spriteID;
        }

        public List<Vector2[]> GetOutlines() => spriteOutline.Select(p => p.shape).ToList();
        public void SetOutlines(List<Vector2[]> outlines) => spriteOutline = outlines.Select(p => new Outline(){shape = p}).ToList();
        
        public static Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset)
        {
            switch (alignment)
            {
                case SpriteAlignment.BottomLeft:
                    return new Vector2(0f, 0f);
                case SpriteAlignment.BottomCenter:
                    return new Vector2(0.5f, 0f);
                case SpriteAlignment.BottomRight:
                    return new Vector2(1f, 0f);

                case SpriteAlignment.LeftCenter:
                    return new Vector2(0f, 0.5f);
                case SpriteAlignment.Center:
                    return new Vector2(0.5f, 0.5f);
                case SpriteAlignment.RightCenter:
                    return new Vector2(1f, 0.5f);

                case SpriteAlignment.TopLeft:
                    return new Vector2(0f, 1f);
                case SpriteAlignment.TopCenter:
                    return new Vector2(0.5f, 1f);
                case SpriteAlignment.TopRight:
                    return new Vector2(1f, 1f);

                case SpriteAlignment.Custom:
                    return customOffset;
            }
            return Vector2.zero;
        }
    }
}