using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerLevel : LDtkAssetReferenceDrawer<Level>
    {
        public LDtkReferenceDrawerLevel(SerializedObject obj, string key) : base(obj, key)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, Level data)
        {
            DrawLeftIcon(controlRect, LDtkIconLoader.LoadWorldIcon());
            DrawLabel(controlRect, data);
            DrawField(controlRect);

            LDtkLevelFile file = (LDtkLevelFile) Value.objectReferenceValue;
            if (file == null)
            {
                ThrowWarning(controlRect, "Level not assigned");
                return;
            }

            if (file.Identifier != data.Identifier)
            {
                ThrowError(controlRect, $"Invalid Level assignment: Assign the level as this field specifies.\n \"{file.Identifier}\" is not \"{data.Identifier}\"");
            }
        }


    }
}