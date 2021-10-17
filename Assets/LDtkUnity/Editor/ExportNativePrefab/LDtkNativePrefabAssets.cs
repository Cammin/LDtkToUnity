using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// This clones assets so that the exported prefab has it's independent assets set up. once generated, then it can be used to replace the assets in the prefab factory
    /// </summary>
    public class LDtkNativePrefabAssets
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LDtkArtifactAssets _assets;
        private readonly string _path;
        
        private List<Sprite> _artTileSprites;
        private List<Tile> _artTiles;
        private List<Tile> _intGridTiles;
        private List<Sprite> _backgroundArtifacts;

        public List<Sprite> ArtTileSprites => _artTileSprites.ToList();
        public List<Tile> ArtTiles => _artTiles.ToList();
        public List<Tile> IntGridTiles => _intGridTiles.ToList();
        public List<Sprite> BackgroundArtifacts => _backgroundArtifacts.ToList();
        
        public LDtkNativePrefabAssets(LDtkProjectImporter importer, LDtkArtifactAssets assets, string path)
        {
            _importer = importer;
            _path = path;
            _assets = assets;
        }

        public void GenerateAssets()
        {
            if (_importer == null)
            {
                Debug.LogError("Null Importer");
                return;
            }
            
            if (_assets == null)
            {
                Debug.LogError("Null ArtifactAssets");
                return;
            }
            
            _artTileSprites = CloneArtifacts(_assets.SpriteArtifacts, "Sprites");
            _artTiles = CloneArtifacts(_assets.TileArtifacts, "ArtTiles").Cast<Tile>().ToList();
            _intGridTiles = CloneArtifacts(_importer.GetIntGridTiles().ToList(), "IntGridValues").Cast<Tile>().ToList();
            _backgroundArtifacts = CloneArtifacts(_assets.BackgroundArtifacts, "Backgrounds");
            
            
            //give each new native art tile a matching cloned sprite to reference
            foreach (Tile artTile in _artTiles)
            {
                string nameMatch = artTile.name;
                Sprite artTileSprite = _artTileSprites.Find(sprite => sprite.name == nameMatch);
                artTile.sprite = artTileSprite;
            }
            
            //now that this is done, we can make the prefab factory replace the old ones with these newly created prefabs
        }

        private List<T> CloneArtifacts<T>(List<T> artifacts, string extraPath) where T : Object
        {
            if (artifacts.IsNullOrEmpty())
            {
                return null;
            }
            
            string parentPath = $"{_path}/{extraPath}";
            LDtkPathUtility.TryCreateDirectory(parentPath);

            List<T> list = new List<T>();
            foreach (T artifact in artifacts)
            {
                string destinationPath = $"{parentPath}/{artifact.name}.asset";

                //Debug.Log($"Copy asset\n{artifact.name}\nto\n{destinationPath}");

                Object clone = CreateClone(artifact);
                clone.name = artifact.name;

                T loadedAsset = AssetDatabase.LoadAssetAtPath<T>(destinationPath);
                
                if (loadedAsset)
                {
                    EditorUtility.CopySerializedIfDifferent(clone, loadedAsset);
                    list.Add(loadedAsset);
                }
                else
                {
                    AssetDatabase.CreateAsset(clone, destinationPath);
                    list.Add((T)clone);
                }
            }

            return list;
        }

        //if it's an LDtk asset, then turn it into a native asset, otherwise if it's already native, then instantiate
        private static Object CreateClone<T>(T artifact) where T : Object
        {
            if (typeof(TileBase).IsAssignableFrom(typeof(T)))
            {
                return CreateNativeTile(artifact);
            }
            
            T clone = Object.Instantiate(artifact);
            clone.name = artifact.name;
            return clone;
        }

        private static Tile CreateNativeTile<T>(T artifact) where T : Object
        {
            TileBase tile = artifact as TileBase;
            if (tile == null)
            {
                Debug.LogError("Tile not casted");
                return null;
            }

            TileData tileData = default;
            tile.GetTileData(default, null, ref tileData);
            
            Tile nativeTile = ScriptableObject.CreateInstance<Tile>();
            nativeTile.name = artifact.name;
            nativeTile.sprite = tileData.sprite;
            nativeTile.color = tileData.color;
            nativeTile.transform = tileData.transform;
            nativeTile.gameObject = tileData.gameObject;
            nativeTile.flags = tileData.flags;
            nativeTile.colliderType = tileData.colliderType;
            return nativeTile;
        }
    }
}