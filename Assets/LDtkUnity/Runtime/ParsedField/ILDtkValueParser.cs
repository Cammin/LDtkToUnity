using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public interface ILDtkValueParser
    {
        bool TypeName(FieldInstance instance);
        object ImportString(object input);
    }
}