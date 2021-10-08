using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerAnimatedTile : LDtkAssetDrawer<LayerDefinition, LDtkArtTileAnimation>
    {
        private TilesetAssetNameFormatter _keyName;
        
        
        public LDtkDrawerAnimatedTile(LayerDefinition data, SerializedProperty prop, string key) : base(data, prop, key)
        {
        }

        public override void Draw()
        {
            base.Draw();
        }


    }
}