using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkGUIEnabledScope : IDisposable
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