using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    public abstract class LDtkJsonFileEditor<T> : UnityEditor.Editor
    {
        private T _cachedData = default;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            LDtkJsonFile<T> file = (LDtkJsonFile<T>) target;
            Assert.IsNotNull(file);
            
            _cachedData ??= file.FromJson;
            Assert.AreNotEqual(_cachedData, default);
            
            DrawInspectorGUI(_cachedData);
            
        }

        protected abstract void DrawInspectorGUI(T data);
    }
}