using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.AutoLayers_2_stamps
{
    public class ExampleLabel : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField("Text")] public string _label;
        [LDtkField("Color")] public Color _color;

        public void UpdateValues()
        {
            _textMesh.text = _label;
            _textMesh.color = _color;
        }
    }
}
