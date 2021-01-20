// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;

namespace LDtkUnity.Data
{
    public partial class FieldInstance
    {
        public FieldDefinition Definition() => LDtkProviderUid.GetUidData<FieldDefinition>(DefUid);
    }
}