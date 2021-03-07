using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionTilesets  : LDtkProjectSectionDrawer<TilesetDefinition>
    {
        protected override string PropertyName => LDtkProject.TILESETS;
        protected override string GuiText => "Tilesets";
        protected override string GuiTooltip => "Auto-assign the tilesets, and then hit the circle button to automatically generate spiced sprites, and also create tile assets out of them.";
        protected override Texture2D GuiImage => LDtkIconLoader.LoadTilesetIcon();
        
        public LDtkProjectSectionTilesets(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void DrawDropdownContent(TilesetDefinition[] datas)
        {
            HasProblem = !DrawTilesets(datas, ArrayProp);

            AutoAssetLinkerTilesets tilesetLinker = new AutoAssetLinkerTilesets();
            tilesetLinker.DrawButton(ArrayProp, datas, Project.ProjectJson);
        }

        private bool DrawTilesets(TilesetDefinition[] definitions, SerializedProperty tilesetArrayProp)
        {
            bool passed = true;
            for (int i = 0; i < definitions.Length; i++)
            {
                TilesetDefinition definition = definitions[i];
                
                SerializedProperty tilesetObj = tilesetArrayProp.GetArrayElementAtIndex(i);

                LDtkDrawerTileset drawer = new LDtkDrawerTileset(tilesetObj, definition.Identifier);
                
                if (drawer.HasError(definition))
                {
                    passed = false;
                }
                
                drawer.Draw(definition);
            }
            return passed;
        }
    }
}