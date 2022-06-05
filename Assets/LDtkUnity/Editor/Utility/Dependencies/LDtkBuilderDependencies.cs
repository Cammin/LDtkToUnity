using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    
    /// <summary>
    /// The master class for managed dependencies.
    /// We get out dependencies from 2 sources: the GatherDependencies, and the actual import.
    /// </summary>
    public static class LDtkBuilderDependencies
    {
        /*private readonly HashSet<string> _dependencies = new HashSet<string>();
        private readonly AssetImportContext _ctx;

        public LDtkBuilderDependencies(AssetImportContext ctx)
        {
            _ctx = ctx;
        }
        
        public HashSet<string> GetDependencies()
        {
            return _dependencies;
        }

        /#2#/this would only track dependencies that are brought on from the import process. not the serialized objects. So, this would be the level backgrounds and tilesets
        public bool AddDependency(Object obj)
        {
            //only depend on source asset if we actually intend to read the file. otherwise, always use artifact so that import orders are working properly
            if (obj == null)
            {
                LDtkDebug.LogError($"LDtk: Adding dependency was null");
                return false;
            }

            if (LDtkResourcesLoader.IsDefaultAsset(obj))
            {
                return false;
            }
            
            string path = AssetDatabase.GetAssetPath(obj);
            
            bool add = _dependencies.Add(path);
            if (!add)
            {
                return false;
            }

            string ctxAssetPath = _ctx.assetPath;
            

            TestLogDependencySet("DependsOnArtifact", ctxAssetPath, path);
            
            //todo maybe this never needs to be called. it could all be within the "GatherDependencies" instead so that we don't depend on this API.
            //We will need it for 
#if UNITY_2020_1_OR_NEWER
            _ctx.DependsOnArtifact(path);
#else
            _ctx.DependsOnSourceAsset(path);
#endif
            return true;
        }#2#*/

        
    }
}