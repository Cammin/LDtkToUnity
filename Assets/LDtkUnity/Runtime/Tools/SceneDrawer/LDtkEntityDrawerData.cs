using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [Serializable]
    [ExcludeFromDocs]
    public class LDtkEntityDrawerData : LDtkSceneDrawerBase
    {
        [SerializeField] protected RenderMode _entityMode;
        [SerializeField] protected bool _drawTile;
        [SerializeField] private bool _hollow;
        [SerializeField] private bool _showName;
        
        [SerializeField] protected Transform _transform;
        [SerializeField] private Texture _tex;
        [SerializeField] private Rect _texRect;
        [SerializeField] private Vector2 _pivot;
        [SerializeField] private float _fillOpacity;
        [SerializeField] private float _lineOpacity;
        
        [SerializeField] private Vector2 _size;

        public RenderMode EntityMode => _entityMode;
        public bool DrawTile => _drawTile;
        public bool Hollow => _hollow;
        public bool ShowName => _showName;
        public Transform Transform => _transform;
        public Texture Tex => _tex;
        public Rect TexRect => _texRect;
        public Vector2 Pivot => _pivot;
        public float FillOpacity => _fillOpacity;
        public float LineOpacity => _lineOpacity;
        public Vector2 Size => _size;
        
        public LDtkEntityDrawerData(Transform transform, EntityDefinition def, Texture tex, Rect texRect, Vector2 size, Color gizmoColor) : base(def.Identifier, gizmoColor)
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
            _size = size;

            _drawTile = def.RenderMode == RenderMode.Tile || def.FieldDefs.Any(field => field.EditorDisplayMode == EditorDisplayMode.EntityTile);
        }
    }
}