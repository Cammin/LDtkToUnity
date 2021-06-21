using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkFieldDrawerData : LDtkSceneDrawerBase
    {
        [SerializeField] private LDtkFields _fields;
        [SerializeField] private EditorDisplayMode _fieldMode;
        [SerializeField] private  int _gridSize;
        
        //[SerializeField] private Texture _iconTex;
        //[SerializeField] private Rect _iconRect;
        [SerializeField] private Vector3 _middleCenter;
        
        public LDtkFields Fields => _fields;
        public EditorDisplayMode FieldMode => _fieldMode;
        public int GridSize => _gridSize;
        //public Texture IconTex => _iconTex;
        //public Rect IconRect => _iconRect;
        public Vector3 MiddleCenter => _middleCenter;

        public LDtkFieldDrawerData(LDtkFields fields, Color color, EditorDisplayMode fieldMode, string identifier, int gridSize, Vector3 middleCenter) : base(identifier, color)
        {
            _fields = fields;
            _fieldMode = fieldMode;
            _gridSize = gridSize;
            //_iconTex = iconTex;
            //_iconRect = iconRect;
            _middleCenter = middleCenter;
        }
    }
}