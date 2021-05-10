using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkTileArtifactFactory
    {

        private delegate Object ObjectCreationAction();

        private readonly LDtkProjectImporter _importer;
        private readonly LDtkArtifactAssets _assets;
        private readonly Texture2D _srcTex;
        private readonly Vector2Int _srcPos;
        private readonly int _pixelsPerUnit; 

        public LDtkTileArtifactFactory(LDtkProjectImporter importer, LDtkArtifactAssets assets, Texture2D srcTex, Vector2Int srcPos, int pixelsPerUnit)
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

            Object TileCreationAction()
            {
                LDtkArtTile newArtTile = ScriptableObject.CreateInstance<LDtkArtTile>();
                Sprite sprite = TryGetOrCreateSprite(assetName);

                if (sprite == null)
                {
                    Debug.LogError("TileCreationAction error");
                    return null;
                }
                
                newArtTile.name = assetName;
                newArtTile._artSprite = sprite;
                //get a sprite
                
                return newArtTile;
            }
            
            return (TileBase)CreateAndAddAsset(TileCreationAction);
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
            return (Sprite)CreateAndAddAsset(SpriteCreationAction);
        }
        
        private Object CreateAndAddAsset(ObjectCreationAction objectCreationAction)
        {
            Object obj = objectCreationAction.Invoke();
            if (obj == null)
            {
                Debug.LogError("CreateAndAddAsset error");
                return null;
            }
            _importer.AddArtifact(obj);
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