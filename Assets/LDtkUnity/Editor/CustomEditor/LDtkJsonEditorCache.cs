using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    //this is for the editor scripts, so that selecting the importers subsequent times is quicker.
    public class LDtkJsonEditorCache
    {
        private class Cache
        {
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
                for (i=0;i < arrInput.Length -1; i++)
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
            if (!GlobalCache.ContainsKey(_assetPath))
            {
                GlobalCache.Add(_assetPath, null);
            }
            
            byte[] newHash = GetFileHash();
            
            //if the asset is null or check if the new hash is different from the last one, to update the json info for the editor
            if (ShouldDeserialize(newHash))
            {
                LdtkJson fromJson = null;
                try
                {
                    //Debug.Log("FROM_JSON");
                    fromJson = importer.JsonFile.FromJson; //todo this is triggering for the ui before the import actually finishes, which is not making this update at the right time.
                }
                catch
                {
                    //Debug.LogError($"LDtk: Issue deserializing json: {e}");
                }

                GlobalCache[_assetPath] = new Cache()
                {
                    Hash = newHash,
                    Json = fromJson
                };
            }
            else
            {
                //Debug.Log("cached_JSON");
            }

            Json = GlobalCache[_assetPath].Json;
        }

        public bool ShouldReconstruct()
        {
            byte[] newHash = GetFileHash();
            return ShouldDeserialize(newHash);
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
            using FileStream stream = new FileStream(_assetPath, FileMode.Open, FileAccess.Read);

            Profiler.BeginSample("ComputeHash");
            byte[] hash = sha1.ComputeHash(stream);
            Profiler.EndSample();
            
            return hash;
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