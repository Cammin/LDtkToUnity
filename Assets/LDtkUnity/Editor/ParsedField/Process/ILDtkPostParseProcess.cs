namespace LDtkUnity.Editor
{
    public interface ILDtkPostParseProcess<T>
    {
        public T Postprocess(T value);
    }
}