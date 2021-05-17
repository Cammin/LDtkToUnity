using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkBuilderTileset : LDtkLayerBuilder
    {
        private TileInstance[] _tiles;

        private readonly LDtkLayeredTilesetProvider _tilesetProvider;
        private int _layerCount = 0;
        
        public LDtkBuilderTileset(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder) : base(importer, layerGameObject, sortingOrder)
        {
            _tilesetProvider = new LDtkLayeredTilesetProvider(sortingOrder, ConstructNewTilemap);
        }

        public void BuildTileset(TileInstance[] tiles)
        {
            _tiles = tiles;
            
            _tilesetProvider.Clear();
            
            
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
            for (int i = _tiles.Length - 1; i >= 0; i--)
            {
                TileInstance tileData = _tiles[i];
                Tilemap tilemap = _tilesetProvider.GetAppropriatelyLayeredTilemap(tileData.UnityPx);

                TileBase tile = Importer.GetTile(texAsset, tileData.UnitySrc, (int)Layer.TilesetDefinition.TileGridSize);
                
                SetTile(tileData, tilemap, tile);
            }

            //set each layer's alpha
            foreach (Tilemap tilemap in _tilesetProvider.Tilemaps)
            {
                tilemap.SetOpacity(Layer);
            }
        }
        
        private Tilemap ConstructNewTilemap()
        {

            string objName = $"{GetLayerName(Layer)}_{_layerCount}";
            GameObject tilemapObj = LayerGameObject.CreateChildGameObject(objName);
            Tilemap tilemap = tilemapObj.AddComponent<Tilemap>();

            TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = SortingOrder.SortingOrderValue;

            _layerCount++;
            
            return tilemap;
        }

        // Layer type (possible values: IntGrid, Entities, Tiles or AutoLayer)
        private string GetLayerName(LayerInstance layer)
        {
            if (layer.IsTilesLayer)
            {
                return "Tiles";
            }

            return "AutoLayer";

        }

        private void SetTile(TileInstance tileData, Tilemap tilemap, TileBase tile)
        {
            Vector2Int coord = GetConvertedCoord(tileData);

            //Vector2Int px = tileData.UnityPx;
            //int tilemapLayer = GetTilemapLayerToBuildOn(px);
            Vector3Int tilemapCoord = new Vector3Int(coord.x, coord.y, 0);

            tilemap.SetTile(tilemapCoord, tile);
            
            ApplyTileInstanceFlips(tilemap, tileData, tilemapCoord);
        }

        private Vector2Int GetConvertedCoord(TileInstance tileData)
        {
            //doing the division like this because the operator is not available in older unity versions
            Vector2Int coord = new Vector2Int(
                tileData.UnityPx.x / (int) Layer.GridSize,
                tileData.UnityPx.y / (int) Layer.GridSize);
            
            return LDtkToolOriginCoordConverter.ConvertCell(coord, (int) Layer.CHei);
        }

        
        
        private void ApplyTileInstanceFlips(Tilemap tilemap, TileInstance tileData, Vector3Int coord)
        {
            float rotX = tileData.FlipY ? 180 : 0;
            float rotY = tileData.FlipX ? 180 : 0;
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
            
            tilemap.SetTransformMatrix(coord, matrix);
        }


    }
}
