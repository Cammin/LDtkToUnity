// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataEntityExtensions
    {
        public static LDtkDefinitionEntity Definition(this LDtkDataEntity data) => LDtkProviderUid.GetUidData<LDtkDefinitionEntity>(data.defUid);
        public static Vector2Int Px(this LDtkDataEntity data) => data.px.ToVector2Int();
    }
}