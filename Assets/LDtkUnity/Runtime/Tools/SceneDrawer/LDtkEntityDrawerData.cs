using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [Serializable]
    [ExcludeFromDocs]
    public class LDtkEntityDrawerData : LDtkSceneDrawerEntity
    {
        [SerializeField] private RenderMode _renderMode;
        
        [SerializeField] private Texture _tex;
        [SerializeField] private Rect _rect;

        public LDtkEntityDrawerData(RenderMode renderMode, Texture tex, Rect rect) : base()
        {
            _renderMode = renderMode;
            _tex = tex;
            _rect = rect;
        }

        public void OnDrawGizmos()
        {
            switch (_renderMode)
            {
                case RenderMode.Cross:
                    break;
                case RenderMode.Ellipse:
                    break;
                case RenderMode.Rectangle:
                    break;
                case RenderMode.Tile:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}