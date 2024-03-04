using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class LayerDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Reference to the AutoLayer source definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public LayerDefinition AutoSourceLayerDefinition => AutoSourceLayerDefUid != null ? LDtkUidBank.GetUidData<LayerDefinition>(AutoSourceLayerDefUid.Value) : null;
        
        /// <value>
        /// Reference to the tileset definition being used by this Tile layer. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        /// <value>
        /// Parallax factor (from -1 to 1, defaults to 0) which affects the scrolling
        /// speed of this layer, creating a fake 3D (parallax) effect.
        /// </value>
        [IgnoreDataMember] public Vector2 ParallaxFactor => new Vector2(ParallaxFactorX, ParallaxFactorY);
        
        /// <value>
        /// Offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </value>
        [IgnoreDataMember] public Vector2Int PxOffset => new Vector2Int(PxOffsetX, PxOffsetY);
        
        /// <summary>
        /// Size of the optional "guide" grid in pixels
        /// </summary>
        [IgnoreDataMember] public Vector2Int GuideGridSize => new Vector2Int(GuideGridWid, GuideGridHei);
        
        /// <value>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </value>
        [IgnoreDataMember] public Vector2 TilePivot => new Vector2(TilePivotX, TilePivotY);

        /// <summary>
        /// User defined color for the UI
        /// </summary>
        [IgnoreDataMember] public Color UnityUiColor => UiColor.ToColor();
        
        /// <value>
        /// Returns true if this layer is an IntGrid layer.
        /// </value>
        [IgnoreDataMember] public bool IsIntGridLayer => LayerDefinitionType == TypeEnum.IntGrid;
        
        /// <value>
        /// Returns true if this layer is an Entities layer.
        /// </value>
        [IgnoreDataMember] public bool IsEntitiesLayer => LayerDefinitionType == TypeEnum.Entities;
        
        /// <value>
        /// Returns true if this layer is a Tiles layer.
        /// </value>
        [IgnoreDataMember] public bool IsTilesLayer => LayerDefinitionType == TypeEnum.Tiles;
        
        /// <value>
        /// Returns true if this layer is an Auto Layer.
        /// </value>
        [IgnoreDataMember] public bool IsAutoLayer => LayerDefinitionType == TypeEnum.AutoLayer;
    }
}