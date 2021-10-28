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
        private readonly Sprite _defaultSprite;
        private readonly Texture2D _defaultTexture;
        
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
            _defaultSprite = null;//LDtkResourcesLoader.LoadDefaultTileSprite();
            _defaultTexture = Texture2D.whiteTexture; //_defaultSprite.texture;
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
            LDtkIntGridTile defaultTile = LDtkResourcesLoader.LoadDefaultTile();
            Tile nativeDefaultTile = CreateNativeTile(defaultTile);


            Texture2D defaultTextureClone = CloneArtifacts(new[] { _defaultTexture }.ToList(), "/Sprites", "LDtkDefault").First();

            //string defaultTextureClonePath = AssetDatabase.GetAssetPath(defaultTextureClone);

            Sprite defaultSpriteClone = Sprite.Create(defaultTextureClone, new Rect(0, 0, 4, 4), new Vector2(2, 2), 4);
            CloneArtifacts(new[] { defaultSpriteClone }.ToList(), "/Sprites", defaultTextureClone.name + "_sprite");
            //AssetDatabase.ImportAsset(defaultTextureClonePath);



            List<Sprite> artTileSprites = CloneArtifacts(_assets.SpriteArtifacts, "/Sprites");
            _artTiles = CloneArtifacts(_assets.TileArtifacts, "/ArtTiles").Cast<Tile>().ToList();

            List<TileBase> intGridArtifacts = _importer.GetIntGridTiles().Where(p => p != null).Append(nativeDefaultTile).ToList();
            _intGridTiles = CloneArtifacts(intGridArtifacts, "/IntGridValues").Cast<Tile>().ToList();

            _backgroundArtifacts = CloneArtifacts(_assets.BackgroundArtifacts, "/Backgrounds");
            
            //give each new native art tile a matching cloned sprite to reference
            foreach (Tile artTile in _artTiles)
            {
                string nameMatch = artTile.name;
                Sprite artTileSprite = artTileSprites.Find(sprite => sprite.name == nameMatch);
                artTile.sprite = artTileSprite;
            }
            
            //give the int grid tiles the clone sprite instead if they were using the default tile
            foreach (Tile intGridTile in _intGridTiles)
            {
                if (intGridTile.sprite == _defaultSprite)
                {
                    intGridTile.sprite = defaultSpriteClone;
                }
            } 
            
            //we've generated the default sprite which is used by multiple, simply add to the list since it's already created
            _backgroundArtifacts.Add(defaultSpriteClone);
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