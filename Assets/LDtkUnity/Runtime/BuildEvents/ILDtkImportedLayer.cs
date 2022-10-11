namespace LDtkUnity
{
    /// <summary>
    /// Applicable for entity prefabs only. <br />
    /// Use this interface on an entity's prefab components to access the layer of this entity instance during the import process.
    /// </summary>
    public interface ILDtkImportedLayer : ILDtkImported
    {
        /// <summary>
        /// Triggers on an all entity components that implements this interface during the import process.
        /// </summary>
        /// <param name="layerInstance">
        /// The layer instance that this entity is in.
        /// </param>
        void OnLDtkImportLayer(LayerInstance layerInstance);
    }
}