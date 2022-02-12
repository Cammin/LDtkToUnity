namespace LDtkUnity
{
    //used as a substitute for LevelFieldType. todo check if that type is still around after 1.0 releases
    //FBool, FColor, FEntityRef, FEnum, FFloat, FInt, FPath, FPoint, FString, FText, FTile
    
    /// <summary>
    /// Used in conjunction with some functionality in the <see cref="LDtkFields"/> component.
    /// </summary>
    internal enum LDtkFieldType
    {
        None,
        FInt,
        FFloat,
        FBool,
        FString,
        FText,
        FPath,
        FColor,
        FEnum,
        FPoint,
        FEntityRef,
        FTile
    }
}