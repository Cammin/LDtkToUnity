using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used to override how the tile should do animation, so that tiles can animate.
    /// This is only an option for animating a tile, and could completely go custom in terms of options for animation overriding instead of using this scriptable object.
    /// </summary>
    [HelpURL(LDtkHelpURL.SO_ART_TILE_OVERRIDE)]
    [CreateAssetMenu(fileName = nameof(LDtkArtTileAnimationOverride), menuName = LDtkToolScriptableObj.SO_ROOT + nameof(LDtkArtTileAnimationOverride), order = LDtkToolScriptableObj.SO_ORDER)]
    [ExcludeFromDocs]//todo remove this once animated tiles are added
    public class LDtkArtTileAnimationOverride : TileBase
    {
        /// <summary>
        /// The List of Sprites set for the Animated Tile.
        /// This will be played in sequence.
        /// </summary>
        [SerializeField] private Sprite[] _animatedSprites = Array.Empty<Sprite>();
        [SerializeField] private float _animationSpeed = 1;
        [SerializeField] private float _animationStartTime = 0;

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            if (_animatedSprites.IsNullOrEmpty())
            {
                return false;
            }
            
            tileAnimationData.animatedSprites = _animatedSprites;
            tileAnimationData.animationSpeed = _animationSpeed;
            tileAnimationData.animationStartTime = _animationStartTime;
            return true;
        }
    }
}