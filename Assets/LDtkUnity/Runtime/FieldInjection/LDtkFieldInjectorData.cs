using System.Reflection;

namespace LDtkUnity.FieldInjection
{
    public class LDtkFieldInjectorData
    {
        public readonly FieldInfo Info;
        public readonly string FieldIdentifier;
        public readonly object ObjectRef;

        public LDtkFieldInjectorData(FieldInfo info, string fieldIdentifier, object objectRef)
        {
            Info = info;
            FieldIdentifier = fieldIdentifier;
            ObjectRef = objectRef;
        }
    }
}