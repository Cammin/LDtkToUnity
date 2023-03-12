using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class TilesetDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Used for deserializing ".ldtkt" files.
        /// </summary>
        /// <param name="json">
        /// The LDtk json string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized tileset definition.
        /// </returns>
        public static TilesetDefinition FromJson(string json)
        {
            return Utf8Json.JsonSerializer.Deserialize<TilesetDefinition>(json);
        }
        
        /// <value>
        /// Enum definition used for this tileset meta-data. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public EnumDefinition TagsSourceEnum => TagsSourceEnumUid != null ? LDtkUidBank.GetUidData<EnumDefinition>(TagsSourceEnumUid.Value) : null;

        /// <value>
        /// Image size in pixels
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityPxSize => new Vector2Int(PxWid, PxHei);

        /// <value>
        /// If this value is true, then it means that this definition uses an internal LDtk atlas image
        /// instead of a loaded one.
        /// </value>
        [IgnoreDataMember] public bool IsEmbedAtlas => EmbedAtlas != null;
    }
}