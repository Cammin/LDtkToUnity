namespace LDtkUnity
{
    public interface ILDtkPostParser<T>
    {
        public T Postprocess(T value);
    }
}