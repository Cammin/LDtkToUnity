using System.Collections.Generic;
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
        
        public byte[] ToJson()
        {
            return Utf8Json.JsonSerializer.Serialize(this);
        }
        
        /// <value>
        /// Enum definition used for this tileset meta-data. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public EnumDefinition TagsSourceEnum => TagsSourceEnumUid != null ? LDtkUidBank.GetUidData<EnumDefinition>(TagsSourceEnumUid.Value) : null;

        /// <value>
        /// Grid based size
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityCSize => new Vector2Int(CWid, CHei);
        
        /// <value>
        /// Image size in pixels
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityPxSize => new Vector2Int(PxWid, PxHei);

        /// <value>
        /// If this value is true, then it means that this definition uses an internal LDtk atlas image
        /// instead of a loaded one.
        /// </value>
        [IgnoreDataMember] public bool IsEmbedAtlas => EmbedAtlas != null;
        
        internal Dictionary<int, string> CustomDataToDictionary()
        {
            Dictionary<int,string> dict = new Dictionary<int, string>(CustomData.Length);
            foreach (TileCustomMetadata metadata in CustomData)
            {
                if (!dict.ContainsKey(metadata.TileId))
                {
                    dict.Add(metadata.TileId, metadata.Data);
                    continue;
                }
                dict[metadata.TileId] = metadata.Data;
            }
            return dict;
        }
        internal Dictionary<int, List<string>> EnumTagsToDictionary()
        {
            Dictionary<int, List<string>> dict = new Dictionary<int, List<string>>();
            foreach (EnumTagValue tagValue in EnumTags)
            {
                foreach (int tileId in tagValue.TileIds)
                {
                    if (!dict.ContainsKey(tileId))
                    {
                        dict.Add(tileId, new List<string>());
                    }
                    dict[tileId].Add(tagValue.EnumValueId);
                }
            }
            return dict;
        }
    }
}