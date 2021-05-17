namespace LDtkUnity.Editor
{
    /*public class LDtkSectionTilesets : LDtkSectionDrawer<TilesetDefinition>
    {
        protected override string PropertyName => "LDtkProjectImporter.TILESETS";
        protected override string GuiText => "Tilesets";
        protected override string GuiTooltip => "The textures are used to generate Tile Collections in the section below this one.\n" +
                                                "Hit the button at the bottom of this dropdown to automatically assign them.";
        protected override Texture GuiImage => LDtkIconUtility.LoadTilesetIcon();
        
        public LDtkSectionTilesets(SerializedObject serializedObject) : base(serializedObject)
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

        /*protected override void DrawDropdownContent(TilesetDefinition[] datas)
        {
            base.DrawDropdownContent(datas);
            
            if (Project == null)
            {
                return;
            }
            
            LDtkRelativeAssetFinderTilesets tilesetFinder = new LDtkRelativeAssetFinderTilesets();
            tilesetFinder.GetRelativeAssets(datas, Project.JsonFile);
        }#1#
    }*/
}