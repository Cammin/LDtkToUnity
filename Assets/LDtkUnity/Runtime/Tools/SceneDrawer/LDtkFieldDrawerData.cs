using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    internal class LDtkFieldDrawerData : LDtkSceneDrawerBase
    {
        [SerializeField] private LDtkFields _fields;
        [SerializeField] private EditorDisplayMode _fieldMode;
        [SerializeField] private int _gridSize;
        [SerializeField] private int _pixelsPerUnit;
        [SerializeField] private Vector3 _middleCenter;
        
        public LDtkFields Fields => _fields;
        public EditorDisplayMode FieldMode => _fieldMode;
        public int GridSize => _gridSize;
        public int PixelsPerUnit => _pixelsPerUnit;
        public Vector3 LocalMiddleCenter => (Vector2)_fields.transform.position + _middleCenter * (Vector2)_fields.transform.lossyScale;

        public LDtkFieldDrawerData(LDtkFields fields, Color smartColor, EditorDisplayMode fieldMode, string identifier, int gridSize, int pixelsPerUnit, Vector3 middleCenterWorldPos) : base(identifier, smartColor)
        {
            _fields = fields;
            _fieldMode = fieldMode;
            _gridSize = gridSize;
            _pixelsPerUnit = pixelsPerUnit;
            _middleCenter = _fields.transform.InverseTransformPoint(middleCenterWorldPos);
        }
    }
}