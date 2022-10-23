﻿using System.Text.Json.Serialization;

namespace LDtkUnity
{
    public partial class FieldDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Reference to the tileset that uses this icon. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public TilesetDefinition Tileset => TilesetUid == null ? null : LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid.Value);
    }
}