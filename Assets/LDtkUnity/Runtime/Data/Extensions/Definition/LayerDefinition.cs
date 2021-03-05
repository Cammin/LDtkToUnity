using UnityEngine;

namespace LDtkUnity
{
    public partial class LayerDefinition : ILDtkUid, ILDtkIdentifier
    {
        public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkProviderUid.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;

        public TilesetDefinition AutoTilesetDefinition => AutoTilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(AutoTilesetDefUid.Value) : null;

        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;
        
        public bool IsIntGridLayer => !IntGridValues.NullOrEmpty() && !(IntGridValues.Length == 1 && IntGridValues[0].UnityColor == Color.black); //TODO 2nd AND check is for the instances where the tiles are completely empty
    }
}