using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal class LDtkGUIScope : IDisposable
    {
        private readonly bool _prevEnabled;
        public LDtkGUIScope(bool enabled)
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