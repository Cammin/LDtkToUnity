using LDtkUnity.EntityEvents;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples
{
    public class ExampleTutorial : MonoBehaviour
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