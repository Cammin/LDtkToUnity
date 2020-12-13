using LDtkUnity.EntityEvents;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples
{
    public class ExampleTutorial : MonoBehaviour, ILDtkFieldInjectedEvent
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField] public string text;
        [LDtkField] public Color color;

        public void OnLDtkFieldsInjected()
        {
            _textMesh.text = text;
            _textMesh.color = color;
        }
    }
}