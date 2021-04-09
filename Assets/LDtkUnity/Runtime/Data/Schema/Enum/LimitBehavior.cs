using System.ComponentModel;

namespace LDtkUnity
{
    /// <summary>
    /// Possible values: `DiscardOldOnes`, `PreventAdding`, `MoveLastOne`
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum LimitBehavior { DiscardOldOnes, MoveLastOne, PreventAdding };
}