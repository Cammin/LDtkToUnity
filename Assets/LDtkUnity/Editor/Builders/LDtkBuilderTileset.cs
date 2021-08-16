using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkBuilderTileset : LDtkLayerBuilder
    {
        private TileInstance[] _tiles;

        private readonly OffsetTilemapStacks _tilesetProvider;
        private int _layerCount = 0;

        public List<Tilemap> Tilemaps = new List<Tilemap>();
        
        public LDtkBuilderTileset(LDtkProjectImporter importer, GameObject layerGameObject, LDtkSortingOrder sortingOrder) : base(importer, layerGameObject, sortingOrder)
        {
            _tilesetProvider = new OffsetTilemapStacks(ConstructNewTilemap);
            
        }

        public void BuildTileset(TileInstance[] tiles)
        {
            
            //if we are also an intgrid layer, then we already reduced our position in the intGridBuilder
            if (!Layer.IsIntGridLayer)
            {
                RoundTilemapPos();
            }
            
            _tiles = tiles;
            
            _tilesetProvider.Clear();

            TilesetDefinition definition = EvaluateTilesetDefinition();
            if (definition == null)
            {
                Debug.LogError($"LDtk: Tileset Definition for {Layer.Identifier} was null.");
                return;
            }

            LDtkRelativeGetterTilesetTexture getter = new LDtkRelativeGetterTilesetTexture();
            Texture2D texAsset = getter.GetRelativeAsset(definition, Importer.assetPath);
            if (texAsset == null)
            {
                return;
            }
            
            Importer.SetupAssetDependency(texAsset);
            LogPotentialTextureProblems(texAsset);
            
            //figure out if we have already built a tile in this position. otherwise, build up to the next tilemap. build in a completely seperate p[ath if this is an offset position from the normal standard coordinates
            for (int i = _tiles.Length - 1; i >= 0; i--)
            {
                TileInstance tileData = _tiles[i];
                Tilemap tilemap = _tilesetProvider.GetTilemapFromStacks(tileData.UnityPx, (int)Layer.GridSize);
                
                
                Tilemaps.Add(tilemap);

                TileBase tile = Importer.GetTile(texAsset, tileData.UnitySrc, (int)Layer.TilesetDefinition.TileGridSize);
                
                SetTile(tileData, tilemap, tile);
            }

            //set each layer's alpha
            foreach (Tilemap tilemap in _tilesetProvider.Tilemaps)
            {
                AddLayerOffset(tilemap);
                tilemap.SetOpacity(Layer);
            }
        }

        private void LogPotentialTextureProblems(Texture2D tex)
        {
            string texPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(texPath);
            if (importer.textureType == TextureImporterType.Sprite)
            {
                return;
            }
            
            Debug.LogWarning($"LDtk: Referenced texture type is not Sprite. It is recommended to use Sprite mode for texture: \"{tex.name}\"", tex);
            
            if (importer.npotScale != TextureImporterNPOTScale.None)
            {
                Debug.LogError($"LDtk: Referenced texture Non-Power of Two is not None, which may corrupt the tileset art! Fix this for: \"{Importer.AssetName}\"", tex);
            }
        }

        private TilesetDefinition EvaluateTilesetDefinition()
        {
            if (Layer.IsAutoLayer)
            {
                return Layer.Definition.AutoTilesetDefinition;
            }

            if (Layer.OverrideTilesetUid != null)
            {
                return Layer.OverrideTilesetDefinition;
            }

            return Layer.Definition.TilesetDefinition;
        }

        private Tilemap ConstructNewTilemap()
        {
            SortingOrder.Next();
            
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

            return ConvertCellCoord(coord);
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
