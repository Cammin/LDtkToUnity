using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LDtkUnity
{
    [Serializable]
    internal abstract class LDtkSceneDrawerBase
    {
        [SerializeField] private string _identifier;
        [SerializeField] private bool _enabled = true;
        [FormerlySerializedAs("_gizmoColor")] [SerializeField] private Color _smartColor;

        public string Identifier => _identifier;
        public bool Enabled => _enabled;
        public Color SmartColor => _smartColor;

        protected LDtkSceneDrawerBase(string identifier, Color smartColor)
        {
            _identifier = identifier;
            _smartColor = smartColor;
            AdjustGizmoColor();
        }

        private void AdjustGizmoColor()
        {
            Color c = _smartColor;
            c.a = 0.66f;
            const float incrementDifference = -0.1f;
            c.r += incrementDifference;
            c.g += incrementDifference;
            c.b += incrementDifference;
            _smartColor = c;
        }
        
    }
}