using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        /// <value>
        /// Parent group. null if not in a group <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public IntGridValueGroupDefinition Group => GroupUid == 0 ? null : LDtkUidBank.GetUidData<IntGridValueGroupDefinition>(GroupUid);
        
        /// <value>
        /// The "color" field converted for use with Unity
        /// </value>
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
    }
}