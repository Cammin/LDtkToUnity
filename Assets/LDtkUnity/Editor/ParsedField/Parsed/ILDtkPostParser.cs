using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal interface ILDtkPostParser
    {
        void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field);
    }
}