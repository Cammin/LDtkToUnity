using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// A utility class used in conjunction with <see cref="LDtkArtifactAssets"/> to get certain assets by name.
    /// </summary>
    public static class LDtkKeyFormatUtil
    {
        /// <summary>
        /// Creates a formatted string usable for getting a <see cref="LDtkIntGridTile"/> by name in the importer.
        /// </summary>
        /// <param name="intGridLayerDef">
        /// The layer definition used for it's identifier.
        /// </param>
        /// <param name="def">
        /// The definition of the IntGrid Value.
        /// </param>
        /// <returns>
        /// A formatted string for getting an IntGrid Value from the importer's IntGridValues configuration.
        /// </returns>
        public static string IntGridValueFormat(LayerDefinition intGridLayerDef, IntGridValueDefinition def)
        {
            return $"{intGridLayerDef.Identifier}_{def.Value}";
        }
        
        /// <summary>
        /// Creates a formatted string usable for getting a sprite by name in the imported <see cref="LDtkArtifactAssets"/> object.
        /// </summary>
        /// <param name="tex">
        /// The texture.
        /// </param>
        /// <param name="srcRect">
        /// The source rectangle.
        /// </param>
        /// <returns>
        /// A formatted string for getting a Sprite from the importer's imported sprites.
        /// </returns>
        public static string TilesetKeyFormat(Texture2D tex, RectInt srcRect)
        {
            return $"{tex.name}_{srcRect.x}_{srcRect.y}_{srcRect.width}_{srcRect.height}";
        }
        /// <summary>
        /// Creates a formatted string usable for getting a sprite by name in the imported <see cref="LDtkArtifactAssets"/> object.
        /// </summary>
        /// <param name="assetName">
        /// The texture's name.
        /// </param>
        /// <param name="srcRect">
        /// The source rectangle.
        /// </param>
        /// <returns>
        /// A formatted string for getting a Sprite from the importer's imported sprites.
        /// </returns>
        public static string TilesetKeyFormat(string assetName, RectInt srcRect)
        {
            return $"{assetName}_{srcRect.x}_{srcRect.y}_{srcRect.width}_{srcRect.height}";
        }
        
        
        internal static string GetSpriteOrTileAssetName(Texture2D tex, RectInt rect)
        {
            if (tex == null)
            {
                LDtkDebug.LogError("Tried getting sprite/tile asset name for rect but the texture was null. Returning \"corrupted\" instead");
                return "corrupted";
            }
            
            RectInt imageSliceCoord = LDtkCoordConverter.ImageSlice(rect, tex.height);
            string key = TilesetKeyFormat(tex, imageSliceCoord);
            return key;
        }
        internal static string GetSpriteOrTileAssetName(string assetName, RectInt rect, int textureHeight)
        {
            RectInt imageSliceCoord = LDtkCoordConverter.ImageSlice(rect, textureHeight);
            string key = TilesetKeyFormat(assetName, imageSliceCoord);
            return key;
        }
    }
}