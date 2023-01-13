using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class GridPoint
    {
        /// <value>
        /// Grid-based coordinate
        /// </value>
        [JsonIgnore] public Vector2Int UnityCoord => new Vector2Int((int)Cx, (int)Cy);
        
        internal static GridPoint FromJson(string json)
        {
            return JsonConvert.DeserializeObject<GridPoint>(json, Converter.Settings);
        }
    }
}