namespace LDtkUnity
{
    /// <summary>
    /// An enum describing how the the Entity tile is rendered inside the Entity bounds. Possible
    /// values: `Cover`, `FitInside`, `Repeat`, `Stretch`, `FullSizeCropped`,
    /// `FullSizeUncropped`, `NineSlice`
    /// </summary>
    public enum TileRenderMode { Cover, FitInside, FullSizeCropped, FullSizeUncropped, NineSlice, Repeat, Stretch };
}