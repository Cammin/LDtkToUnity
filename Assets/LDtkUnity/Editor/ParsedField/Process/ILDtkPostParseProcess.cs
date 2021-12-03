using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public interface ILDtkPostParseProcess<T>
    {
        T Postprocess(T value);
    }
}