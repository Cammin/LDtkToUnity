namespace LDtkUnity
{
    public partial class FieldInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Reference of this instance's definition.
        /// </summary>
        public FieldDefinition Definition => LDtkUidBank.GetUidData<FieldDefinition>(DefUid);
    }
}