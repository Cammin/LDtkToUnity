using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSectionIntGrids : LDtkSectionDataDrawer<LayerDefinition>
    {
        private readonly GUIContent _buttonContent;
        
        protected override string PropertyName => LDtkProjectImporter.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "Assign a tile to be used in place of an IntGridValue. If you assign Int Grid tiles, you'll be able to separate tilemaps by Tag/Layer/PhysicsMaterial. Make some with the button to the right of this text.";
        protected override Texture GuiImage => LDtkIconUtility.LoadIntGridIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_INTGRID;

        public LDtkSectionIntGrids(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
        {
            _buttonContent = new GUIContent()
            {
                text = "+",
                image = LDtkIconUtility.LoadIntGridIcon(),
                tooltip = "Create a new IntGrid tile asset."
            };
        }

        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            //iterator is for figuring out which array index we should really be using, since any layer could have any amount of intgrid values
            LDtkDrawerIntGridValueIterator intGridValueIterator = new LDtkDrawerIntGridValueIterator();
            
            foreach (LayerDefinition def in defs)
            {
                LDtkGroupDrawerIntGrid intGridDrawer = new LDtkGroupDrawerIntGrid(def, ArrayProp, intGridValueIterator);
                intGridDrawer.InitDrawers();
                drawers.Add(intGridDrawer);
            }
        }

        protected override int GetSizeOfArray(LayerDefinition[] datas)
        {
            return datas.SelectMany(p => p.IntGridValues).Count();
        }

        protected override string[] GetAssetKeysFromDefs(LayerDefinition[] defs)
        {
            return defs.SelectMany(def => def.IntGridValues.Select(value => LDtkKeyFormatUtil.IntGridValueFormat(def, value))).ToArray();
        }

        protected override void DrawDropdownContent()
        {
            
            //Rect buttonRect = GUILayoutUtility.GetLastRect();
            Rect buttonRect = EditorGUILayout.GetControlRect(false, 0);
            buttonRect.y -= 2;
            buttonRect.height = EditorGUIUtility.singleLineHeight;
            
            const float width = 45;
            buttonRect.x = buttonRect.xMax - width;
            buttonRect.width = width;
            
            LDtkAssetCreator.CreateAssetButton(buttonRect, _buttonContent, $"New {nameof(LDtkIntGridTile)}.asset", ScriptableObject.CreateInstance<LDtkIntGridTile>, true);
            base.DrawDropdownContent();
        }
    }
}