using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal class LDtkIconSizeScope : IDisposable
    {
        private readonly Vector2 _prevSize;
        
        public LDtkIconSizeScope(Vector2 iconSize)
        {
            _prevSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize(iconSize);
        }
        
        public void Dispose()
        {
            EditorGUIUtility.SetIconSize(_prevSize);
        }
    }
}