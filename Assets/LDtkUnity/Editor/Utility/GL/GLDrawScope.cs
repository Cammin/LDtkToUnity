using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class GLDrawScope : IDisposable
    {
        public GLDrawScope(int mode)
        {
            GL.Begin(mode);
        }
        public void Dispose()
        {
            GL.End();
        }
    }
}