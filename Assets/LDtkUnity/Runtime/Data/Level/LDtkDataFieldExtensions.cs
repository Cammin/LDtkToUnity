// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public static class LDtkDataFieldExtensions
    {
        public static LDtkDefinitionField Definition(this LDtkDataField data) => LDtkProviderUid.GetUidData<LDtkDefinitionField>(data.defUid);
    }
}