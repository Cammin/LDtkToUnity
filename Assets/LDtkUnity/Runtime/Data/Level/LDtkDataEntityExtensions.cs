// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public static class LDtkDataEntityExtensions
    {
        public static LDtkDefinitionEntity Definition(this LDtkDataEntity data) => LDtkProviderUid.GetUidData<LDtkDefinitionEntity>(data.defUid);
    }
}