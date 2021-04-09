using System.ComponentModel;

namespace LDtkUnity
{
    /// <summary>
    /// Possible values: `Rectangle`, `Ellipse`, `Tile`, `Cross`
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum RenderMode { Cross, Ellipse, Rectangle, Tile };
}