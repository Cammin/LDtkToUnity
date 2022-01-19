using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal class GLClipScope : IDisposable
    {
        public GLClipScope(Rect rect)
        {
            GUI.BeginClip(rect);
        }
        public void Dispose()
        {
            GUI.EndClip();
        }
    }
}