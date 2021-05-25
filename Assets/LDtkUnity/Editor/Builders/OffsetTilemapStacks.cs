using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public delegate Tilemap TilemapCreation();
    public class OffsetTilemapStacks
    {
        private readonly Dictionary<Vector2Int, OffsetTilemapStack> _stacks = new Dictionary<Vector2Int, OffsetTilemapStack>();
        private readonly TilemapCreation _creationAction;

        public IEnumerable<Tilemap> Tilemaps => _stacks.Values.SelectMany(stack => stack.Tilemaps);
        
        public OffsetTilemapStacks(TilemapCreation creationAction)
        {
            _creationAction = creationAction;
        }
        
        public Tilemap GetTilemapFromStacks(Vector2Int pxPos, int gridSize)
        {
            if (gridSize == 0)
            {
                Debug.LogError("Unexpected problem");
                return null;
            }
            
            OffsetTilemapStack stack = GetStack(pxPos, gridSize);
            return stack.GetTilemapForTilePosition(pxPos);
        }

        private OffsetTilemapStack GetStack(Vector2Int pxPos, int gridSize)
        {
            Vector2Int offset = pxPos;
            offset.x %= gridSize;
            offset.y %= gridSize;

            if (_stacks.ContainsKey(offset))
            {
                return _stacks[offset];
            }

            OffsetTilemapStack newStack = new OffsetTilemapStack(gridSize, offset, _creationAction);
            _stacks.Add(offset, newStack);
            
            return newStack;
        }

        public void Clear()
        {
            _stacks.Clear();
        }
    }
}