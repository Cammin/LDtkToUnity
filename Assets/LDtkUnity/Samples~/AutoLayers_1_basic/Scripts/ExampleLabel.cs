using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.AutoLayers_1_basic
{
    public class ExampleLabel : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField("Label")] public string _label;
        [LDtkField("Color")] public Color _color;

        public void UpdateValues()
        {
            _textMesh.text = _label;
            _textMesh.color = _color;
        }
    }
}
