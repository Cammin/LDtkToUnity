using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

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

        internal AssetImportContext _importContext;

        /// <summary>
        /// The AssetImportContext of the current importing project file or level file.
        /// </summary>
        public AssetImportContext ImportContext => _importContext;
        
        /// <summary>
        /// Use to perform operations after a project is created.<br/>
        /// This would primarily be used with a project that does **not** use separate level files. 
        /// </summary>
        /// <param name="root">
        /// The root GameObject of the imported LDtk project.
        /// This GameObject has a <see cref="LDtkComponentProject"/> component to get the project's json data with GetComponent.
        /// </param>
        protected virtual void OnPostprocessProject(GameObject root) { }
        
        /// <summary>
        /// Use to perform operations after a level is created. <br/>
        /// This is called when importing a separate level file, or called on every level gameobject in a project that doesn't use separate level files.
        /// </summary>
        /// <param name="root">
        /// The root GameObject of the imported LDtk level. <br/>
        /// This GameObject has a <see cref="LDtkComponentLevel"/> component to get the level's json data with GetComponent.
        /// </param>
        /// <param name="projectJson">
        /// The Json data of the project this level is referenced by.
        /// </param>
        protected virtual void OnPostprocessLevel(GameObject root, LdtkJson projectJson) { }

        /// <summary>
        /// Override the order in which postprocessors and import interfaces are processed. This is also ordered alongside the import interfaces: <see cref="ILDtkImported.GetPostprocessOrder"/>
        /// </summary>
        /// <returns>
        /// The order value. Default value is 1.
        /// </returns>
        public virtual int GetPostprocessOrder() => 1;
    }
}