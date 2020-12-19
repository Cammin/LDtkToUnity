using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleTutorial : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField("desc")] public string _desc;
        [LDtkField("color")] public Color _color;

        public void UpdateValues()
        {
            _textMesh.text = _desc;
            _textMesh.color = _color;
        }
    }
}
