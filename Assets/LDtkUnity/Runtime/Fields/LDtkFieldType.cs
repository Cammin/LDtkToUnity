namespace LDtkUnity
{
    /// <summary>
    /// Used in conjunction with some functionality in the <see cref="LDtkFields"/> component.
    /// </summary>
    internal enum LDtkFieldType
    {
        None,
        Int,
        Float,
        Bool,
        String,
        Multiline,
        FilePath,
        Color,
        Enum,
        Point,
        EntityRef,
        Tile
    }
}