using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class GLDrawScope : IDisposable
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