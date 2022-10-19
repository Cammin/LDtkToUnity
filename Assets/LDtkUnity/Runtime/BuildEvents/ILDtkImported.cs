namespace LDtkUnity
{
    /// <summary>
    /// Base interface for the import interfaces. Don't inherit from this directly.
    /// </summary>
    public interface ILDtkImported
    {
        /// <summary>
        /// Override the order in which all of the import events are processed. Smaller priorities will be run first. This is also ordered alongside the postprocessors via LDtkPostprocessor.GetPostprocessOrder.
        /// </summary>
        /// <returns>
        /// The order value. Set as 0 if unsure.
        /// </returns>
#if UNITY_2021_2_OR_NEWER //doing this to add this default interface implementation while also not breaking code for users above 2021.2
        int GetPostprocessOrder() { return 0; }
#else
        int GetPostprocessOrder();
#endif
    }
}