using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldDrawerValue : ILDtkHandleDrawer
    {
        private readonly Vector3 _pos;
        private readonly string _text;

        public LDtkFieldDrawerValue(Vector3 pos, string text)
        {
            _pos = pos;
            _text = text;
        }
        
        public void OnDrawHandles()
        {
            /*if (LDtkPrefs.ShowFieldIdentifier)
            {
                HandleUtil.DrawText(_text, _pos);
            }*/
        }
    }
}