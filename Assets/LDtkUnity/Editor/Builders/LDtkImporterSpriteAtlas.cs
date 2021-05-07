using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace LDtkUnity.Editor
{
    public class LDtkImporterSpriteAtlas
    {
        private readonly Sprite[] _assets;
        private readonly SpriteAtlas _atlas;

        public LDtkImporterSpriteAtlas(Sprite[] assets, SpriteAtlas atlas)
        {
            _assets = assets;
            _atlas = atlas;
        }

        public void AddToAtlas()
        {
            List<Object> packables = _atlas.GetPackables().ToList();
            _atlas.Remove(packables.ToArray());


            //Debug.Log(_assets.Sprites.Length);
            
            packables.RemoveAll(packedSprite => _assets.Any(assetSprite => assetSprite.name == packedSprite.name));
            
            Object[] sprites = _assets.Cast<Object>().ToArray();
            _atlas.Add(sprites);
            
        }
    }
}