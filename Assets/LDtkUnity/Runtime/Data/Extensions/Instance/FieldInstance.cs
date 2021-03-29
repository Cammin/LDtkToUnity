namespace LDtkUnity
{
    public partial class FieldInstance : ILDtkIdentifier
    {
        /// <summary>
        /// Reference of this instance's definition.
        /// </summary>
        public FieldDefinition Definition => LDtkUidBank.GetUidData<FieldDefinition>(DefUid);

        /// <summary>
        /// Returns if this field (or array element) type is an Int. 
        /// </summary>
        public bool IsInt => Type.Contains("Int");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Float. 
        /// </summary>
        public bool IsFloat => Type.Contains("Float");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Bool. 
        /// </summary>
        public bool IsBool => Type.Contains("Bool");
        
        /// <summary>
        /// Returns if this field (or array element) type is a String. 
        /// </summary>
        public bool IsString => Type.Contains("String");
        
        /// <summary>
        /// Returns if this field (or array element) type is a FilePath. 
        /// </summary>
        public bool IsFilePath => Type.Contains("FilePath");
        
        /// <summary>
        /// Returns if this field (or array element) type is MultiLines. 
        /// </summary>
        public bool IsMultiLines => Type.Contains("MultiLines");
        
        /// <summary>
        /// Returns if this field (or array element) type is an Enum. 
        /// </summary>
        public bool IsEnum => Type.Contains("LocalEnum");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Color. 
        /// </summary>
        public bool IsColor => Type.Contains("Color");
        
        /// <summary>
        /// Returns if this field (or array element) type is a Point. 
        /// </summary>
        public bool IsPoint => Type.Contains("Point");
    }
}