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
        [JsonIgnore] public bool IsInt => Type == "Int" || Type == "Array<Int>";
        
        /// <value>
        /// Returns if this field (or array element) type is a Float. 
        /// </value>
        [JsonIgnore] public bool IsFloat => Type == "Float" || Type == "Array<Float>";
        
        /// <value>
        /// Returns if this field (or array element) type is a Bool. 
        /// </value>
        [JsonIgnore] public bool IsBool => Type == "Bool" || Type == "Array<Bool>";
        
        /// <value>
        /// Returns if this field (or array element) type is a String. 
        /// </value>
        [JsonIgnore] public bool IsString => Type == "String" || Type == "Array<String>";

        /// <value>
        /// Returns if this field (or array element) type is MultiLines. 
        /// </value>
        [JsonIgnore] public bool IsMultilines => Type == "Multilines" || Type == "Array<Multilines>";
        
        /// <value>
        /// Returns if this field (or array element) type is a FilePath. 
        /// </value>
        [JsonIgnore] public bool IsFilePath => Type == "FilePath" || Type == "Array<FilePath>";
        
        /// <value>
        /// Returns if this field (or array element) type is a Color. 
        /// </value>
        [JsonIgnore] public bool IsColor => Type == "Color" || Type == "Array<Color>";

        /// <value>
        /// Returns if this field (or array element) type is an Enum. 
        /// </value>
        [JsonIgnore]
        public bool IsEnum => Type.StartsWith("LocalEnum.") || Type.StartsWith("Array<LocalEnum.") ||
                              Type.StartsWith("ExternEnum.") || Type.StartsWith("Array<ExternEnum.");

        /// <value>
        /// Returns if this field (or array element) type is a Tile. 
        /// </value>
        [JsonIgnore] public bool IsTile => Type == "Tile" || Type == "Array<Tile>";    
        
        /// <value>
        /// Returns if this field (or array element) type is a EntityRef. 
        /// </value>
        [JsonIgnore] public bool IsEntityRef => Type == "EntityRef" || Type == "Array<EntityRef>";    
        
        /// <value>
        /// Returns if this field (or array element) type is a Point. 
        /// </value>
        [JsonIgnore] public bool IsPoint => Type == "Point" || Type == "Array<Point>";
    }
}