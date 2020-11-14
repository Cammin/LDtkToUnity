// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.FieldInjection;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#113-field-instance
    public struct LDtkDataField
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string __identifier { get; private set; }
        
        /// <summary>
        /// Type of the field, such as Int, Float, Enum(enum_name), Bool, etc.
        /// </summary>
        [JsonProperty] public string __type { get; private set; }
        
        /// <summary>
        /// Actual value of the field instance. The value type may vary, depending on __type (Integer, Boolean, String etc.)
        /// It can also be an Array of various types.
        /// </summary>
        [JsonConverter(typeof(LDtkDataEntityInstanceFieldJsonConverter))]
        [JsonProperty] public string[] __value { get; private set; }
        
        /// <summary>
        /// Reference of the Field definition UID
        /// </summary>
        [JsonProperty] public int defUid { get; private set; }

        public LDtkDefinitionField Definition => LDtkProviderUid.GetUidData<LDtkDefinitionField>(defUid);
    }
}