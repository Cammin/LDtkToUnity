using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkNativePrefabAssets
    {
        private readonly LDtkArtifactAssets _assets;
        private readonly string _path;

        public LDtkNativePrefabAssets(LDtkArtifactAssets assets, string path)
        {
            _path = path;
            _assets = assets;
        }

        public void GenerateAssets()
        {
            if (_assets == null)
            {
                Debug.LogError("Null ArtifactAssets");
                return;
            }
            
            CreateSprites(_assets.BackgroundArtifacts);
            CreateSprites(_assets.TileArtifacts);
            CreateSprites(_assets.SpriteArtifacts);
        }

        private void CreateSprites<T>(List<T> artifacts) where T : Object
        {
            foreach (T sprite in artifacts)
            {
                string assetPath = AssetDatabase.GetAssetPath(sprite);
                
                string path = $"{_path}/{sprite.name}.asset";

                Debug.Log($"Copy asset from {assetPath} to {path} TEST nothing happened");
                
                return;
                
                if (AssetDatabase.CopyAsset(assetPath, path))
                {
                    Debug.Log($"Copied asset to {path}");
                    return;
                }

                Debug.LogError($"Failed copying asset to {path}");

            }
        }
    }
}