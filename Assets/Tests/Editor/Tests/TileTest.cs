using System.Linq;
using LDtkUnity;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class TileTest
    {
        //[Test]
        public void GetCorrectTileBits()
        {
            LdtkJson project = TestJsonLoader.DeserializeProject();

            Level level = project.Levels.FirstOrDefault(level1 => level1.LayerInstances != null);
            Assert.NotNull(level, "level was null");

            LayerInstance layer = level.LayerInstances.FirstOrDefault(p => p != null && p.IsTilesLayer);
            Assert.NotNull(layer, "Layer is null");
            
            TileInstance[] tiles = layer.GridTiles.ToArray();

            foreach (TileInstance tile in tiles)
            {
                Debug.Log($"Tile: {tile.FlipX}, {tile.FlipY}");
            }
            
            Assert.IsTrue(tiles[0].FlipX == false && tiles[0].FlipY == false);
            Assert.IsTrue(tiles[1].FlipX == true && tiles[1].FlipY == false);
            Assert.IsTrue(tiles[2].FlipX == false && tiles[2].FlipY == true);
            Assert.IsTrue(tiles[3].FlipX == true && tiles[3].FlipY == true);
        }
    }
}