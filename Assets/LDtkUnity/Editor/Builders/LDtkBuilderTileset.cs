using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

namespace LDtkUnity.Editor.Builders
{
    public class LDtkBuilderTileset : LDtkLayerBuilder
    {
        private readonly TileInstance[] _tiles;
        private readonly Tilemap[] _tilemaps;
        
        private readonly Dictionary<Vector2Int, int> _builtTileLayering = new Dictionary<Vector2Int, int>();
        
        public LDtkBuilderTileset(LayerInstance layer, LDtkProjectImporter importer, TileInstance[] tiles, Tilemap[] tilemaps) : base(layer, importer)
        {
            _tiles = tiles;
            _tilemaps = tilemaps;
        }

        public void BuildTileset()
        {
            
            TilesetDefinition definition = Layer.IsAutoLayer
                ? Layer.Definition.AutoTilesetDefinition
                : Layer.Definition.TilesetDefinition;

            LDtkRelativeGetterTilesetTexture getter = new LDtkRelativeGetterTilesetTexture();
            Texture2D texAsset = getter.GetRelativeAsset(definition, Importer.assetPath);
            if (texAsset == null)
            {
                return;
            }
            
            //figure out if we have already built a tile in this position. otherwise, build up to the next tilemap
            
            
            foreach (TileInstance tileData in _tiles)
            {
                Tilemap tilemap = _tilemaps.FirstOrDefault();
                TileBase tile = GetTile(tileData, texAsset);
                SetTile(tileData, tilemap, tile);
            }

            //todo this may not be needed because it's imported instead
            /*foreach (Tilemap tilemap in tilemaps)
            {
                LDtkEditorUtil.Dirty(tilemap);
            }*/
        }

        private TileBase GetTile(TileInstance tileData, Texture2D texAsset)
        {
            LDtkArtifactAssets assets = Importer.AutomaticallyGeneratedArtifacts;
            if (assets == null)
            {
                Debug.LogError("Did not get ArtifactAssets");
                return null;
            }

            LDtkArtifactAssetsContentCreator creator = new LDtkArtifactAssetsContentCreator(Importer, assets, texAsset, tileData.UnitySrc, (int)Layer.TilesetDefinition.TileGridSize);
            TileBase tile = creator.TryGetOrCreateTile();

            
            
            if (tile == null)
            {
                Debug.LogError("Null tile, problem?");
            }
            return tile;


        }

        private void SetTile(TileInstance tileData, Tilemap tilemap, TileBase tile)
        {
            Vector2Int coord = GetConvertedCoord(tileData);

            Vector2Int px = tileData.UnityPx;
            int tilemapLayer = GetTilemapLayerToBuildOn(px);
            Vector3Int tilemapCoord = new Vector3Int(coord.x, coord.y, tilemapLayer);

            tilemap.SetTile(tilemapCoord, tile);
            
            ApplyTileInstanceFlips(tilemap, tileData, coord);
        }

        private Vector2Int GetConvertedCoord(TileInstance tileData)
        {
            //doing the division like this because the operator is not available in older unity versions
            Vector2Int coord = new Vector2Int(
                tileData.UnityPx.x / (int) Layer.GridSize,
                tileData.UnityPx.y / (int) Layer.GridSize);
            
            return LDtkToolOriginCoordConverter.ConvertCell(coord, (int) Layer.CHei);
        }

        /// <summary>
        /// Input a pixel position, and spits out the correct index of tilemap we need to build on.
        /// if we had already built on this position before, then we need to use the next tilemap component because that space is already occupied by a tile,
        /// and we can only have one tile in a position for a tilemap.
        ///
        /// ACTUALLY, the tiles have a z component in the tilemap, so position them this was instead by ordingering them.
        /// </summary>
        /// <param name="key">
        /// the pixel position.
        /// </param>
        /// <returns>
        /// the index of tilemap we'd wish to use.
        /// </returns>
        private int GetTilemapLayerToBuildOn(Vector2Int key)
        {
            if (_builtTileLayering.ContainsKey(key))
            {
                return _builtTileLayering[key]--;
            }

            int startingNumber = _tilemaps.Length - 1;
            _builtTileLayering.Add(key, startingNumber);
            return startingNumber;
        }
        
        private void ApplyTileInstanceFlips(Tilemap tilemap, TileInstance tileData, Vector2Int coord)
        {
            float rotX = tileData.FlipY ? 180 : 0;
            float rotY = tileData.FlipX ? 180 : 0;
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
            tilemap.SetTransformMatrix((Vector3Int) coord, matrix);
        }
    }
}
