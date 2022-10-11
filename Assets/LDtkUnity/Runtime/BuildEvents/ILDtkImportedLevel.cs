namespace LDtkUnity
{
    /// <summary>
    /// Applicable for the level prefab only. <br />
    /// Use this interface on a level's prefab components to access the level's data during the import process.
    /// </summary>
    public interface ILDtkImportedLevel : ILDtkImported
    {
        /// <summary>
        /// Triggers on an all level components that implements this interface during the import process.
        /// </summary>
        /// <param name="level">
        /// The level. 
        /// </param>
        void OnLDtkImportLevel(Level level);
    }
}