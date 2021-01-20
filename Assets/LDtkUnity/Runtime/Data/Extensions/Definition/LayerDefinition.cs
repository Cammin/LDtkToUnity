// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using UnityEngine;

namespace LDtkUnity.Data
{
    public partial class LayerDefinition : ILDtkUid, ILDtkIdentifier
    {
        public bool IsIntGridLayer => !(IntGridValues.Length == 1 && IntGridValues[0].Color() == Color.black); //TODO somewhat hacky, but works
        
        public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkProviderUid.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;

        public TilesetDefinition AutoTilesetDefinition => AutoTilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(AutoTilesetDefUid.Value) : null;

        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;
    }
}