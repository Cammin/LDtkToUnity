namespace LDtkUnity.Editor
{
    public interface ILDtkPostParser
    {
        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field);
    }
}