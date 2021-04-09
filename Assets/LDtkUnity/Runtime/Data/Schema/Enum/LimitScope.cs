using System.ComponentModel;

namespace LDtkUnity
{
    /// <summary>
    /// If TRUE, the maxCount is a "per world" limit, if FALSE, it's a "per level". Possible
    /// values: `PerLayer`, `PerLevel`, `PerWorld`
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum LimitScope { PerLayer, PerLevel, PerWorld };
}