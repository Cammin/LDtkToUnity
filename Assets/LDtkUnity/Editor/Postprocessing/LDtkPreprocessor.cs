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
        
        /// <summary>
        /// Use to perform operations before the project hierarchy is created.<br/>
        /// This would primarily be used with a project that does **not** use separate level files. 
        /// </summary>
        /// <param name="projectJson">
        /// The project json.
        /// </param>
        /// <param name="projectName">
        /// Name of the project file.
        /// </param>
        protected virtual void OnPreprocessProject(LdtkJson projectJson, string projectName) { }
        
        /// <summary>
        /// Use to perform operations before the level hierarchy is created.<br/>
        /// This runs for both levels in a project and separate levels.<br/>
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
        protected virtual void OnPreprocessLevel(Level level, LdtkJson projectJson, string projectName) { }

        /// <summary>
        /// Override the order in which preprocessors are processed.
        /// This will only apply control over ordering between projects/levels if not using separate levels because Unity's import priority for ScriptedImporters can only be determined from an attribute.
        /// If using separate levels, the levels will always be processed after the project. 
        /// </summary>
        /// <returns>
        /// The order value. Default value is 1.
        /// </returns>
        public virtual int GetPreprocessOrder() => 1;
    }
}