// ReSharper disable InconsistentNaming

using LDtkUnity.Data;
using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity
{
    public partial class EntityInstance : ILDtkIdentifier
    {
        public EntityDefinition Definition => LDtkProviderUid.GetUidData<EntityDefinition>(DefUid);
        public Vector2Int UnityPx => Px.ToVector2Int();
    }
}