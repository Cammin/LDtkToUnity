using LDtkUnity.Runtime.FieldInjection;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#113-field-instance
    public struct LDtkDataField
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string __identifier;
        
        /// <summary>
        /// Type of the field, such as Int, Float, Enum(enum_name), Bool, etc.
        /// </summary>
        public string __type;
        
        /// <summary>
        /// Actual value of the field instance. The value type may vary, depending on __type (Integer, Boolean, String etc.)
        /// It can also be an Array of various types.
        /// </summary>
        [JsonConverter(typeof(LDtkDataEntityInstanceFieldJsonConverter))]
        public string[] __value;
        
        /// <summary>
        /// Reference of the Field definition UID
        /// </summary>
        public int defUid;
    }
}