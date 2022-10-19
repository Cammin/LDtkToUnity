using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //use this instead of EditorGUI.DisabledScope because it doesn't work in some situations
    internal sealed class LDtkGUIEnabledScope : IDisposable
    {
        private readonly bool _prevEnabled;
        public LDtkGUIEnabledScope(bool enabled)
        {
            _prevEnabled = GUI.enabled;
            GUI.enabled = enabled;
        }
            
        public void Dispose()
        {
            GUI.enabled = _prevEnabled;
        }
    }
}