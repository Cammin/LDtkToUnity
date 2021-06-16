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
        [SerializeField] private Rect _rect;
        

        public LDtkEntityDrawerData(Transform transform, EntityDefinition def, Texture tex, Rect rect) : base(def.UnityColor)
        {
            _entityMode = def.RenderMode;
            _hollow = def.Hollow;
            _showName = def.ShowName;
            
            _transform = transform;
            _tex = tex;
            _rect = rect;
        }

        protected override ILDtkGizmoDrawer GetDrawer()
        {
            switch (_entityMode)
            {
                case RenderMode.Cross:
                case RenderMode.Ellipse:
                case RenderMode.Rectangle:
                    return new LDtkEntityDrawerShapes(_transform, _entityMode, _transform.localScale);
                
                case RenderMode.Tile:
                    return new LDtkEntityDrawerIcon(_transform, _tex, _rect);
            }

            return null;
        }
    }
}