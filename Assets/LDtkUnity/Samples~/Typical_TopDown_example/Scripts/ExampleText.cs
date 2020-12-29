using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Typical_TopDown_example
{
    public class ExampleText : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField] public string text;
        [LDtkField] public Color color;

        public void UpdateValues()
        {
            _textMesh.text = text;
            _textMesh.color = color;
        }
    }
}