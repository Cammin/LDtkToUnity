// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public static class FieldInstanceExtensions
    {
        public static FieldDefinition Definition(this FieldInstance data) => LDtkProviderUid.GetUidData<FieldDefinition>(data.DefUid);
    }
}