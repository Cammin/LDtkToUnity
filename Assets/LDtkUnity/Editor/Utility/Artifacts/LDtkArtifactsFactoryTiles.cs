using System;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// This takes the already-generated sprites from the tileset importer, and makes tile assets out of it.
    /// This is content that was originally in the standard artifacts factory
    ///
    /// All that we're doing is creating tilemap tiles: That means we should only make tiles out of the ones that are the sprites that are the height/wisth of the gridsize.
    /// We only depend on the sprites supplied. No dependence on a texture, because the name of the tile is the exact same name as the sprite.
    ///  
    /// </summary>
    internal sealed class LDtkArtifactsFactoryTiles
    {
        private readonly LDtkTilesetImporter _importer;
        private readonly AssetImportContext _ctx;
        
        private readonly HashSet<string> _uniqueTiles = new HashSet<string>();
        private readonly List<Action> _tileActions = new List<Action>();
        
        //public 

        public void MakeTilesForSprites(List<Sprite> sprites)
        {
            Profiler.BeginSample($"TileActions {_tileActions.Count}");
            foreach (Sprite sprite in sprites)
            {
                /*if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return false;
            }*/
                //LDtkTileArtifactFactory tileFactory = CreateArtTileFactory(sprite);

                string assetName = sprite.name;
                if (!_uniqueTiles.Add(assetName))
                {
                    continue;
                }
                //new LDtkTileArtifactFactory(_ctx, _artifacts, assetName)?.TryCreateTile();
            }
            Profiler.EndSample();
        }
        
        
        /*private LDtkTileArtifactFactory CreateArtTileFactory(Texture2D srcTex, Rect srcPos)
        {
            
        }
        
        /// <summary>
        /// Based on the Field slices, add some sprite creation actions. instead, we're going to just cache the  
        /// </summary>
        private void SetupAllFieldSlices(List<TilesetRectangle> fieldSlices)
        {
            foreach (TilesetRectangle rectangle in fieldSlices)
            {
                //Debug.Log($"Process FieldSlice: {rectangle}");

                Texture2D texAsset = null;
                if (rectangle.Tileset.IsEmbedAtlas)
                {
                    texAsset = LDtkProjectSettings.InternalIconsTexture;
                    if (texAsset == null)
                    {
                        LDtkDebug.LogError($"A Tile field uses the LDtk internal icons texture but it's not assigned in the project settings.");
                        continue;
                    }
                }
                else
                {
                    texAsset = _dict.Get(rectangle.Tileset.RelPath);
                    if (texAsset == null)
                    {
                        LDtkDebug.LogError($"Didn't load texture at path \"{rectangle.Tileset.RelPath}\" when setting up field slices {_dict.Textures.Count()}");
                        continue;
                    }
                }
                
                if (!ValidateTextureWithTilesetDef(rectangle.Tileset, texAsset))
                {
                    continue;
                }

                _spriteActions.Add(() => CreateSprite(texAsset, rectangle.UnityRect, _pixelsPerUnit));
            }
        }*/
    }
}