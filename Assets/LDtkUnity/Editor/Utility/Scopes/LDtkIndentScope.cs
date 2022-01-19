using System;
using UnityEditor;

namespace LDtkUnity.Editor
{
    internal class LDtkIndentScope : IDisposable
    {
        public LDtkIndentScope()
        {
            EditorGUI.indentLevel++;
        }
        
        public void Dispose()
        {
            EditorGUI.indentLevel--;
        }
    }
}