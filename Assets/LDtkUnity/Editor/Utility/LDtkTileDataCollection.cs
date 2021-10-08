using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// With this class, we use it to gather the nessesary tiles that are used in a tileset
    /// </summary>
    public class LDtkTileDataCollection
    {
        private readonly Dictionary<LayerInstance, Vector2Int[]> _assets = new Dictionary<LayerInstance, Vector2Int[]>();

        public LDtkTileDataCollection(Texture2D tex, Vector2Int srcPos, int gridSize)
        {
            _formatter = new TilesetAssetNameFormatter(tex, srcPos, gridSize);
        }

        public void AddLayerTiles(LayerInstance layer)
        {
            if (layer.IsAutoLayer)
            {
                AddTiles(layer.AutoLayerTiles);
                return;
            }

            if (layer.IsTilesLayer)
            {
                AddTiles(layer.GridTiles);
            }
        }

        private void AddTiles(TileInstance[] layerAutoLayerTiles)
        {
            foreach (TileInstance tileInstance in layerAutoLayerTiles)
            {
                TryAddTile(tileInstance);
            }
        }

        private void TryAddTile(TileInstance tile)
        {
            TilesetAssetNameFormatter _formatter = new TilesetAssetNameFormatter()
            
            tile.UnitySrc
        }
    }
}