using System.Reflection;

namespace LDtkUnity.FieldInjection
{
    public class LDtkFieldInjectorData
    {
        public readonly FieldInfo Info;
        public readonly string FieldIdentifier;
        public readonly object ObjectRef;

        //field, fieldname, monobehaviour ref
        public LDtkFieldInjectorData(FieldInfo info, string fieldIdentifier, object objectRef)
        {
            Info = info;
            FieldIdentifier = fieldIdentifier;
            ObjectRef = objectRef;
        }
    }
}