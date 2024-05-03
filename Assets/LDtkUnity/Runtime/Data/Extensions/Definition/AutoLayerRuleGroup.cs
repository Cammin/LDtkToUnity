using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class AutoLayerRuleGroup : ILDtkUid
    {
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
    }
}