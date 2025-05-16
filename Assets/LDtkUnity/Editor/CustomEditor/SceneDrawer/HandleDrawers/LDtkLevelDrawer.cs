using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Drawing gizmos related to the level. We only draw an identifier label, and a border. No fields!
    /// </summary>
    internal sealed class LDtkLevelDrawer
    {
        private LDtkComponentLevel _level;
        
        public void DrawHandles(LDtkComponentLevel level)
        {
            _level = level;
            //borders, then labels, so that borders are never in front of labels

            var borderRect = new Rect(_level.transform.position, _level.Size);
            
            if (LDtkPrefs.ShowLevelBorder)
            {
                Handles.color = _level.SmartColor;
                HandleAAUtil.DrawAABox(borderRect.center, _level.Size, LDtkPrefs.LevelBorderThickness, 0);
            }
            
            if (LDtkPrefs.ShowLevelIdentifier)
            {
                HandleUtil.DrawText(_level.Identifier, borderRect.min, _level.BgColor);
            }
        }
    }
}