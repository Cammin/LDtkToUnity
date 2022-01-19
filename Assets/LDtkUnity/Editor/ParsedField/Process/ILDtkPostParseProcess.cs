using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal interface ILDtkPostParseProcess<T>
    {
        T Postprocess(T value);
    }
}