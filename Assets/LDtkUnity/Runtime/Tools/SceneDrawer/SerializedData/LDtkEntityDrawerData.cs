using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [Serializable]
    [ExcludeFromDocs]
    public class LDtkEntityDrawerData : LDtkSceneDrawerBase
    {
        [SerializeField] protected RenderMode _entityMode;
        [SerializeField] private bool _hollow;
        [SerializeField] private bool _showName;
        
        [SerializeField] protected Transform _transform;
        [SerializeField] private Texture _tex;
        [SerializeField] private Rect _texRect;
        [SerializeField] private Vector2 _pivot;
        [SerializeField] private float _fillOpacity;
        [SerializeField] private float _lineOpacity;
        
        [SerializeField] private Vector2 _size;




        public LDtkEntityDrawerData(Transform transform, EntityDefinition def, Texture tex, Rect texRect, Vector2 gridSize) : base(def.UnityColor)
        {
            _entityMode = def.RenderMode;
            _hollow = def.Hollow;
            _showName = def.ShowName;
            _pivot = def.UnityPivot;
            _fillOpacity = (float)def.FillOpacity;
            _lineOpacity = (float)def.LineOpacity;
            
            _transform = transform;
            _tex = tex;
            _texRect = texRect;
            _size = gridSize;
        }

        protected override ILDtkGizmoDrawer GetDrawer()
        {
            switch (_entityMode)
            {
                case RenderMode.Cross:
                case RenderMode.Ellipse:
                case RenderMode.Rectangle:
                    LDtkEntityDrawerShapes.Data data = new LDtkEntityDrawerShapes.Data()
                    {
                        EntityMode = _entityMode,
                        FillOpacity = _fillOpacity,
                        LineOpacity = _lineOpacity,
                        Hollow = _hollow,
                        Pivot = _pivot,
                        Size = _size
                    };
                    return new LDtkEntityDrawerShapes(_transform, data);
                
                case RenderMode.Tile:
                    return new LDtkEntityDrawerIcon(_transform, _tex, _texRect);
            }

            return null;
        }
    }
}