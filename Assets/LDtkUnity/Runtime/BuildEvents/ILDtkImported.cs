namespace LDtkUnity
{
    /// <summary>
    /// Base interface for the import interfaces. Don't inherit from this directly.
    /// </summary>
    public interface ILDtkImported
    {
        /// <summary>
        /// Override the order in which all of the import events are processed. Smaller priorities will be run first.
        /// </summary>
        /// <returns>
        /// The order value. Set as 0 if unsure.
        /// </returns>
        int GetPostprocessOrder();
    }
}