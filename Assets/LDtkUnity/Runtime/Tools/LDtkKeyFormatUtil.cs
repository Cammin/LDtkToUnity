using System.Collections.Generic;
using System.Text;

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
        // The reason for naming them as such is because it can't be done from simple indexing.
        public static string IntGridValueFormat(LayerDefinition intGridLayerDef, IntGridValueDefinition def)
        {
            return $"{intGridLayerDef.Identifier}_{def.Value}";
        }
        internal static string IntGridValueFormat(string layerIdentifier, string intGridValue)
        {
            return $"{layerIdentifier}_{intGridValue}";
        }
    }
}