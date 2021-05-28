using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Json Instance Data
    /// </summary>
    public partial class FieldInstance : ILDtkIdentifier
    {
        /// <value>
        /// Reference of this instance's definition. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [JsonIgnore] public FieldDefinition Definition => LDtkUidBank.GetUidData<FieldDefinition>(DefUid);

        /// <value>
        /// Returns if this field (or array element) type is an Int. 
        /// </value>
        [JsonIgnore] public bool IsInt => Type.Contains("Int");
        
        /// <value>
        /// Returns if this field (or array element) type is a Float. 
        /// </value>
        [JsonIgnore] public bool IsFloat => Type.Contains("Float");
        
        /// <value>
        /// Returns if this field (or array element) type is a Bool. 
        /// </value>
        [JsonIgnore] public bool IsBool => Type.Contains("Bool");
        
        /// <value>
        /// Returns if this field (or array element) type is a String. 
        /// </value>
        [JsonIgnore] public bool IsString => Type.Contains("String");

        /// <value>
        /// Returns if this field (or array element) type is MultiLines. 
        /// </value>
        [JsonIgnore] public bool IsMultilines => Type.Contains("MultiLines");
        
        /// <value>
        /// Returns if this field (or array element) type is a FilePath. 
        /// </value>
        [JsonIgnore] public bool IsFilePath => Type.Contains("FilePath");
        
        /// <value>
        /// Returns if this field (or array element) type is a Color. 
        /// </value>
        [JsonIgnore] public bool IsColor => Type.Contains("Color");
        
        /// <value>
        /// Returns if this field (or array element) type is an Enum. 
        /// </value>
        [JsonIgnore] public bool IsEnum => Type.Contains("LocalEnum");
        
        /// <value>
        /// Returns if this field (or array element) type is a Point. 
        /// </value>
        [JsonIgnore] public bool IsPoint => Type.Contains("Point");
    }
}