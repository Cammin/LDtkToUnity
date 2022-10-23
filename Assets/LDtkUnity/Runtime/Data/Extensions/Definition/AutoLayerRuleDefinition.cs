using System.Text.Json.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class AutoLayerRuleDefinition : ILDtkUid
    {
        /// <value>
        /// Pivot of a tile stamp (0-1)
        /// </value>
        [JsonIgnore] public Vector2 UnityPivot => new Vector2((float)PivotX, (float)PivotY);
        
        /// <value>
        /// Cell coord modulo
        /// </value>
        [JsonIgnore] public Vector2Int UnityModulo => new Vector2Int((int)XModulo, (int)YModulo);
        
        /// <value>
        /// Cell start offset
        /// </value>
        [JsonIgnore] public Vector2Int UnityOffset => new Vector2Int((int)XOffset, (int)YOffset);
    }
}