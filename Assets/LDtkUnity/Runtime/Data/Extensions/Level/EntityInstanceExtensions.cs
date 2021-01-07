// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class EntityInstanceExtensions
    {
        public static EntityDefinition Definition(this EntityInstance data) => LDtkProviderUid.GetUidData<EntityDefinition>(data.DefUid);
        public static Vector2Int UnityPx(this EntityInstance data) => data.Px.ToVector2Int();
    }
}