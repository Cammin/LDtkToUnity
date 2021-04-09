using System.ComponentModel;

namespace LDtkUnity
{
    /// <summary>
    /// Possible values: `Cover`, `FitInside`, `Repeat`, `Stretch`
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum TileRenderMode { Cover, FitInside, Repeat, Stretch };
}