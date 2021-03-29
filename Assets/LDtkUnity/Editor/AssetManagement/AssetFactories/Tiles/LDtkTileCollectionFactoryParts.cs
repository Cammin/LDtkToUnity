using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkTileCollectionFactoryParts
    {
        public readonly string Name;
        public readonly Sprite Sprite;
        public readonly Color Color;
            
        public LDtkTileCollectionFactoryParts(string name, Sprite sprite, Color color)
        {
            Name = name;
            Sprite = sprite;
            Color = color;
        }
    }
}