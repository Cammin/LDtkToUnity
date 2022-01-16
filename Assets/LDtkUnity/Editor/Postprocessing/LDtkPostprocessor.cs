using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// LDtkPostprocessor lets you hook into the import pipeline and run scripts after importing LDtk projects.<br/>
    /// Inherit from this class to postprocess an import of a LDtk project.
    /// </summary>
    public abstract class LDtkPostprocessor
    {
        internal const string METHOD_PROJECT = nameof(OnPostprocessProject);
        internal const string METHOD_LEVEL = nameof(OnPostprocessLevel);
        
        /// <summary>
        /// Use to perform operations after a project is created.<br/>
        /// </summary>
        /// <param name="root">
        /// The root GameObject of the imported LDtk project.
        /// This GameObject has a <see cref="LDtkComponentProject"/> component to get the project's Json data.
        /// </param>
        protected virtual void OnPostprocessProject(GameObject root) { }
        
        /// <summary>
        /// Use to perform operations after a level is created. <br/>
        /// Primarily useful for imported separate level files, but can also work for levels in a project. 
        /// </summary>
        /// <param name="root">
        /// The root GameObject of the imported LDtk level. <br/>
        /// This GameObject has a <see cref="LDtkComponentLevel"/> component to get some useful level data.
        /// </param>
        /// <param name="projectJson">
        /// The Json data of the project this level is referenced by.
        /// </param>
        protected virtual void OnPostprocessLevel(GameObject root, LdtkJson projectJson) { }

        /// <summary>
        /// Override the order in which importers are processed. Smaller priorities will be imported first.
        /// </summary>
        /// <returns>
        /// The order value. Default value is 0.
        /// </returns>
        public virtual int GetPostprocessOrder() => 0;
    }
}