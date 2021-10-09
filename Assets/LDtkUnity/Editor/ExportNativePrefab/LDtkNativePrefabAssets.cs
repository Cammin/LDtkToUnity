using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkNativePrefabAssets
    {
        public LDtkArtifactAssets _assets;

        public void GenerateAssets()
        {
            List<Sprite> sprites = _assets.SpriteArtifacts;
        }
    }
}