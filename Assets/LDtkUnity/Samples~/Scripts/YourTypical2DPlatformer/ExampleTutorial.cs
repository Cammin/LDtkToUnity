using LDtkUnity.Runtime.EntityCallbacks;
using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace Samples.Scripts.YourTypical2DPlatformer
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