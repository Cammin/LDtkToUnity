using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkSectionIntGrids : LDtkSectionDataDrawer<LayerDefinition>
    {
        protected override string PropertyName => LDtkProjectImporter.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "Assign Int Grid tiles, which has options for custom collision, rendering colors, and GameObjects. Make some at 'Create > LDtkIntGridTile'";
        protected override Texture GuiImage => LDtkIconUtility.LoadIntGridIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_INTGRID;

        public LDtkSectionIntGrids(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            //iterator is for figuring out which array index we should really be using, since any layer could have any amount of intgrid values
            LDtkDrawerIntGridValueIterator intGridValueIterator = new LDtkDrawerIntGridValueIterator();
            
            foreach (LayerDefinition def in defs)
            {
                LDtkDrawerIntGrid intGridDrawer = new LDtkDrawerIntGrid(def, ArrayProp, intGridValueIterator);
                drawers.Add(intGridDrawer);
            }
        }

        protected override int GetSizeOfArray(LayerDefinition[] datas)
        {
            return datas.SelectMany(p => p.IntGridValues).Count();
        }
    }
}