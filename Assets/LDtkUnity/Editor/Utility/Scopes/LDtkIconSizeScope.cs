using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkIconSizeScope : IDisposable
    {
        private readonly Vector2 _prevSize;
        
        public LDtkIconSizeScope(float iconSize)
        {
            _prevSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize(Vector2.one * iconSize);
        }
        
        public void Dispose()
        {
            EditorGUIUtility.SetIconSize(_prevSize);
        }
    }
}