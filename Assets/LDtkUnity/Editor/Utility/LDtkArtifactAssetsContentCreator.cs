using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Methods that belong to LDtkArtifactAssets but use editor functionality
    /// </summary>
    public class LDtkArtifactAssetsContentCreator
    {

        private delegate T ObjectCreationAction<out T>();
        private delegate void ListAddAction<in T>(T addedInput);


        private readonly LDtkProjectImporter _importer;
        private readonly LDtkArtifactAssets _assets;
        private readonly Texture2D _srcTex;
        private readonly Vector2Int _srcPos;
        private readonly int _pixelsPerUnit; 

        public LDtkArtifactAssetsContentCreator(LDtkProjectImporter importer, LDtkArtifactAssets assets, Texture2D srcTex, Vector2Int srcPos, int pixelsPerUnit)
        {
            _importer = importer;
            _assets = assets;
            _srcTex = srcTex;
            _srcPos = srcPos;
            _pixelsPerUnit = pixelsPerUnit;
        }


        public TileBase TryGetOrCreateTile()
        {
            string assetName = GetKeyName();
            
            //if we already cached from a previous operation
            TileBase tile = _assets.GetTileByName(assetName);
            if (tile != null)
            {
                return tile;
            }

            LDtkTile TileCreationAction()
            {
                LDtkTile newTile = ScriptableObject.CreateInstance<LDtkTile>();
                Sprite sprite = TryGetOrCreateSprite(assetName);

                if (sprite == null)
                {
                    Debug.LogError("TileCreationAction error");
                    return null;
                }
                
                newTile.name = assetName;
                newTile._sprite = sprite;
                //get a sprite
                
                return newTile;
            }

            return CreateAndAddAsset(_assets.AddTile, TileCreationAction, assetName);
        }
        

        private Sprite TryGetOrCreateSprite(string assetName)
        {
            //if we already cached from a previous operation
            Sprite item = _assets.GetSpriteByName(assetName);
            if (item != null)
            {
                return item;
            }
            
            Sprite SpriteCreationAction()
            {
                LDtkTextureSpriteSlicer slicer = new LDtkTextureSpriteSlicer(_srcTex, _pixelsPerUnit);
                Sprite sprite = slicer.CreateSpriteSliceForPosition(_srcPos);

                if (sprite == null)
                {
                    Debug.LogError("SpriteCreationAction error");
                    return null;
                }
                
                sprite.name = assetName;
                
                return sprite;
            }
            
            //otherwise make a new one
            return CreateAndAddAsset(_assets.AddSprite, SpriteCreationAction, assetName);
        }
        
        private T CreateAndAddAsset<T>(ListAddAction<T> listAddAction, ObjectCreationAction<T> objectCreationAction, string assetName) where T : Object
        {
            T obj = objectCreationAction.Invoke();
            if (obj == null)
            {
                Debug.LogError("CreateAndAddAsset error");
                return null;
            }
            
            //make any of the assets invisible from the hierarchy
            obj.hideFlags = HideFlags.HideInHierarchy;
            _importer.ImportContext.AddObjectToAsset(assetName, obj);

            listAddAction.Invoke(obj);
            return obj;
        }
        
        private string GetKeyName()
        {
            Vector2Int imageSliceCoord = LDtkToolOriginCoordConverter.ImageSliceCoord(_srcPos, _srcTex.height, _pixelsPerUnit);
            string key = LDtkKeyFormatUtil.TilesetKeyFormat(_srcTex, imageSliceCoord);
            return key;
        }
    }
}