// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDefinitionLayerExtensions
    {
        public static bool IsIntGridLayer(this LayerDefinition definition) => !(definition.IntGridValues.Length == 1 && definition.IntGridValues[0].Color() == Color.black); //TODO somewhat hacky, but works
        
        public static LayerDefinition AutoSourceLayerDefinition(this LayerDefinition definition)
        {
            if (definition.AutoSourceLayerDefUid != null)
            {
                return LDtkProviderUid.GetUidData<LayerDefinition>(definition.AutoSourceLayerDefUid.Value);
            }
            return null;
        }

        public static TilesetDefinition AutoTilesetDefinition(this LayerDefinition definition)
        {
            if (definition.AutoTilesetDefUid != null)
            {
                return LDtkProviderUid.GetUidData<TilesetDefinition>(definition.AutoTilesetDefUid.Value);
            }
            return null;
        }

        public static TilesetDefinition TilesetDefinition(this LayerDefinition definition)
        {
            if (definition.TilesetDefUid != null)
            {
                return LDtkProviderUid.GetUidData<TilesetDefinition>(definition.TilesetDefUid.Value);
            }
            return null;
            
        }
    }
}