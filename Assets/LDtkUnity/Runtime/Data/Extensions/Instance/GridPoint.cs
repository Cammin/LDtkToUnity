using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public class FieldInstancePoint
    {
        [JsonProperty("cx")]
        public int Cx { get; set; }
        
        [JsonProperty("cy")]
        public int Cy { get; set; }

        /// <value>
        /// Grid-based coordinate
        /// </value>
        [JsonIgnore] public Vector2Int UnityCoord => new Vector2Int(Cx, Cy);
        
        internal static FieldInstancePoint FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FieldInstancePoint>(json, Converter.Settings);
        }
    }
}