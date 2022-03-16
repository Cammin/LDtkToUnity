namespace LDtkUnity
{
    public struct FieldsResult<T>
    {
        public T Value;
        public bool Success;
        
        public static FieldsResult<T> Null()
        {
            return new FieldsResult<T>()
            {
                Value = default,
                Success = false
            };
        }
    }
}