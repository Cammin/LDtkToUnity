using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class GLClipScope : IDisposable
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