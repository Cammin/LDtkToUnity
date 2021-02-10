using System.Collections.Generic;
using LDtkUnity.Providers;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

namespace LDtkUnity.Builders
{
    public static class LDtkBuilderTileset
    {
        public static void BuildTileset(LayerInstance layer, TileInstance[] tiles, LDtkProject project, Tilemap[] tilemaps)
        {
            TilesetDefinition definition = layer.IsAutoTilesLayer
                ? layer.Definition.AutoTilesetDefinition
                : layer.Definition.TilesetDefinition;
            
            LDtkTilesetAsset asset = project.GetTileset(definition.Identifier);
            if (asset == null) return;
            
            //it's important to allow the sprite to have read/write enabled
            Texture2D tex = asset.ReferencedAsset;
            if (!tex.isReadable)
            {
                Debug.LogError($"Tileset \"{tex.name}\" texture does not have Read/Write Enabled, is it enabled?", tex);
                return;
            }
            
            //figure out if we have already built a tile in this position. otherwise, build up to the next tilemap
            Dictionary<Vector2Int, int> builtTileLayering = new Dictionary<Vector2Int, int>();
            
            foreach (TileInstance tileData in tiles)
            {
                Vector2Int px = tileData.Px.ToVector2Int();
                int tilemapLayer = GetTilemapLayerToBuildOn(builtTileLayering, px, tilemaps.Length-1);

                
                BuildTile(layer, tileData, asset, tilemaps[tilemapLayer]);
            }
        }

        private static void BuildTile(LayerInstance layer, TileInstance tileData, LDtkTilesetAsset asset, Tilemap tilemap)
        {
            Vector2Int coord = GetConvertedCoord(layer, tileData);
            Vector3Int tilemapCoord = new Vector3Int(coord.x, coord.y, 0);

            //todo gain a reference to this instead of statically trying to get it; its hacky
            Tile tile = LDtkTilemapTileFactory.Get(asset.ReferencedAsset, tileData.SourcePixelPosition, (int)layer.GridSize);

            
            
            tilemap.SetTile(tilemapCoord, tile);
            SetTileFlips(tilemap, tileData, coord); 
        }

        private static Vector2Int GetConvertedCoord(LayerInstance layer, TileInstance tileData)
        {
            Vector2Int coord = new Vector2Int(tileData.LayerPixelPosition.x / (int) layer.GridSize,
                tileData.LayerPixelPosition.y / (int) layer.GridSize);
            coord = LDtkToolOriginCoordConverter.ConvertCell(coord, (int) layer.CHei);
            return coord;
        }

        private static int GetTilemapLayerToBuildOn(Dictionary<Vector2Int, int> builtTileLayering, Vector2Int key, int startingNumber)
        {
            if (builtTileLayering.ContainsKey(key))
            {
                return --builtTileLayering[key];
            }
            
            builtTileLayering.Add(key, startingNumber);
            return startingNumber;
        }
        

        private static void SetTileFlips(Tilemap tilemap, TileInstance tileData, Vector2Int coord)
        {
            float rotY = tileData.FlipX ? 180 : 0;
            float rotX = tileData.FlipY ? 180 : 0;
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
            tilemap.SetTransformMatrix((Vector3Int) coord, matrix);
        }
    }
}
