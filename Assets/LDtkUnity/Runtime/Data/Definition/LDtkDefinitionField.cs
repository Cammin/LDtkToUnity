// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#221-field-definition
    public struct LDtkDefinitionField : ILDtkUid, ILDtkIdentifier
    {
        //todo documentation for it not available yet. some of these values are just to try

        
        [JsonProperty] public string identifier { get; private set; }
        [JsonProperty] public string __type { get; private set; }
        [JsonProperty] public int uid { get; private set; }

    }
}