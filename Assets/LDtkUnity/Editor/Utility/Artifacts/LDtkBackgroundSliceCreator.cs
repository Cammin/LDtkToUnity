using System;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal class LDtkBackgroundSliceCreator
    {
        
        private readonly int _pixelsPerUnit;
        private readonly LDtkArtifactAssets _artifacts;
        private readonly AssetImportContext _ctx;
        
        private LDtkLoadedBackgroundDict _dict;
        private readonly HashSet<string> _uniqueSprites = new HashSet<string>();
        private readonly List<Action> _backgroundActions = new List<Action>();

        public void CreateAllArtifacts(LdtkJson json)
        {
            //cache every possible artifact in the project. All tileset definitions.
            //this would be all tiles, all sprites, and the background texture.

            Profiler.BeginSample("TextureDict.LoadAllProjectTextures");
            LoadAllProjectTextures(json); //loads all tileset textures and ALSO Level backgrounds!
            Profiler.EndSample();

            Profiler.BeginSample("SetupAllBackgroundSlices");
            SetupAllBackgroundSlices(json);
            Profiler.EndSample();

            Profiler.BeginSample($"BackgroundActions {_backgroundActions.Count}");
            BackgroundActions();
            Profiler.EndSample();
            
            //sprites, THEN tiles. tiles depend on sprites first
            /*Profiler.BeginSample($"SpriteActions {_spriteActions.Count}");
            SpriteActions();
            Profiler.EndSample();*/
            
            
        }
        
        private LDtkBackgroundArtifactFactory CreateBackgroundFactory(Texture2D srcTex, Level level, int pixelsPerUnit)
        {
            string assetName = LDtkKeyFormatUtil.GetCreateSpriteOrTileAssetName(level.BgPos.UnityCropRect, srcTex);
            if (!_uniqueSprites.Add(assetName))
            {
                return null;
            }
            return new LDtkBackgroundArtifactFactory(_ctx, _artifacts, assetName, srcTex, pixelsPerUnit, level);
        }
        
        private void SetupAllBackgroundSlices(LdtkJson json)
        {
            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels)
                {
                    //if no bg pos defined, then no background was set.
                    if (level.BgPos == null)
                    {
                        continue;
                    }
                    
                    Texture2D bgTex = _dict.Get(level.BgRelPath);
                    if (bgTex == null)
                    {
                        continue;
                    }
                    
                    //if (!ValidateTextureWithTilesetDef(def, texAsset)) //todo we cannot preemptively test the image's dimensions. wait until this value is available
                    //{
                    //continue;
                    //}
                    
                    SetupBackgroundSlices(level, bgTex);
                }
            }
        }
        private void SetupBackgroundSlices(Level level, Texture2D texAsset)
        {
            Level lvl = level;
            Texture2D tex = texAsset;
            _backgroundActions.Add(() => CreateLevelBackground(tex, lvl, _pixelsPerUnit));
        }
        
        /// <summary>
        /// Creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        private void CreateLevelBackground(Texture2D srcTex, Level lvl, int pixelsPerUnit)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return;
            }
            if (srcTex == null)
            {
                LDtkDebug.LogError("CreateAsset srcTex was null, not making tile");
                return;
            }

            LDtkBackgroundArtifactFactory bgFactory = CreateBackgroundFactory(srcTex, lvl, pixelsPerUnit);
            bgFactory?.TryCreateBackground(); //this can be null in case there were duplicate assets being made
        }
        
        private void BackgroundActions()
        {
            for (int i = 0; i < _backgroundActions.Count; i++)
            {
                _backgroundActions[i].Invoke();
            }
        }
        
        
        private void LoadAllProjectTextures(LdtkJson json)
        {
            _dict = new LDtkLoadedBackgroundDict(_ctx.assetPath);
            _dict.LoadAll(json);
        }
        
        /// <summary>
        /// Creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        private void CreateSprite(Texture2D srcTex, Rect srcPos, int pixelsPerUnit)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return;
            }
            if (srcTex == null)
            {
                LDtkDebug.LogError("CreateAsset srcTex was null, not making tile");
                return;
            }
            LDtkSpriteArtifactFactory spriteFactory = CreateSpriteFactory(srcTex, srcPos, pixelsPerUnit);
            spriteFactory?.TryCreateSprite();
        }

        private LDtkSpriteArtifactFactory CreateSpriteFactory(Texture2D srcTex, Rect srcPos, int pixelsPerUnit)
        {
            string assetName = LDtkKeyFormatUtil.GetCreateSpriteOrTileAssetName(srcPos, srcTex);
            if (!_uniqueSprites.Add(assetName))
            {
                return null;
            }
            return new LDtkSpriteArtifactFactory(_ctx, _artifacts, srcTex, srcPos, pixelsPerUnit, assetName);
        }
    }
}