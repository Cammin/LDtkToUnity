using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    public partial class AutoLayerRuleGroup : ILDtkUid
    {
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
    }
}