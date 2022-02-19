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
        private readonly Sprite[] _assetsToPack;
        
        private static readonly List<SpriteAtlas> Atlases = new List<SpriteAtlas>();
        private static bool _hasPacked;

        public LDtkProjectImporterAtlasPacker(SpriteAtlas atlas, Sprite[] spriteAssetsToPack)
        {
            _atlas = atlas;
            _assetsToPack = spriteAssetsToPack;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatus()
        {
            _hasPacked = false;
        }

        public void TryPack()
        {
            Pack();
            //there is a unity issue where we try to pack in 2019.3, but a lost reference to a sprite trying to get packed, results in an editor crash. so make sure there is nothing null in the previous packables
        }
        
        //dirty zombie code is for potential optimizing later
        private void Pack()
        {
            Object[] prevPackables = _atlas.GetPackables();
            Object[] newSprites = _assetsToPack.Cast<Object>().ToArray();


            Debug.Log($"prev:{prevPackables.Length}");
            Debug.Log($"new:{newSprites.Length}");

            //Debug.Log(newSprites.Length);
            
            string strings = string.Join("\n", newSprites.Select(p => p.name).ToArray());
            Debug.Log(strings);
            
            //Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(_assetPath);

            //load artifacts. the local reference is lost after the delay call
            //LDtkArtifactAssets artifacts = (LDtkArtifactAssets)_subAssets.FirstOrDefault(t => t is LDtkArtifactAssets);
            //if (artifacts == null)
            //{
                //Debug.LogError("LDtk: Import issue, was not able to load the artifact asset. Not packing to atlas");
                //return;
            //}

            //don't pack backgrounds
                //.Where(p => p != null && p is Sprite sprite && !artifacts.ContainsBackground(sprite))
                //.OrderBy(p => p.name).ToArray();

            //compare with existing sprites to make sure if it's even necessary to pack. also may help keep git cleaner
            //Object[] prevSprites = prevPackables.Where(p => p != null && p is Sprite).ToArray();

            //only remove and re-add to the sprite atlas if any sprite assets are different from the last one. regardless, pack in case the texture was modified
            //if (!AreEqualSpriteArrays(prevSprites, newSprites))
            {
                //remove existing
                _atlas.Remove(prevPackables);

                //add sorted sprites
                _atlas.Add(newSprites);
                //Debug.Log($"atlas \"{_atlas.name}\" add {string.Join(", ", newSprites.Select(p => p.name))}");
            }
            
            Object[] newPackables = _atlas.GetPackables();
            Debug.Log($"newPackables:{newPackables.Length}");

            //todo check if the texture was changed, or if there was a reimport as a result of a texture, in order to detect if we should spend the time to pack textures if it's really necessary.

            SaveAtlases();

            //automatically pack it
            DoPackAction();
        }
        
        private void DoPackAction()
        {
            Atlases.Add(_atlas);
            EditorApplication.delayCall += PackAction;
        }

        private static void PackAction()
        {
            if (_hasPacked)
            {
                return;
            }
            _hasPacked = true;

            SpriteAtlasUtility.PackAtlases(Atlases.ToArray(), EditorUserBuildSettings.activeBuildTarget);
            Debug.Log($"packed, Atlases {Atlases[0].name}");
            
            foreach (SpriteAtlas atlas in Atlases)
            {
                EditorUtility.SetDirty(atlas); //todo this may not be needed?
            }

            EditorApplication.delayCall += Reset;
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

        private static void Reset()
        {
            _hasPacked = false;
            Atlases.Clear();
        }

        private static void SaveAtlases()
        {
            foreach (SpriteAtlas atlas in Atlases)
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

                string[] atlasesToSave = Atlases.Select(AssetDatabase.GetAssetPath).ToArray();
                //Debug.Log($"Saving atlases:\"{string.Join("\",\n\"", atlasesToSave)}\"");
                return atlasesToSave;

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