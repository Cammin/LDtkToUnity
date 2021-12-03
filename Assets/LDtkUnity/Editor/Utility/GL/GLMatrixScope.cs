using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
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