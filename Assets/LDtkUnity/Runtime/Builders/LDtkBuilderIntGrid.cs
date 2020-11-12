using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Colliders;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderIntGrid
    {
        //todo add this text to the docs to be clear that tilesets arent exactly required if the user only wants simple art in teir game since tile assets can contain art themselves, if there was no need for complex art.
        //only builds the collision boxes. if a user wants to put sprites onto tiles, they are free to do that instead of trying to make tileset art stuff.
        
        public static void BuildIntGrid(LDtkDataLayer layer, LDtkIntGridTileAssetCollection tileAssets, Tilemap tilemap)
        {
            foreach (LDtkDataIntGridValue intTile in layer.intGrid)
            {
                BuildTile(layer, intTile, tileAssets, tilemap);
            }

            if (!tileAssets.CollisionTilesVisible)
            {
                tilemap.GetComponent<TilemapRenderer>().enabled = false;
            }
        }

        private static void BuildTile(LDtkDataLayer layer, LDtkDataIntGridValue intValueData, LDtkIntGridTileAssetCollection tileAssets, Tilemap tilemap)
        {
            LDtkDefinitionIntGridValue definition = layer.Definition.intGridValues[intValueData.v];

            LDtkIntGridTileAsset asset = tileAssets.GetAssetByIdentifier(definition.identifier);
            if (asset == null) return;

            Tile referencedTile = asset.ReferencedAsset;

            //todo cache any unique ones, so that they can be used instead, for performance
            Tile newTileInstance = ScriptableObject.CreateInstance<Tile>();
            newTileInstance.colliderType = referencedTile.colliderType;
            newTileInstance.sprite = referencedTile.sprite;
            newTileInstance.color = definition.color.ToColor();

            Vector2Int coordToSet = LDtkToolTileCoord.GetCellPositionFromCoordID(intValueData.coordId, layer.CellSize);
            tilemap.SetTile((Vector3Int)coordToSet, newTileInstance);
        }
    }
}