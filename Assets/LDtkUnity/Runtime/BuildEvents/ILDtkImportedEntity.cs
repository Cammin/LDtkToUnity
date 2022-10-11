namespace LDtkUnity
{
    /// <summary>
    /// Applicable for entity prefabs only. <br />
    /// Use this interface on a entity's prefab components to access the entity instance's data during the import process.
    /// </summary>
    public interface ILDtkImportedEntity : ILDtkImported
    {
        /// <summary>
        /// Triggers on an all entity prefab components that implements this interface during the import process.
        /// </summary>
        /// <param name="entityInstance">
        /// The entity instance.
        /// </param>
        void OnLDtkImportEntity(EntityInstance entityInstance);
    }
}