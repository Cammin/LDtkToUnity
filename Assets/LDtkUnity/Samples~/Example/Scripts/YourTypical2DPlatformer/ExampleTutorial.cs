using System;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.FieldInjection;
using TMPro;
using UnityEngine;

namespace LDtkUnity.Samples.Example.Scripts.YourTypical2DPlatformer
{
    public class ExampleTutorial : MonoBehaviour, ILDtkFieldInjectedEvent
    {
        [SerializeField] private TextMeshPro _textMesh;
        
        [LDtkField] public string text;
        [LDtkField] public Color color;

        public void OnLDtkFieldsInjected()
        {
            _textMesh.text = text;
            _textMesh.color = color;
        }
    }
}