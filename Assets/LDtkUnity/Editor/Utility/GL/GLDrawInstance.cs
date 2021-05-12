using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    public class GLDrawInstance : IDisposable
    {
        private readonly Material _mat;

        private GLDrawInstance()
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _mat = new Material(shader);

        }
        
        public void Dispose()
        {
            Object.DestroyImmediate(_mat);
        }
        

        public void DrawInInspector(Rect rect, Action<Rect> glDrawAction)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            using (new GLClipScope(rect))
            {
                _mat.SetPass(0);
                glDrawAction.Invoke(rect);
            }
        }
    }
}