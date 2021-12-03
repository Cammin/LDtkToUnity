using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class GLDrawScope : IDisposable
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