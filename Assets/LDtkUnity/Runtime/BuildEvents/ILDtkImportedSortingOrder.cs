namespace LDtkUnity
{
    /// <summary>
    /// Applicable for entity prefabs only. <br />
    /// Use this interface on an entity for setting a renderer's sorting order in the order of LDtk's layers.
    /// </summary>
    public interface ILDtkImportedSortingOrder
    {
        /// <summary>
        /// Triggers on an all entity components that implements this interface during the import process.
        /// </summary>
        /// <param name="sortingOrder">
        /// The sorting order at which to set on a renderer. The value is dictated by how many layers were already previously built.
        /// </param>
        void OnLDtkImportSortingOrder(int sortingOrder);
    }
}