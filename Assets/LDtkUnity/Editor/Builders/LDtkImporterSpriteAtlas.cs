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
            
            List<Object> existing = _atlas.GetPackables().ToList();
            //_atlas.Remove(packables.ToArray());


            //Debug.Log(_assets.Sprites.Length);
            
            //existing.RemoveAll(packedSprite => _assets.Any(assetSprite => assetSprite.name == packedSprite.name));
            
            //only add those items that are not already added by name
            //filter out if 
            Object[] sprites = _assets.Cast<Object>().Where(p => existing.IsNullOrEmpty() || existing.Any(packable => packable.name != p.name)).ToArray();
            
            _atlas.Add(sprites);
            
        }
    }
}