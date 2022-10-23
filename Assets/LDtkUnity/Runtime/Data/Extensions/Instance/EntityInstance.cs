﻿using System.Text.Json.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class EntityInstance : ILDtkIdentifier, ILDtkIid
    {
        /// <value>
        /// Reference of this instance's definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public EntityDefinition Definition => LDtkUidBank.GetUidData<EntityDefinition>(DefUid);
        
        /// <value>
        /// Pixel coordinates in current level coordinate space. Don't forget optional layer offsets, if they exist!
        /// </value>
        [JsonIgnore] public Vector2Int UnityPx => Px.ToVector2Int();
        
        /// <value>
        /// Pivot coordinates of the Entity. (values are from 0 to 1)
        /// </value>
        [JsonIgnore] public Vector2 UnityPivot => Pivot.ToVector2();
        
        /// <value>
        /// The entity "smart" color, guessed from either Entity definition, or one its field
        /// instances.
        /// </value>
        [JsonIgnore] public Color UnitySmartColor => string.IsNullOrEmpty(SmartColor) ? Definition.UnityColor : SmartColor.ToColor();
        
        /// <value>
        /// Grid-based coordinates
        /// </value>
        [JsonIgnore] public Vector2Int UnityGrid => Grid.ToVector2Int();
        
        /// <value>
        /// Entity size in pixels, adjusted for this instance's resizing.
        /// </value>
        [JsonIgnore] public Vector2Int UnitySize => new Vector2Int((int)Width, (int)Height);
        
        /// <value>
        /// Entity scale multiplier, suitable for a transform's scale.
        /// </value>
        [JsonIgnore] public Vector3 UnityScale => new Vector3(UnitySize.x / (float) Definition.UnitySize.x, UnitySize.y / (float) Definition.UnitySize.y, 1);
    }
}