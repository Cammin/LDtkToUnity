namespace LDtkUnity.Editor
{
    public interface ILDtkPostParseProcess<T>
    {
        T Postprocess(T value);
    }
}