// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public partial class FieldInstance : ILDtkIdentifier
    {
        public FieldDefinition Definition() => LDtkProviderUid.GetUidData<FieldDefinition>(DefUid);
    }
}