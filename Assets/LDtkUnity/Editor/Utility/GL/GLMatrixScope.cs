using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class GLMatrixScope : IDisposable
    {
        public GLMatrixScope()
        {
            GL.PushMatrix();
        }
        public void Dispose()
        {
            GL.PopMatrix();
        }
    }
}