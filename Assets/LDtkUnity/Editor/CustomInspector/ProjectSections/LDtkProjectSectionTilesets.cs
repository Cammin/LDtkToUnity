using System.Collections.Generic;
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

        protected override void GetDrawers(TilesetDefinition[] defs, List<LDtkContentDrawer<TilesetDefinition>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                TilesetDefinition definition = defs[i];
                
                SerializedProperty tilesetObj = ArrayProp.GetArrayElementAtIndex(i);

                LDtkDrawerTileset drawer = new LDtkDrawerTileset(definition, tilesetObj, definition.Identifier);
                
                drawers.Add(drawer);
            }
        }

        protected override void DrawDropdownContent(TilesetDefinition[] datas)
        {
            base.DrawDropdownContent(datas);

            AutoAssetLinkerTilesets tilesetLinker = new AutoAssetLinkerTilesets();
            tilesetLinker.DrawButton(ArrayProp, datas, Project.ProjectJson);
        }
    }
}