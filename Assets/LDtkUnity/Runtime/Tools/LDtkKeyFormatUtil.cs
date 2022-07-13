using System.IO;
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
        /// A formatted string for getting an IntGrid Value serialized in the importer's IntGridValues.
        /// </returns>
        public static string IntGridValueFormat(LayerDefinition intGridLayerDef, IntGridValueDefinition def)
        {
            return $"{intGridLayerDef.Identifier}_{def.Value}";
        }
        internal static string IntGridValueFormat(string layerIdentifier, string intGridValue)
        {
            return $"{layerIdentifier}_{intGridValue}";
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
        /// A formatted string for getting a Sprite or art tile from the importer's imported sprites.
        /// </returns>
        public static string TileKeyFormat(string assetName, RectInt srcRect)
        {
            return $"{assetName}_{srcRect.x}_{srcRect.y}_{srcRect.width}_{srcRect.height}";
        }
        public static string TileKeyFormat(string assetName, Rect srcRect)
        {
            return $"{assetName}_{srcRect.x}_{srcRect.y}_{srcRect.width}_{srcRect.height}";
        }
        
        //needed when creating the asset.
        internal static string GetCreateSpriteOrTileAssetName(Rect rect, Texture2D tex)
        {
            if (tex == null)
            {
                LDtkDebug.LogError("Tried getting sprite/tile asset name for rect but the texture was null. Returning null instead");
                return null;
            }
            
            Rect imageSliceCoord = LDtkCoordConverter.ImageSlice(rect, tex.height);
            return TileKeyFormat(tex.name, imageSliceCoord);
        }
        
        //needed when creating the asset.
        internal static string GetDefaultTileAssetName(int ppu)
        {
            return $"Default_{ppu}";
        }
        
        //used when getting the created assets from artifacts.
        internal static string GetGetterSpriteOrTileAssetName(Rect rect, string assetRelPath, int texHeight)
        {
            string assetName = Path.GetFileNameWithoutExtension(assetRelPath);
            Rect imageSliceCoord = LDtkCoordConverter.ImageSlice(rect, texHeight);
            return TileKeyFormat(assetName, imageSliceCoord);
        }
    }
}