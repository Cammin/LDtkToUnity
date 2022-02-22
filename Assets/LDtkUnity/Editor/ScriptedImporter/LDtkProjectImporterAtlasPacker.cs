using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal class LDtkProjectImporterAtlasPacker
    {
        private readonly SpriteAtlas _atlas;
        private readonly string _assetPath;
        
        private static readonly Dictionary<SpriteAtlas, string> Atlases = new Dictionary<SpriteAtlas, string>();
        private static bool _hasPacked;

        public LDtkProjectImporterAtlasPacker(SpriteAtlas atlas, string assetPath)
        {
            _atlas = atlas;
            _assetPath = assetPath;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatus()
        {
            _hasPacked = false;
        }

        public void TryPack()
        {
            if (Atlases.ContainsKey(_atlas))
            {
                Debug.LogWarning($"LDtk: Tried statically adding a sprite atlas more than once for \"{_assetPath}\"");
                return;
            }
            
            Atlases.Add(_atlas, _assetPath);
            EditorApplication.delayCall += TryPackAction;
        }

        private static void TryPackAction()
        {
            if (_hasPacked)
            {
                return;
            }
            _hasPacked = true;
            PackAction();
        }

        private static void PackAction()
        {
            foreach (KeyValuePair<SpriteAtlas, string> pair in Atlases)
            {
                SetupSpriteAtlas(pair.Key, pair.Value);
            }

            SpriteAtlas[] atlases = Atlases.Keys.ToArray();
            SpriteAtlasUtility.PackAtlases(atlases, EditorUserBuildSettings.activeBuildTarget);

            EditorApplication.delayCall += Reset;
        }
        
        private static void SetupSpriteAtlas(SpriteAtlas atlas, string assetPath)
        {
            atlas.RemoveAll();

            Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            Object[] newSprites = subAssets.Where(p => p is Sprite).ToArray();

            //add sorted sprites
            atlas.Add(newSprites);
        }
        
        private static void Reset()
        {
            _hasPacked = false;
            Atlases.Clear();
        }

        /*private static void ResetAndSave()
        {
            try
            {
                SaveAtlases();
            }
            finally
            {
                _hasPacked = false;
                Atlases.Clear();
            }
        }*/


        private static void SaveAtlases()
        {
            foreach (SpriteAtlas atlas in Atlases.Keys)
            {
                EditorUtility.SetDirty(atlas); //todo this may not be needed?
            }
            
            foreach (SpriteAtlas atlas in Atlases.Keys)
            {

#if UNITY_2020_3_OR_NEWER
                //Debug.Log($"Saving atlas: \"{atlas.name}\"");
                AssetDatabase.SaveAssetIfDirty(atlas);
#else
                AssetDatabase.SaveAssets();
#endif
            }
        }

        private class AtlasSaveChecker : UnityEditor.AssetModificationProcessor
        {
            private static string[] OnWillSaveAssets(string[] paths)
            {
                if (!_hasPacked || Atlases.IsNullOrEmpty())
                {
                    return paths;
                }

                //string[] atlasesToSave = Atlases.Select(AssetDatabase.GetAssetPath).ToArray();
                //Debug.Log($"Saving atlases:\"{string.Join("\",\n\"", atlasesToSave)}\"");
                //return atlasesToSave;

                return null;

            }
        }

        private static bool AreEqualSpriteArrays(Object[] a1, Object[] a2)
        {
            if (a1 == null && a2 == null)
            {
                return true;
            }
            
            if (a1 == null || a2 == null)
            {
                return false;
            }

            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++)
            {
                Object i1 = a1[i];
                Object i2 = a2[i];

                if (!AreEqualSpriteElements(i1, i2))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AreEqualSpriteElements(Object i1, Object i2)
        {
            if (i1 == null && i2 == null)
            {
                return true;
            }

            if (i1 == null || i2 == null)
            {
                return false;
            }
            
            return i1.name == i2.name;
        }

        /// <summary>
        /// made this function for the use that it unites the associated function calls that would revolve around using a sprite atlas
        /// </summary>
        public static bool UsesSpriteAtlas(LdtkJson json)
        {
            return !json.Defs.Tilesets.IsNullOrEmpty();
        }
    }
}