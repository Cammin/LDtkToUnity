// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionLayerExtensions
    {
        public static bool IsIntGridLayer(this LDtkDefinitionLayer definition) => !(definition.intGridValues.Length == 1 && definition.intGridValues[0].Color() == Color.black); //TODO somewhat hacky, but works
        public static LDtkDefinitionLayer AutoSourceLayerDefinition(this LDtkDefinitionLayer definition) => LDtkProviderUid.GetUidData<LDtkDefinitionLayer>(definition.autoSourceLayerDefUid);
        public static LDtkDefinitionTileset AutoTilesetDefinition(this LDtkDefinitionLayer definition) => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(definition.autoTilesetDefUid);
        public static LDtkDefinitionTileset TileLayerDefinition(this LDtkDefinitionLayer definition) => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(definition.tilesetDefUid);
    }
}