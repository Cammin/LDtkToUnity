using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        private readonly Sprite _oldSprite;
        private readonly Texture2D _newTexture;
        
        private List<Sprite> _artTileSprites = new List<Sprite>();
        private List<Tile> _artTiles = new List<Tile>();
        private List<Tile> _intGridTiles = new List<Tile>();
        private List<Sprite> _backgroundArtifacts = new List<Sprite>();
        
        public List<Tile> ArtTiles => _artTiles.ToList();
        public List<Tile> IntGridTiles => _intGridTiles.ToList();
        public List<Sprite> BackgroundArtifacts => _backgroundArtifacts.ToList();

        public LDtkNativePrefabAssets(LDtkProjectImporter importer, LDtkArtifactAssets assets, string path)
        {
            _importer = importer;
            _path = path;
            _assets = assets;
            _oldSprite = LDtkResourcesLoader.LoadDefaultTileSprite();
            _newTexture = Texture2D.whiteTexture;
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
            
            try
            {
                AssetDatabase.StartAssetEditing();
                MainAssetGeneration();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
            
            //now that this is done, we can make the prefab factory replace the old ones with these newly created prefabs
            AssetDatabase.Refresh();
        }

        private void MainAssetGeneration()
        {
            LDtkIntGridTile oldDefaultTile = LDtkResourcesLoader.LoadDefaultTile();
            Tile newDefaultTile = CreateNativeTile(oldDefaultTile);
            
            //export default texture
            Texture2D newDefaultTexture = CloneArtifacts(new[] { _newTexture }.ToList(), "/Sprites", _oldSprite.name + "Texture").First();
            
            //export the default sprite
            Sprite newDefaultSprite = Sprite.Create(newDefaultTexture, _oldSprite.rect, new Vector2(0.5f, 0.5f), _oldSprite.pixelsPerUnit, 1, SpriteMeshType.Tight, _oldSprite.border, true);
            newDefaultSprite = CloneArtifacts(new[] { newDefaultSprite }.ToList(), "/Sprites", _oldSprite.name).First();
            
            //clone art tile sprites
            List<Sprite> oldArtTileSprites = CloneArtifacts(_assets.SpriteArtifacts, "/Sprites");
            
            //clone art tiles
            _artTiles = CloneArtifacts(_assets.TileArtifacts, "/ArtTiles").Cast<Tile>().ToList();

            //clone int grid tiles
            List<TileBase> oldIntGridArtifacts = _importer.GetIntGridTiles().Where(p => p != null).Append(newDefaultTile).ToList();
            _intGridTiles = CloneArtifacts(oldIntGridArtifacts, "/IntGridValues").Cast<Tile>().ToList();

            //clone background sprites
            _backgroundArtifacts = CloneArtifacts(_assets.BackgroundArtifacts, "/Backgrounds");
            
            //give each new native art tile a matching cloned sprite to reference
            foreach (Tile artTile in _artTiles)
            {
                string nameMatch = artTile.name;
                Sprite oldArtTileSprite = oldArtTileSprites.Find(sprite => sprite.name == nameMatch);
                artTile.sprite = oldArtTileSprite;
            }
            
            //give the int grid tiles the clone sprite instead if they were using the default tile
            foreach (Tile intGridTile in _intGridTiles)
            {
                if (intGridTile.sprite == _oldSprite)
                {
                    intGridTile.sprite = newDefaultSprite;
                }
            } 
            
            //we've generated the default sprite which is used by multiple, simply add to the list since it's already created
            _backgroundArtifacts.Add(newDefaultSprite);
        }

        private List<T> CloneArtifacts<T>(List<T> artifacts, string extraPath, string assetName = null) where T : Object
        {
            if (artifacts.IsNullOrEmpty())
            {
                return new List<T>();
            }
            
            string parentPath = $"{_path}{extraPath}";
            LDtkPathUtility.TryCreateDirectory(parentPath);

            List<T> list = new List<T>();
            foreach (T artifact in artifacts)
            {
                if (artifact == null)
                {
                    continue;
                }
                
                string cloneName = assetName != null ? assetName : artifact.name;
                string destinationPath = $"{parentPath}/{cloneName}.asset";

                //Debug.Log($"Copy asset\n{artifact.name}\nto\n{destinationPath}");

                Object clone = CreateClone(artifact);
                clone.name = cloneName;

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