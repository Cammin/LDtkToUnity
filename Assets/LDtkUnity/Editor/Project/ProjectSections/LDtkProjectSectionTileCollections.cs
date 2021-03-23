using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionTileCollections : LDtkProjectSectionDrawer<TilesetDefinition>
    {
        public LDtkProjectSectionTileCollections(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override string PropertyName => LDtkProject.TILE_COLLECTIONS;
        protected override string GuiText => "Tile Collections";
        protected override string GuiTooltip => "Tile Collections store tilemap tiles based on a texture's sliced sprites.\n" +
                                                "Generate the collections in the Tilesets section and then assign them here.\n" +
                                                "If the texture was only used for entity visuals in the LDtk editor, then it's not required to assign the field.";

        protected override Texture GuiImage => LDtkIconLoader.GetUnityIcon("Tilemap");

        protected override void GetDrawers(TilesetDefinition[] defs, List<LDtkContentDrawer<TilesetDefinition>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                TilesetDefinition definition = defs[i];
                SerializedProperty tileCollection = ArrayProp.GetArrayElementAtIndex(i);
                LDtkDrawerTileCollection drawer = new LDtkDrawerTileCollection(definition, tileCollection, definition.Identifier);
                drawers.Add(drawer);
            }
        }
    }
}