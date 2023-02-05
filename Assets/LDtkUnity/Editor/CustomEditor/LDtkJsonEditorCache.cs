using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    //this is for the editor scripts, so that selecting the importers subsequent times is quicker.
    //We also want to store it in the editor, because storing in the object as a serialized field affects source control. 
    //don't get confused, domain reload is different from a json file reimport. A json reimport does not cause a domain reload, so these values can indeed survive the reimport.  
    internal sealed class LDtkJsonEditorCache
    {
        private static readonly Dictionary<string, LDtkJsonEditorCacheInstance> GlobalCache = new Dictionary<string, LDtkJsonEditorCacheInstance>();
        public readonly LdtkJson Json;
        private readonly string _assetPath;
        
        public LDtkJsonEditorCache(LDtkProjectImporter importer)
        {
            _assetPath = importer.assetPath;
            
            TryCreateKey(_assetPath);
            TryReconstruct(importer);
            
            if (GlobalCache[_assetPath] == null)
            {
                LDtkDebug.LogError("A cached editor value is null, this should never be expected");
                return;
            }
            
            GlobalCache[_assetPath].ShouldReconstruct = false;
            Json = GlobalCache[_assetPath].Json;
        }

        private void TryReconstruct(LDtkProjectImporter importer)
        {
            //if the asset is null or check if the new hash is different from the last one, to update the json info for the editor. or if enforced
            byte[] newHash;
            
            if (ShouldForceReconstruct())
            {
                newHash = GetFileHash();
                Reconstruct(importer, newHash);
            }

            newHash = GetFileHash();
            if (ShouldDeserialize(newHash))
            {
                Reconstruct(importer, newHash);
            }
        }

        private void Reconstruct(LDtkProjectImporter importer, byte[] newHash)
        {
            LdtkJson fromJson;
            try
            {
                LDtkProfiler.BeginSample($"JsonEditorCache/{Path.GetFileName(importer.assetPath)}");
                fromJson = importer.FromJson<LdtkJson>(); //todo benchmark how long each one takes. a test run-through
            }
            finally
            {
                LDtkProfiler.EndSample();
            }

            GlobalCache[_assetPath] = new LDtkJsonEditorCacheInstance()
            {
                Hash = newHash,
                Json = fromJson,
                ShouldReconstruct = false
            };
        }

        public static void ForceRefreshJson(string assetPath)
        {
            //don't force a refresh if we haven't setup the key value yet
            if (!GlobalCache.ContainsKey(assetPath))
            {
                return;
            }
            GlobalCache[assetPath].ShouldReconstruct = true;
        }

        private static void TryCreateKey(string assetPath)
        {
            if (!GlobalCache.ContainsKey(assetPath))
            {
                GlobalCache.Add(assetPath, null);
            }
        }

        public bool ShouldForceReconstruct()
        {
            if (!GlobalCache.ContainsKey(_assetPath))
            {
                LDtkDebug.LogError("Bug with cache; no key. this should never happen");
                return false;
            }

            LDtkJsonEditorCacheInstance cache = GlobalCache[_assetPath];
            if (cache == null)
            {
                return false;
            }

            return cache.ShouldReconstruct;
        }
        
        private bool ShouldDeserialize(byte[] newHash)
        {
            if (!GlobalCache.ContainsKey(_assetPath))
            {
                LDtkDebug.LogError("bug");
                return true;
            }

            LDtkJsonEditorCacheInstance cache = GlobalCache[_assetPath];
            if (cache == null)
            {
                //Debug.Log($"null, new json");
                return true;
            }

            byte[] prevHash = cache.Hash;
            bool areDifferent = !CompareHash(prevHash, newHash);
            
            //Debug.Log($"Compare hashes: {(areDifferent ? "DIFFERENT" : "SAME")} for {_assetPath}\n{string.Join("", prevHash)}\n{string.Join("", newHash)}");
            
            return areDifferent;
        }
        
        private byte[] GetFileHash()
        {
            HashAlgorithm sha1 = HashAlgorithm.Create();
            
            FileStream stream = new FileStream(_assetPath, FileMode.Open, FileAccess.Read);
            using (stream)
            {
                Profiler.BeginSample("ComputeHash");
                byte[] hash = sha1.ComputeHash(stream);
                Profiler.EndSample();
                return hash;
            }
        }
        
        private bool CompareHash(byte[] lhs, byte[] rhs)
        {
            if (rhs.Length != lhs.Length)
            {
                return false;
            }
            
            bool isEqual = false;
            int i = 0;
            while (i < rhs.Length && (rhs[i] == lhs[i]))
            {
                i += 1;
            }
            
            if (i == rhs.Length)
            {
                isEqual = true;
            }

            return isEqual;
        }
    }
}