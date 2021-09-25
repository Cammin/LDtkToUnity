namespace LDtkUnity.Editor
{
    public interface ILDtkPostParser
    {
        void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field);
    }
}