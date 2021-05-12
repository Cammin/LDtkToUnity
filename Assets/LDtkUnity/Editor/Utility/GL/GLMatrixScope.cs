using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class GLMatrixScope : IDisposable
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