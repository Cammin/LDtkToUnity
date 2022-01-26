using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace LDtkUnity.Editor
{
    internal class LDtkProjectImporterAtlasPacker
    {
        private readonly SpriteAtlas _atlas;
        private readonly string _assetPath;

        public LDtkProjectImporterAtlasPacker(SpriteAtlas atlas, string assetPath)
        {
            _atlas = atlas;
            _assetPath = assetPath;
        }

        public void TryPack()
        {
            Object[] prevPackables = _atlas.GetPackables();
            
            //there is a unity issue where we try to pack in 2019.3, but a lost reference to a sprite trying to get packed, results in an editor crash. so make sure there is nothing null in the previous packables
#if !UNITY_2019_4_OR_NEWER
            if (!prevPackables.IsNullOrEmpty() && prevPackables.Any(p => p == null))
            {
                Debug.LogWarning($"LDtk: Did not pack sprite atlas \"{_atlas.name}\"; A Unity 2019.3 bug could have crashed the editor when packing the atlas. Try emptying the atlas of any references and reimport the LDtk project.", _atlas);
                return;
            }
#endif

            //if only 2019.4, there's an issue when reimporting all.
#if UNITY_2019_4_OR_NEWER && !UNITY_2020_1_OR_NEWER
            Debug.LogWarning($"LDtk: Did not pack sprite atlas \"{_atlas.name}\"; A Unity 2019.4 bug could have crashed/locked the editor when packing the atlas. You will need to manually pack the atlas.", _atlas);
#else
            Pack(prevPackables);
#endif
        }

        private void Pack(Object[] prevPackables)
        {
            Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(_assetPath);

            //load artifacts. the local reference is lost after the delay call
            LDtkArtifactAssets artifacts = (LDtkArtifactAssets)subAssets.FirstOrDefault(t => t is LDtkArtifactAssets);
            if (artifacts == null)
            {
                Debug.LogError("LDtk: Import issue, was not able to load the artifact asset. Not packing to atlas");
                return;
            }

            //don't pack backgrounds
            Object[] newSprites = subAssets.Distinct()
                .Where(p => p != null && p is Sprite sprite && !artifacts.ContainsBackground(sprite))
                .OrderBy(p => p.name).ToArray();

            //compare with existing sprites to make sure if it's even necessary to pack. also may help keep git cleaner
            Object[] prevSprites = prevPackables.Where(p => p != null && p is Sprite).ToArray();

            //only remove and re-add to the sprite atlas if any sprite assets are different from the last one. regardless, pack in case the texture was modified
            if (!AreEqualSpriteArrays(prevSprites, newSprites))
            {
                //remove existing
                _atlas.Remove(prevPackables);

                //add sorted sprites
                _atlas.Add(newSprites);
            }

            //todo check if the texture was changed, or if there was a reimport as a result of a texture, in order to detect if we should spend the time to pack textures if it's really necessary.

            //automatically pack it
            SpriteAtlasUtility.PackAtlases(new[] { _atlas }, EditorUserBuildSettings.activeBuildTarget);
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