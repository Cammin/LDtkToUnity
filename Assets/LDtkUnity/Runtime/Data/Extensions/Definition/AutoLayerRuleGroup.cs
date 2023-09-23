using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]//keep like this until we add custom functionality
    public partial class AutoLayerRuleGroup : ILDtkUid
    {
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
    }
}