using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    //this is for the editor scripts, so that selecting the importers subsequent times is quicker.
    //We also want to store it in the editor, because storing in the object as a serialized field affects source control. 
    //don't get confused, domain reload is different from a json file reimport. A json reimport does not cause a domain reload, so these values can indeed survive the domain reload.  
    internal class LDtkJsonEditorCache
    {
        private class Cache
        {
            public bool ShouldReconstruct;
            public LdtkJson Json;
            public byte[] Hash;

            public override string ToString()
            {
                return ByteArrayToString(Hash);
            }

            private static string ByteArrayToString(byte[] arrInput)
            {
                int i;
                StringBuilder sOutput = new StringBuilder(arrInput.Length);
                for (i = 0; i < arrInput.Length -1; i++)
                {
                    sOutput.Append(arrInput[i].ToString("X2"));
                }
                return sOutput.ToString();
            }
        }
        
        private static readonly Dictionary<string, Cache> GlobalCache = new Dictionary<string, Cache>();
        public readonly LdtkJson Json;
        private readonly string _assetPath;
        
        public LDtkJsonEditorCache(LDtkProjectImporter importer)
        {
            _assetPath = importer.assetPath;
            
            TryCreateKey(_assetPath);

            byte[] newHash = GetFileHash();
            
            //if the asset is null or check if the new hash is different from the last one, to update the json info for the editor. or if enforced
            if (ShouldDeserialize(newHash))
            {
                LdtkJson fromJson = null;
                try
                {
                    fromJson = importer.JsonFile.FromJson;
                }
                catch
                {
                    // ignored
                }

                GlobalCache[_assetPath] = new Cache()
                {
                    Hash = newHash,
                    Json = fromJson,
                    ShouldReconstruct = false
                };
            }

            if (GlobalCache[_assetPath] == null)
            {
                Debug.LogError("LDtk: A cached editor value is null, this should never be expected");
                return;
            }
            
            GlobalCache[_assetPath].ShouldReconstruct = false;
            Json = GlobalCache[_assetPath].Json;
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
                Debug.LogError("LDtk: bug with cache, should never realistically happen");
                return false;
            }
            
            return GlobalCache[_assetPath].ShouldReconstruct;
        }
        
        private bool ShouldDeserialize(byte[] newHash)
        {
            if (!GlobalCache.ContainsKey(_assetPath))
            {
                Debug.LogError("bug");
                return true;
            }

            Cache cache = GlobalCache[_assetPath];
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