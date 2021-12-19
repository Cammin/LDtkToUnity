using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkGUIScope : IDisposable
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