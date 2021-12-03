using System;
using UnityEditor;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
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