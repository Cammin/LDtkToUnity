namespace LDtkUnity
{
    public partial class FieldInstance : ILDtkIdentifier
    {
        public FieldDefinition Definition => LDtkProviderUid.GetUidData<FieldDefinition>(DefUid);
    }
}