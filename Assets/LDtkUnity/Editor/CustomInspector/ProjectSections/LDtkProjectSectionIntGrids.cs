using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionIntGrids  : LDtkProjectSectionDrawer<LayerDefinition>
    {
        protected override string PropertyName => LDtkProject.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "The sprites assigned to IntGrid values determine the collision shape of them in the tilemap. Leave any fields empty for no collision.";
        protected override Texture2D GuiImage => LDtkIconLoader.LoadIntGridIcon();

        public LDtkProjectSectionIntGrids(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            //iterator is for figuring out which array index we should really be using, since any layer could have any amount of intgrid values
            int? intGridValueIterator = 0;
            
            foreach (LayerDefinition def in defs)
            {
                //draw intgrid
                LDtkDrawerIntGrid intGridDrawer = new LDtkDrawerIntGrid(def, ArrayProp, intGridValueIterator);
                drawers.Add(intGridDrawer);
            }
        }

        protected override int GetSizeOfArray(LayerDefinition[] datas)
        {
            return datas.SelectMany(p => p.IntGridValues).Distinct().Count();
        }
    }
}