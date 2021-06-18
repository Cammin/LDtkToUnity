using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkFieldDrawerData : LDtkSceneDrawerBase
    {
        [SerializeField] private string _identifier;
        [SerializeField] private LDtkFields _fields;
        [SerializeField] private EditorDisplayMode _fieldMode;
        [SerializeField] private  int _gridSize;
        
        [SerializeField] private Texture _iconTex;
        [SerializeField] private Rect _iconRect;

        public string Identifier => _identifier;
        public LDtkFields Fields => _fields;
        public EditorDisplayMode FieldMode => _fieldMode;
        public int GridSize => _gridSize;
        public Texture IconTex => _iconTex;
        public Rect IconRect => _iconRect;

        public LDtkFieldDrawerData(LDtkFields fields, Color color, EditorDisplayMode fieldMode, string identifier, int gridSize, Texture iconTex, Rect iconRect) : base(color)
        {
            _identifier = identifier;
            _fields = fields;
            _fieldMode = fieldMode;
            _gridSize = gridSize;
            _iconTex = iconTex;
            _iconRect = iconRect;
        }

        
    }
}