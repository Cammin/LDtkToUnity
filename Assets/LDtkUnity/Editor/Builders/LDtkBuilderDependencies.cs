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
    public class LDtkBuilderDependencies
    {
        private readonly HashSet<string> _dependencies = new HashSet<string>();
        private readonly AssetImportContext _ctx;

        public LDtkBuilderDependencies(AssetImportContext ctx)
        {
            _ctx = ctx;
        }
        
        public HashSet<string> GetDependencies()
        {
            return _dependencies;
        }

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
            return AddDependency(path);
        }
        
        public bool AddDependency(string path)
        {
            bool add = _dependencies.Add(path);
            if (!add)
            {
                return false;
            }

            string ctxAssetPath = _ctx.assetPath;
            

            TestLogDependencySet("DependsOnArtifact", ctxAssetPath, path);
            
            _ctx.DependsOnArtifact(path);
            return true;
        }

        public static void TestLogDependencySet(string functionName, string importerPath, string dependencyPath)
        {
            //used for testing
            Debug.Log($"LDtk: {functionName} <color=yellow>{Path.GetFileNameWithoutExtension(importerPath)}</color>:<color=navy>{Path.GetFileName(dependencyPath)}</color>");
        }
    }
}