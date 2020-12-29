using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.AutoLayers_3_Mosaic
{
    public class ExampleTutorial : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField("desc")] public string _label;
        [LDtkField("color")] public Color _color;

        public void UpdateValues()
        {
            _textMesh.text = _label;
            _textMesh.color = _color;
        }
    }
}
