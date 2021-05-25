using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class OffsetTilemapStack
    {
        private readonly TilemapCreation _creationAction;
        private readonly Dictionary<Vector2Int, int> _stacking = new Dictionary<Vector2Int, int>();
        private readonly Dictionary<int, Tilemap> _tilemaps = new Dictionary<int, Tilemap>();

        private readonly Vector2Int _offset;
        private readonly int _gridSize;

        public IEnumerable<Tilemap> Tilemaps => _tilemaps.Values;
        
        //tile position to order
        public OffsetTilemapStack(int gridSize, Vector2Int offset, TilemapCreation creationAction)
        {
            _gridSize = gridSize;
            _offset = offset;
            _creationAction = creationAction;
        }
        
        public Tilemap GetTilemapForTilePosition(Vector2Int pos)
        {
            pos -= _offset;

            int stackOrder = GetStackOrderToBuildOn(pos);
            if (_tilemaps.ContainsKey(stackOrder))
            {
                return _tilemaps[stackOrder];
            }
            
            Tilemap tilemap = _creationAction.Invoke();

            //modify tile anchor so that they are correctly aligned even if some rules tell the tiles to be an odd-formation (ie. The shelves in Typical 2D Platformer)
            Vector2 extraAnchor = (Vector2)_offset / _gridSize;
            extraAnchor.y = -extraAnchor.y;
            tilemap.tileAnchor += (Vector3)extraAnchor;
            
            _tilemaps.Add(stackOrder, tilemap);
            return tilemap;
        }
        
        private int GetStackOrderToBuildOn(Vector2Int pos)
        {
            if (_stacking.ContainsKey(pos))
            {
                return ++_stacking[pos];
            }
            
            _stacking.Add(pos, 0);
            return 0;
        }
    }
}