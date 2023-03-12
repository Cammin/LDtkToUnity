﻿using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class AutoLayerRuleDefinition : ILDtkUid
    {
        /// <value>
        /// Pivot of a tile stamp (0-1)
        /// </value>
        [IgnoreDataMember] public Vector2 UnityPivot => new Vector2(PivotX, PivotY);
        
        /// <value>
        /// Cell coord modulo
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityModulo => new Vector2Int(XModulo, YModulo);
        
        /// <value>
        /// Cell start offset
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityOffset => new Vector2Int(XOffset, YOffset);
    }
}