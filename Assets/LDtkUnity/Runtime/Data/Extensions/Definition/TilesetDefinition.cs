using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TilesetDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Enum definition used for this tileset meta-data. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public EnumDefinition TagsSourceEnum => TagsSourceEnumUid != null ? LDtkUidBank.GetUidData<EnumDefinition>(TagsSourceEnumUid.Value) : null;

        /// <value>
        /// Image size in pixels
        /// </value>
        [JsonIgnore] public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);

        /// <value>
        /// If this value is true, then it means that this definition uses an internal LDtk atlas image
        /// instead of a loaded one.
        /// </value>
        [JsonIgnore] public bool IsEmbedAtlas => EmbedAtlas != null;
    }
}