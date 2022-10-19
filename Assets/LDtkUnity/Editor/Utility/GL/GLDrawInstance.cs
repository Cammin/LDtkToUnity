using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal sealed class GLDrawInstance : IDisposable
    {
        private readonly Material _mat;

        public GLDrawInstance()
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _mat = new Material(shader);

        }
        
        public void Dispose()
        {
            Object.DestroyImmediate(_mat);
        }
        

        public void DrawInInspector(Rect rect, Action glDrawAction)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            using (new GLClipScope(rect))
            {
                using (new GLMatrixScope())
                {
                    _mat.SetPass(0);
                    glDrawAction.Invoke();
                }
            }
        }
    }
}