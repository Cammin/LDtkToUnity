using Newtonsoft.Json;
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
        [JsonIgnore] public RectInt UnityRect => new RectInt((int)X, (int)Y, (int)W, (int)H);

        internal static TilesetRectangle FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TilesetRectangle>(json, Converter.Settings);
        }
    }
    
    //the schema export. It wasn't generated automatically.
    /// <summary>
    /// This object represents a custom sub rectangle in a Tileset image.
    /// </summary>
    public partial class TilesetRectangle
    {
        /// <value>
        /// Height in pixels
        /// </value>
        [JsonProperty("h")]
        public long H { get; set; }
        
        /// <value>
        /// UID of the tileset
        /// </value>
        [JsonProperty("tilesetUid")]
        public long TilesetUid { get; set; }
        
        /// <value>
        /// Width in pixels
        /// </value>
        [JsonProperty("w")]
        public long W { get; set; }

        /// <value>
        /// X pixels coordinate of the top-left corner in the Tileset image
        /// </value>
        [JsonProperty("x")]
        public long X { get; set; }

        /// <value>
        /// Y pixels coordinate of the top-left corner in the Tileset image
        /// </value>
        [JsonProperty("y")]
        public long Y { get; set; }
    }
}