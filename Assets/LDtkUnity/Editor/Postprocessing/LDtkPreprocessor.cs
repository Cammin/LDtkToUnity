namespace LDtkUnity.Editor
{
    /// <summary>
    /// LDtkPreprocessor lets you hook into the import pipeline to modify any json data before the GameObject structure is created.<br/>
    /// Do note that if any json data is modified, it can risk breaking the import process, so modify with care.
    /// </summary>
    public abstract class LDtkPreprocessor
    {
        internal const string METHOD_PROJECT = nameof(OnPreprocessProject);
        internal const string METHOD_LEVEL = nameof(OnPreprocessLevel);
        internal const string METHOD_TILESETDEF = nameof(OnPreprocessTilesetDefinition);

        /// <summary>
        /// Use to perform operations before the project hierarchy is created.<br/>
        /// This is only called for project files, not separate level files.
        /// </summary>
        /// <param name="projectJson">
        /// The project json.
        /// </param>
        /// <param name="projectName">
        /// Name of the project file.
        /// </param>
        /// <param name="assetPath">
        /// Path to the project file
        /// </param>
        protected virtual void OnPreprocessProject(LdtkJson projectJson, string projectName, string assetPath) { }
        
        /// <summary>
        /// Use to perform operations before the level hierarchy is created.<br/>
        /// This is only called for separate level files, not project files.
        /// </summary>
        /// <param name="level">
        /// The level json.
        /// </param>
        /// <param name="projectJson">
        /// The project data of this level. 
        /// </param>
        /// <param name="projectName">
        /// Name of the project file.
        /// </param>
        /// <param name="assetPath">
        /// Path to the level file
        /// </param>
        protected virtual void OnPreprocessLevel(Level level, LdtkJson projectJson, string projectName, string assetPath) { }

        /// <summary>
        /// Use to perform operations before the tileset definition is created.<br/>
        /// </summary>
        /// <param name="tilesetDefinition">
        /// The tileset definition json.
        /// </param>
        /// <param name="assetPath">
        /// Path to the tileset definition file
        /// </param>
        protected virtual void OnPreprocessTilesetDefinition(LDtkTilesetDefinitionWrapper tilesetDefinition, string assetPath) { }
        
        /// <summary>
        /// Override the order in which preprocessors are processed.
        /// All levels will only process after all projects. This is due to the way that Unity's import pipeline is structured.
        /// </summary>
        /// <returns>
        /// The order value. Default value is 1.
        /// </returns>
        public virtual int GetPreprocessOrder() => 1;
    }
}