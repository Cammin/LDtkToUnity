namespace LDtkUnity
{
    /// <summary>
    /// Applicable for both entity prefabs and the level prefab. <br />
    /// Use this interface on entity/level components to access the field instances of the entity/level.
    /// </summary>
    public interface ILDtkImportedFields : ILDtkImported
    {
        /// <summary>
        /// Triggers on an all entity/level prefab components that implements this interface during the import process.
        /// </summary>
        /// <param name="fields">
        /// The fields component.
        /// </param>
        void OnLDtkImportFields(LDtkFields fields);
    }
}