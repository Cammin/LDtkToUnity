using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TilesetRectangle
    {
        /// <value>
        /// Tileset used for this rectangle data. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition Tileset => LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid);
        
        /// <value>
        /// Rectangle of the tile in the Tileset atlas
        /// </value>
        [JsonIgnore] public Rect UnityRect => new Rect(X, Y, W, H);

        internal static TilesetRectangle FromJson(string json)
        {
            return JsonSerializer.Deserialize<TilesetRectangle>(json, Converter.Settings);
        }
        internal static TilesetRectangle[] FromJsonToArray(string json)
        {
            return JsonSerializer.Deserialize<TilesetRectangle[]>(json, Converter.Settings);
        }
        
        protected bool Equals(TilesetRectangle other)
        {
            return H == other.H && 
                   TilesetUid == other.TilesetUid && 
                   W == other.W && 
                   X == other.X && 
                   Y == other.Y;
        }

        public override string ToString() => $"{X}_{Y}, {W}x{H}, uid:{TilesetUid}";
    }
}