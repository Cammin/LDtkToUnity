using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerLevel : LDtkAssetDrawer<Level, LDtkLevelFile>
    {
        public LDtkDrawerLevel(SerializedProperty obj, string key) : base(obj, key)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, Level data)
        {
            /*DrawLeftIcon(controlRect, LDtkIconLoader.LoadWorldIcon());
            DrawLabel(controlRect, data);
            DrawField<LDtkLevelFile>(controlRect);*/
            
            //DrawSelfSimple(controlRect, data);
            
            DrawField(controlRect, data);
        }

        public override bool HasError(Level data)
        {
            if (base.HasError(data))
            {
                return true;
            }

            if (Asset.Identifier != data.Identifier)
            {
                CacheError($"Invalid Level assignment: Assign the level as this field requires.\n \"{Asset.Identifier}\" is not \"{data.Identifier}\"");
                return true;
            }

            return false;
        }
    }
}