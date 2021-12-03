using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public interface ILDtkPostParser
    {
        void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field);
    }
}