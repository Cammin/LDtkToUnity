using System.Reflection;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection
{
    public class LDtkInjectableField
    {
        public readonly FieldInfo Info;
        public readonly string FieldIdentifier;
        public readonly object ObjectRef;

        public LDtkInjectableField(FieldInfo info, string fieldIdentifier, object objectRef)
        {
            Info = info;
            FieldIdentifier = fieldIdentifier;
            ObjectRef = objectRef;
        }
    }
}