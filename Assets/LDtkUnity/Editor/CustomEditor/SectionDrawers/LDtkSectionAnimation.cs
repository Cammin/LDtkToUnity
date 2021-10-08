using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkSectionAnimation : LDtkSectionDataDrawer<LayerDefinition>
    {
        protected override string PropertyName => LDtkProjectImporter.ANIM_TILES;
        protected override string GuiText => "Tile Animation";
        protected override string GuiTooltip => "";
        protected override Texture GuiImage => LDtkIconUtility.GetUnityIcon("AnimationClip");
        protected override string ReferenceLink => LDtkHelpURL.SECTION_ANIM_TILES;

        public LDtkSectionAnimation(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                LayerDefinition def = defs[i];
                SerializedProperty entityObj = ArrayProp.GetArrayElementAtIndex(i);

                float key = LDtkKeyFormatUtil.TilesetKeyFormat(_data, intGridValueDef);
                
                LDtkDrawerAnimatedTile drawer = new LDtkDrawerAnimatedTile(def, entityObj, def.Identifier); //todo we need to set up a proper identifier 
                
                drawers.Add(drawer);
            }
        }
        
        protected override int GetSizeOfArray(LayerDefinition[] datas)
        {
            foreach (LayerDefinition layerDefinition in datas)
            {
                layerDefinition.
            }
            
            return datas.SelectMany(p => p.).Count();
        }
    }
}