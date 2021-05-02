using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LayerInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Deserialize LayerInstance from Json
        /// </summary>
        public static LayerInstance FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LayerInstance>(json, Converter.Settings);
        }
        
        /// <summary>
        /// Reference of this instance's definition.
        /// </summary>
        [JsonIgnore] public LayerDefinition Definition => LDtkUidBank.GetUidData<LayerDefinition>(LayerDefUid);
        
        /// <summary>
        /// The definition of corresponding Tileset, if any.
        /// </summary>
        [JsonIgnore] public TilesetDefinition TilesetDefinition => TilesetDefUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(TilesetDefUid.Value) : null;

        /// <summary>
        /// This layer can use another tileset by overriding the tileset here.
        /// </summary>
        [JsonIgnore] public TilesetDefinition OverrideTilesetDefinition => OverrideTilesetUid != null ? LDtkUidBank.GetUidData<TilesetDefinition>(OverrideTilesetUid.Value) : null;

        /// <summary>
        /// Reference to the level containing this layer instance
        /// </summary>
        [JsonIgnore] public Level LevelReference => LDtkUidBank.GetUidData<Level>(LevelId);
        
        /// <summary>
        /// Returns true if this layer is an IntGrid layer.
        /// </summary>
        [JsonIgnore] public bool IsIntGridLayer => !IntGridCsv.IsNullOrEmpty();
        
        /// <summary>
        /// Returns true if this layer is an Entities layer.
        /// </summary>
        [JsonIgnore] public bool IsEntitiesLayer => !EntityInstances.IsNullOrEmpty();
        
        /// <summary>
        /// Returns true if this layer is a Tiles layer.
        /// </summary>
        [JsonIgnore] public bool IsTilesLayer => !GridTiles.IsNullOrEmpty();
        
        /// <summary>
        /// Returns true if this layer is an Auto Layer.
        /// </summary>
        [JsonIgnore] public bool IsAutoLayer => !AutoLayerTiles.IsNullOrEmpty();
        
        /// <summary>
        /// Grid-based size
        /// </summary>
        [JsonIgnore] public Vector2Int UnityCellSize => new Vector2Int((int)CWid, (int)CHei);
        
        /// <summary>
        /// Total layer pixel offset, including both instance and definition offsets.
        /// </summary>
        [JsonIgnore] public Vector2Int UnityPxTotalOffset => new Vector2Int((int)PxTotalOffsetX, (int)PxTotalOffsetY);
        
        /// <summary>
        /// Offset in pixels to render this layer, usually 0,0
        /// </summary>
        [JsonIgnore] public Vector2Int UnityPxOffset => new Vector2Int((int)PxOffsetX, (int)PxOffsetY);
        
        /// <summary>
        /// A special Vector2 position that solves where the layer's position should be in Unity's world space based off of LDtk's top-left origin
        /// </summary>
        [JsonIgnore] public Vector2 UnityWorldPosition => LevelReference.UnityWorldSpaceCoord((int)GridSize);

        /// <summary>
        /// Total count of IntGrid values that are not empty spaces.
        /// </summary>
        [JsonIgnore] public int IntGridValueCount => IntGridCsv.Count(value => value != 0);
    }
}