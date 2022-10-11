namespace LDtkUnity
{
    /// <summary>
    /// Applicable for entity prefabs only. <br />
    /// This passes in an int that represents the sorting order to set. <br />
    /// The sorting order value is automatically determined by the layer generation. <br />
    /// Use this interface for setting a renderer's sorting order if applicable. ex. Renderer, SpriteRenderer, SortingGroup, etc.
    /// </summary>
    public interface ILDtkImportedSortingOrder : ILDtkImported
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