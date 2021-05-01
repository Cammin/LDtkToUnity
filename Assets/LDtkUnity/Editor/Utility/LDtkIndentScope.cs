using System;
using UnityEditor;

namespace LDtkUnity.Editor
{
    public class LDtkIndentScope : IDisposable
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