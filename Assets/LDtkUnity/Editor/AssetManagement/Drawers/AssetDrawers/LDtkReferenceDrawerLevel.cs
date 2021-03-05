using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkReferenceDrawerLevel : LDtkAssetReferenceDrawer<Level>
    {
        public LDtkReferenceDrawerLevel(SerializedProperty obj, string key) : base(obj, key)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, Level data)
        {
            /*DrawLeftIcon(controlRect, LDtkIconLoader.LoadWorldIcon());
            DrawLabel(controlRect, data);
            DrawField<LDtkLevelFile>(controlRect);*/
            
            DrawSelfSimple<LDtkLevelFile>(controlRect, data);
            
            
        }

        public override bool HasError(Level data)
        {
            if (base.HasError(data))
            {
                return true;
            }
            
            LDtkLevelFile file = (LDtkLevelFile) Value.objectReferenceValue;
            if (file == null)
            {
                ThrowWarning("Level not assigned");
                return true;
            }

            if (file.Identifier != data.Identifier)
            {
                ThrowError($"Invalid Level assignment: Assign the level as this field specifies.\n \"{file.Identifier}\" is not \"{data.Identifier}\"");
                return true;
            }

            return false;
        }
    }
}