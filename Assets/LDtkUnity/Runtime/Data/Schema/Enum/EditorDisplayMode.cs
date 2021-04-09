using System.ComponentModel;

namespace LDtkUnity
{
    /// <summary>
    /// Possible values: `Hidden`, `ValueOnly`, `NameAndValue`, `EntityTile`, `PointStar`,
    /// `PointPath`, `RadiusPx`, `RadiusGrid`
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum EditorDisplayMode { EntityTile, Hidden, NameAndValue, PointPath, PointStar, RadiusGrid, RadiusPx, ValueOnly };
}