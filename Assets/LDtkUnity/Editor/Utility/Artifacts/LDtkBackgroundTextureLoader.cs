using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// relativePath, Texture<br/>
    /// This is used so we don't load textures more than once when creating import artifacts.<br/>
    /// It's structured like this with relative paths as keys so that even if a texture is used as both a background and tile set, then it's still only loaded once.
    /// There is no responsibility to track sprite slices in here. just loading+holding onto textures.
    /// </summary>
    internal sealed class LDtkBackgroundTextureLoader
    {
        private readonly string _assetPath;

        private readonly Dictionary<string, Texture2D> _dict = new Dictionary<string, Texture2D>();
        private readonly HashSet<string> _attemptedFailures = new HashSet<string>();

        public LDtkBackgroundTextureLoader(string assetPath)
        {
            _assetPath = assetPath;
        }

        public IEnumerable<Texture2D> Textures => _dict.Values;

        public void CacheTextures(LdtkJson json)
        {
            //acquire level backgrounds
            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels) //get textures references from level backgrounds. gather them all
                {
                    Profiler.BeginSample(level.Identifier);
                    TryAdd(level, level.BgRelPath, LoadLevelBackground);
                    Profiler.EndSample();
                }
            }
        }

        public Texture2D GetTexture(string textureRelPath)
        {
            if (string.IsNullOrEmpty(textureRelPath))
            {
                return null;
            }

            if (!_dict.ContainsKey(textureRelPath))
            {
                //LDtkDebug.LogError($"Failed getting texture from {_assetPath}: {relPath}, the dictionary didn't contain the key when trying to get it.");
                return null;
            }

            Texture2D tex = _dict[textureRelPath];
            if (tex == null)
            {
                //LDtkDebug.LogError($"Failed getting texture for {_assetPath}: {relPath}, asset was null");
                return null;
            }

            return tex;
        }


        private delegate Texture2D ExternalLoadMethod<in T>(T data);

        private void TryAdd<T>(T data, string relPath, ExternalLoadMethod<T> textureLoadAction)
        {
            if (string.IsNullOrEmpty(relPath))
            {
                return;
            }

            if (_dict.ContainsKey(relPath))
            {
                return;
            }

            if (_attemptedFailures.Contains(relPath))
            {
                //LDtkDebug.LogError($"Failed loading texture from {_assetPath}: {relPath}");
                return;
            }

            Texture2D tex = textureLoadAction.Invoke(data);
            if (tex != null)
            {
                LogPotentialTextureProblems(tex);
                _dict.Add(relPath, tex);
                return;
            }

            _attemptedFailures.Add(relPath);
            //LDtkDebug.LogError($"Failed loading texture from {_assetPath}: {relPath}");
            return;
        }

        /*private Texture2D LoadTilesetTex(TilesetDefinition def)
        {
            LDtkRelativeGetterTilesetTexture getter = new LDtkRelativeGetterTilesetTexture();
            return getter.GetRelativeAsset(def, _assetPath);
        }*/

        private Texture2D LoadLevelBackground(Level def)
        {
            LDtkRelativeGetterLevelBackground getter = new LDtkRelativeGetterLevelBackground();
            return getter.GetRelativeAsset(def, _assetPath);
        }

        private void LogPotentialTextureProblems(Texture2D tex)
        {
            string texPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(texPath);
            if (importer.textureType == TextureImporterType.Sprite)
            {
                return;
            }

            LDtkDebug.LogWarning($"Referenced texture type is not Sprite. It is recommended to use Sprite mode for texture: \"{tex.name}\"", tex);

            if (importer.npotScale != TextureImporterNPOTScale.None)
            {
                LDtkDebug.LogError($"Referenced texture Non-Power of Two is not None, which will corrupt the tileset art! Fix this for: \"{_assetPath}\"", tex);
            }
        }
    }
}