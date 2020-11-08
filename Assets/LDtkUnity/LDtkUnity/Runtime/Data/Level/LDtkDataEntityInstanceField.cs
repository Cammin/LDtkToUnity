using LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Level
{
    public struct LDtkDataEntityInstanceField
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string __identifier;
        
        /// <summary>
        /// Actual value of the field instance. The value type may vary, depending on __type (Integer, Boolean, String etc.)
        /// It can also be an Array of various types.
        /// </summary>
        [JsonConverter(typeof(LDtkDataEntityInstanceFieldJsonConverter))]
        public string[] __value;
        
        /// <summary>
        /// Type of the field, such as Int, Float, Enum(enum_name), Bool, etc.
        /// </summary>
        public string __type;
        
        /// <summary>
        /// Reference of the Field definition UID
        /// </summary>
        public int defUid;
    }
}