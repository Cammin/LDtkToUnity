using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

namespace LDtkUnity.Editor
{
    public class LDtkBuilderTileset : LDtkLayerBuilder
    {
        public LDtkBuilderTileset(LayerInstance layer, LDtkProject project) : base(layer, project)
        {
        }

        public void BuildTileset(TileInstance[] tiles, Tilemap[] tilemaps)
        {
            TilesetDefinition definition = Layer.IsAutoLayer
                ? Layer.Definition.AutoTilesetDefinition
                : Layer.Definition.TilesetDefinition;
            
            Texture2D texAsset = Project.GetTileset(definition.Identifier);
            if (texAsset == null)
            {
                return;
            }
            
            //it's important to allow the sprite to have read/write enabled
            if (!texAsset.isReadable)
            {
                Debug.LogError($"Tileset \"{texAsset.name}\" texture does not have Read/Write Enabled, is it enabled?", texAsset);
                return;
            }
            
            //figure out if we have already built a tile in this position. otherwise, build up to the next tilemap
            Dictionary<Vector2Int, int> builtTileLayering = new Dictionary<Vector2Int, int>();
            
            foreach (TileInstance tileData in tiles)
            {
                Vector2Int px = tileData.Px.ToVector2Int();
                int tilemapLayer = GetTilemapLayerToBuildOn(builtTileLayering, px, tilemaps.Length-1);

                Tilemap tilemap = tilemaps[tilemapLayer];
                GetTile(tileData, texAsset, tilemap);
            }

            foreach (Tilemap tilemap in tilemaps)
            {
                LDtkEditorUtil.Dirty(tilemap);
            }
        }

        private void GetTile(TileInstance tileData, Texture2D texAsset, Tilemap tilemap)
        {
            Vector2Int imageSliceCoord = LDtkToolOriginCoordConverter.ImageSliceCoord(tileData.SourcePixelPosition, texAsset.height, Project.PixelsPerUnit);
            LDtkTileCollection tileCollection = Project.GetTileCollection(Layer.TilesetDefinition.Identifier);

            if (tileCollection == null)
            {
                return;
            }
            
            string key = LDtkKeyFormatUtil.TilesetKeyFormat(texAsset, imageSliceCoord);
            Tile tile = tileCollection.GetByName(key);
            
            if (tile == null)
            {
                return;
            }
            
            Vector2Int coord = GetConvertedCoord(tileData);
            Vector3Int tilemapCoord = new Vector3Int(coord.x, coord.y, 0);
            tilemap.SetTile(tilemapCoord, tile);
            SetTileFlips(tilemap, tileData, coord); 
        }

        private Vector2Int GetConvertedCoord(TileInstance tileData)
        {
            Vector2Int coord = new Vector2Int(tileData.LayerPixelPosition.x / (int) Layer.GridSize,
                tileData.LayerPixelPosition.y / (int) Layer.GridSize);
            coord = LDtkToolOriginCoordConverter.ConvertCell(coord, (int) Layer.CHei);
            return coord;
        }

        private int GetTilemapLayerToBuildOn(Dictionary<Vector2Int, int> builtTileLayering, Vector2Int key, int startingNumber)
        {
            if (builtTileLayering.ContainsKey(key))
            {
                return --builtTileLayering[key];
            }
            
            builtTileLayering.Add(key, startingNumber);
            return startingNumber;
        }
        
        private void SetTileFlips(Tilemap tilemap, TileInstance tileData, Vector2Int coord)
        {
            float rotY = tileData.FlipX ? 180 : 0;
            float rotX = tileData.FlipY ? 180 : 0;
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
            tilemap.SetTransformMatrix((Vector3Int) coord, matrix);
        }
    }
}
