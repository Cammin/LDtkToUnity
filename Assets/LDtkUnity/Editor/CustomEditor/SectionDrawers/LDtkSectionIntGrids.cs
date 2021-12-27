using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkSectionIntGrids : LDtkSectionDataDrawer<LayerDefinition>
    {
        private readonly GUIContent _buttonContent;
        
        protected override string PropertyName => LDtkProjectImporter.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "Assign Int Grid tiles, which has options for custom collision, rendering colors, and GameObjects. Make some at 'Create > LDtkIntGridTile'";
        protected override Texture GuiImage => LDtkIconUtility.LoadIntGridIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_INTGRID;

        public LDtkSectionIntGrids(SerializedObject serializedObject) : base(serializedObject)
        {
            _buttonContent = new GUIContent()
            {
                text = "Create",
                //image = LDtkIconUtility.LoadIntGridIcon(),
                tooltip = "Creates an IntGrid tile asset next to this inspected project."
            };
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
        
        protected override void DrawDropdownContent()
        {
            CreateAssetButton();
            base.DrawDropdownContent();
        }

        private void CreateAssetButton()
        {
            Rect buttonRect = _headerArea;
            
            const float width = 50;

            const float rOffset = 40;
            buttonRect.xMax -= rOffset;
            buttonRect.x = buttonRect.xMax - width;
            buttonRect.width = width;

            
            if (!GUI.Button(buttonRect, _buttonContent))
            {
                return;
            }
            
            string assetName = $"New {nameof(LDtkIntGridTile)}.asset";
            string path = $"{GetSelectedPathOrFallback()}{assetName}";
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            string uniqueFileName = Path.GetFileNameWithoutExtension(uniquePath);
            
            LDtkIntGridTile blankTile = ScriptableObject.CreateInstance<LDtkIntGridTile>();
            blankTile.name = uniqueFileName;
            
            AssetDatabase.CreateAsset(blankTile, uniquePath);
            EditorGUIUtility.PingObject(blankTile);
        }

        private static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path + "/";
        }
    }
}