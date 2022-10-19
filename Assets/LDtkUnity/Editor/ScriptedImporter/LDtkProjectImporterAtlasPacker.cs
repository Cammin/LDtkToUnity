using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectImporterAtlasPacker
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
                //Debug.LogWarning($"LDtk: Tried statically adding a sprite atlas more than once for \"{_assetPath}\"");
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
            ReassignAllAtlasSprites();
            PackAllAtlases();
            EditorApplication.delayCall += Reset;
        }

        private static void ReassignAllAtlasSprites()
        {
            foreach (KeyValuePair<SpriteAtlas, string> pair in Atlases)
            {
                ReassignSpriteAtlasSprites(pair.Key, pair.Value);
            }
        }

        private static void PackAllAtlases()
        {
            SpriteAtlas[] atlases = Atlases.Keys.ToArray();
            SpriteAtlasUtility.PackAtlases(atlases, EditorUserBuildSettings.activeBuildTarget);
        }

        private static void ReassignSpriteAtlasSprites(SpriteAtlas atlas, string assetPath)
        {
            atlas.RemoveAll();

            //It's going to load any sub assets that are a sprite. so make the packable sprites visible while the unpacked sprites remain invisible
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
        
        /// <summary>
        /// made this function for the use that it unites the associated function calls that would revolve around using a sprite atlas
        /// </summary>
        public static bool UsesSpriteAtlas(LdtkJson json)
        {
            return !json.Defs.Tilesets.IsNullOrEmpty();
        }
    }
}