using UnityEngine;

namespace LDtkUnity
{
    public partial class LayerDefinition : ILDtkUid, ILDtkIdentifier
    {
        public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkProviderUid.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;

        public TilesetDefinition AutoTilesetDefinition => AutoTilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(AutoTilesetDefUid.Value) : null;

        public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkProviderUid.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;
        
        public bool IsIntGridLayer => string.Equals(Type, "IntGrid");
        public bool IsEntitiesLayer => string.Equals(Type, "Entities");
        public bool IsTilesLayer => string.Equals(Type, "Tiles");
        public bool IsAutoLayer => string.Equals(Type, "AutoLayer");
    }
}