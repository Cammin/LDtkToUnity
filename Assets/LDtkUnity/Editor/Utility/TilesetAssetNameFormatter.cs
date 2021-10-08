using UnityEngine;

namespace LDtkUnity.Editor
{
    public class TilesetAssetNameFormatter
    {
        private readonly Texture2D _srcTex;
        private readonly Vector2Int _srcPos;
        private readonly int _gridSize;

        public TilesetAssetNameFormatter(Texture2D srcTex, Vector2Int srcPos, int gridSize)
        {
            _srcTex = srcTex;
            _srcPos = srcPos;
            _gridSize = gridSize;
        }

        public string GetAssetName()
        {
            Vector2Int imageSliceCoord = LDtkCoordConverter.ImageSliceCoord(_srcPos, _srcTex.height, _gridSize);
            string key = LDtkKeyFormatUtil.TilesetKeyFormat(_srcTex, imageSliceCoord);
            return key;
        }
    }
}