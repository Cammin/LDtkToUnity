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
}