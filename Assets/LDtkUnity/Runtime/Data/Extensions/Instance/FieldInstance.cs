using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class FieldInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Reference of this instance's definition.
        /// </summary>
        [JsonIgnore] public FieldDefinition Definition => LDtkUidBank.GetUidData<FieldDefinition>(DefUid);

        /// <summary>
        /// Returns if this field (or array element) type is an Int. 
        /// </summary>
        [JsonIgnore] public bool IsInt => Type.Contains("Int");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Float. 
        /// </summary>
        [JsonIgnore] public bool IsFloat => Type.Contains("Float");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Bool. 
        /// </summary>
        [JsonIgnore] public bool IsBool => Type.Contains("Bool");
        
        /// <summary>
        /// Returns if this field (or array element) type is a String. 
        /// </summary>
        [JsonIgnore] public bool IsString => Type.Contains("String");

        /// <summary>
        /// Returns if this field (or array element) type is MultiLines. 
        /// </summary>
        [JsonIgnore] public bool IsMultilines => Type.Contains("MultiLines");
        
        /// <summary>
        /// Returns if this field (or array element) type is a FilePath. 
        /// </summary>
        [JsonIgnore] public bool IsFilePath => Type.Contains("FilePath");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Color. 
        /// </summary>
        [JsonIgnore] public bool IsColor => Type.Contains("Color");
        
        /// <summary>
        /// Returns if this field (or array element) type is an Enum. 
        /// </summary>
        [JsonIgnore] public bool IsEnum => Type.Contains("LocalEnum");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Point. 
        /// </summary>
        [JsonIgnore] public bool IsPoint => Type.Contains("Point");
    }
}