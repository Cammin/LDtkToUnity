using System.Collections.Generic;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderTileset
    {
        //tile scale is to solve tearing lines in the tilemap
        private const float TILE_SCALE = 1; //todo try solving the tearing in an alternate method other than this
        
        public static void BuildTileset(LDtkDataLayer layer, LDtkDataTile[] tiles, LDtkTilesetAssetCollection assets, Tilemap tilemap)
        {
            LDtkDefinitionTileset definition = layer.IsAutoTilesLayer
                ? layer.Definition.AutoTilesetDefinition
                : layer.Definition.TileLayerDefinition;
            
            LDtkTilesetAsset asset = assets.GetAssetByIdentifier(definition.identifier);
            if (asset == null) return;
            
            foreach (LDtkDataTile tileData in tiles)
            {
                BuildTile(layer, tileData, asset, tilemap);
            }
        }

        private static void BuildTile(LDtkDataLayer layer, LDtkDataTile tileData, LDtkTilesetAsset asset, Tilemap tilemap)
        {
            Vector2Int coord = tileData.LayerPixelPosition / layer.__gridSize;
            coord = LDtkToolOriginCoordConverter.ConvertCell(coord, layer.__cHei);

            Sprite tileSprite = GetTileFromTileset(asset.ReferencedAsset, tileData.SourcePixelPosition, layer.__gridSize);

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = Tile.ColliderType.None;
            tile.sprite = tileSprite;

            Vector3Int co = new Vector3Int(coord.x, coord.y, 0);
            tilemap.SetTile(co, tile);

            SetTileFlips(tilemap, tileData, coord);
        }

        private static Sprite GetTileFromTileset(Sprite tileset, Vector2Int sourceCathodeRayPos, int pixelsPerUnit)
        {
            Debug.Assert(pixelsPerUnit != 0);
            
            sourceCathodeRayPos = LDtkToolOriginCoordConverter.ConvertPixel(sourceCathodeRayPos, tileset.texture.height, pixelsPerUnit);
            
            Vector2Int tileSize = Vector2Int.one * pixelsPerUnit;
            Rect rect = new Rect(sourceCathodeRayPos, tileSize);
            
            return LDtkProviderTilesetSprite.GetSpriteFromTilesetAndRect(tileset, rect, pixelsPerUnit);
        }

        private static void SetTileFlips(Tilemap tilemap, LDtkDataTile tileData, Vector2Int coord)
        {
            float rotY = tileData.FlipX ? 180 : 0;
            float rotX = tileData.FlipY ? 180 : 0;
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one * TILE_SCALE);
            tilemap.SetTransformMatrix((Vector3Int) coord, matrix);
        }
    }
}
