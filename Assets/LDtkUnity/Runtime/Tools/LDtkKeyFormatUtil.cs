using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    /// <summary>
    /// A utility class used in conjunction with <see cref="LDtkArtifactAssets"/> to get certain assets by name.
    /// </summary>
    [ExcludeFromDocs]
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
        public static string TilesetKeyFormat(Texture2D tex, Vector2 srcRect)
        {
            return $"{tex.name}_x{srcRect.x}_y{srcRect.y}";
        }
    }
}