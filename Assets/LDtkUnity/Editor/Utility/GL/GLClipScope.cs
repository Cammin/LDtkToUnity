using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class GLClipScope : IDisposable
    {
        public GLClipScope(Rect rect)
        {
            GUI.BeginClip(rect);
            GL.PushMatrix();
                
            GL.Clear(true, false, Color.black);
                
        }
        public void Dispose()
        {
            GL.PopMatrix();
            GUI.EndClip();
        }
    }
}