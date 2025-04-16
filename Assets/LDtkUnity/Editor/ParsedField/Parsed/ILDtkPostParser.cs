namespace LDtkUnity.Editor
{
    internal interface ILDtkPostParser
    {
        void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldDefinition field);
    }
}