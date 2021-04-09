using System.ComponentModel;

namespace LDtkUnity
{
    /// <summary>
    /// Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`,
    /// `AutoLayer`
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum TypeEnum { AutoLayer, Entities, IntGrid, Tiles };
}