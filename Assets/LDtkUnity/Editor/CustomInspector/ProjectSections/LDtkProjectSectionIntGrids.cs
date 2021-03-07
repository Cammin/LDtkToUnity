using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionIntGrids  : LDtkProjectSectionDrawer<LayerDefinition>
    {
        protected override string PropertyName => LDtkProject.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "The sprites assigned to IntGrid Values determine the collision shape of them in the tilemap.";
        protected override Texture2D GuiImage => LDtkIconLoader.LoadIntGridIcon();
        
        public LDtkProjectSectionIntGrids(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override int GetSizeOfArray(LayerDefinition[] datas)
        {
            return datas.SelectMany(p => p.IntGridValues).Distinct().Count();
        }

        protected override void DrawDropdownContent(LayerDefinition[] datas)
        {
            HasProblem = !DrawIntGridLayers(datas);
        }

        private bool DrawIntGridLayers(LayerDefinition[] intGridLayerDefs)
        {
            int intGridValueIterator = 0;
            bool passed = true;
            foreach (LayerDefinition intGridLayerDef in intGridLayerDefs)
            {
                //draw intgrid
                new LDtkDrawerIntGrid().Draw(intGridLayerDef);

                //THEN the int grid values
                foreach (IntGridValueDefinition intGridValueDef in intGridLayerDef.IntGridValues)
                {
                    intGridValueIterator = DrawIntGridValue(intGridValueIterator, intGridLayerDef, intGridValueDef, ref passed);
                }
            }

            return passed;
        }

        private int DrawIntGridValue(int intGridValueIterator, LayerDefinition intGridLayerDef,
            IntGridValueDefinition intGridValueDef, ref bool passed)
        {
            SerializedProperty valueObj = ArrayProp.GetArrayElementAtIndex(intGridValueIterator);
            intGridValueIterator++;


            string key = LDtkIntGridKeyFormat.GetKeyFormat(intGridLayerDef, intGridValueDef);

            LDtkDrawerIntGridValue drawer =
                new LDtkDrawerIntGridValue(valueObj, key,
                    (float) intGridLayerDef.DisplayOpacity);

            if (drawer.HasError(intGridValueDef))
            {
                passed = false;
            }

            drawer.Draw(intGridValueDef);
            return intGridValueIterator;
        }
    }
}