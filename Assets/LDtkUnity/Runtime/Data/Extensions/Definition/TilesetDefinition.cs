using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class TilesetDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Enum definition used for this tileset meta-data
        /// </summary>
        [JsonIgnore] public EnumDefinition TagsSourceEnum => TagsSourceEnumUid != null ? LDtkUidBank.GetUidData<EnumDefinition>(TagsSourceEnumUid.Value) : null;

        /// <summary>
        /// Image size in pixels
        /// </summary>
        [JsonIgnore] public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
    }
}