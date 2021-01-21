// ReSharper disable InconsistentNaming

using System;
using LDtkUnity.Data;
using LDtkUnity.Providers;

namespace LDtkUnity
{
    public partial class FieldInstance : ILDtkIdentifier
    {
        public FieldDefinition Definition => LDtkProviderUid.GetUidData<FieldDefinition>(DefUid);
    }
}