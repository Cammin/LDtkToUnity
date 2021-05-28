namespace LDtkUnity
{
    /// <summary>
    /// Used in conjunction with some functionality in the <see cref="LDtkFields"/> component.
    /// </summary>
    public enum LDtkFieldType
    {
        /// <summary>
        /// No value
        /// </summary>
        None,
        
        /// <summary>
        /// Int value
        /// </summary>
        Int,
        
        /// <summary>
        /// Float value
        /// </summary>
        Float,
        
        /// <summary>
        /// Bool value
        /// </summary>
        Boolean,
        
        /// <summary>
        /// String value
        /// </summary>
        String,
        
        /// <summary>
        /// Multiline value
        /// </summary>
        Multiline,
        
        /// <summary>
        /// FilePath value
        /// </summary>
        FilePath,
        
        /// <summary>
        /// Color value
        /// </summary>
        Color,
        
        /// <summary>
        /// Enum value
        /// </summary>
        Enum,
        
        /// <summary>
        /// Point value
        /// </summary>
        Point,
    }
}