using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal class LDtkBackgroundSliceCreator
    {
        private LDtkProjectImporter _importer;

        private LDtkBackgroundTextureLoader _dict;
        private readonly HashSet<string> _uniqueSprites = new HashSet<string>();

        public LDtkBackgroundSliceCreator(LDtkProjectImporter importer)
        {
            _importer = importer;
        }
        
        public List<Sprite> CreateAllBackgrounds(LdtkJson json)
        {
            Profiler.BeginSample("TextureDict.LoadAllProjectTextures");
            _dict = new LDtkBackgroundTextureLoader(_importer.assetPath);
            _dict.CacheTextures(json);
            Profiler.EndSample();

            Profiler.BeginSample("SetupAllBackgroundSlices");
            List<Sprite> list = MakeAllBackgroundSlices(json);
            Profiler.EndSample();

            return list;
        }
        
        private LDtkBackgroundArtifactFactory CreateBackgroundFactory(Texture2D srcTex, Level level, int pixelsPerUnit)
        {
            string assetName = level.Identifier;
            if (!_uniqueSprites.Add(assetName))
            {
                return null;
            }
            return new LDtkBackgroundArtifactFactory(assetName, srcTex, pixelsPerUnit, level);
        }
        
        private List<Sprite> MakeAllBackgroundSlices(LdtkJson json)
        {
            List<Sprite> backgrounds = new List<Sprite>();
            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels)
                {
                    //if no bg pos defined, then no background was set.
                    if (level.BgPos == null)
                    {
                        continue;
                    }
                    
                    Texture2D bgTex = _dict.GetTexture(level.BgRelPath);
                    if (bgTex == null)
                    {
                        continue;
                    }
                    
                    if (!ValidateTextureWithTilesetDef(level, bgTex))
                    {
                        continue;
                    }
                    
                    if (bgTex == null)
                    {
                        LDtkDebug.LogError("CreateAsset srcTex was null, not making tile");
                        continue;
                    }
                    
                    LDtkBackgroundArtifactFactory bgFactory = CreateBackgroundFactory(bgTex, level, _importer.PixelsPerUnit);
                    Sprite bg = bgFactory?.CreateBackground();
                    backgrounds.Add(bg);
                }
            }

            return backgrounds;
        }

        private bool ValidateTextureWithTilesetDef(Level level, Texture2D bgTex)
        {
            // todo we cannot preemptively test the image's dimensions. wait until this value is available
            return true;
        }
    }
}