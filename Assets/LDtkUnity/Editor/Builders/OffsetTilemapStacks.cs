using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    internal sealed class OffsetTilemapStacks
    {
        private readonly Dictionary<Vector2Int, OffsetTilemapStack> _stacks = new Dictionary<Vector2Int, OffsetTilemapStack>();
        private readonly TilemapCreation _creationAction;

        public IEnumerable<Tilemap> Tilemaps => _stacks.Values.SelectMany(stack => stack.Tilemaps);
        
        public OffsetTilemapStacks(TilemapCreation creationAction)
        {
            _creationAction = creationAction;
        }
        
        public TilemapTilesBuilder GetTilemapFromStacks(Vector2Int pxPos, int gridSize)
        {
            if (gridSize == 0)
            {
                LDtkDebug.LogError("Unexpected problem");
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

        public void SetPendingTiles()
        {
            foreach (OffsetTilemapStack builder in _stacks.Values)
            {
                builder.SetPendingTiles();
            }
        }
        
        public void Clear()
        {
            _stacks.Clear();
        }
    }
}