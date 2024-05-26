using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //this takes the sprite rects, and based on that, generates a texture with sprites
    internal sealed partial class LDtkTilesetImporter
    {
        private int _validSpritesCount;
        private NativeArray<bool> _validIds;
        
        //generation result index to the _sprites index we actually want.
        private Dictionary<int, int> spriteIndexToOutputIndex;
        
        private bool PrepareAndGenerateTexture(TextureImporterPlatformSettings platformSettings, out TextureGenerationOutput output)
        {
            Debug.Assert(_pixelsPerUnit > 0, $"_pixelsPerUnit was {_pixelsPerUnit}");
            
            Texture2D copy = LoadTextureAndMakeCopy();
            TextureImporterSettings importerSettings = GetTextureImporterSettings();

            if (importerSettings.textureType != TextureImporterType.Sprite)
            {
                Logger.LogError($"Didn't generate the texture and sprites for \"{AssetName}\" because the source texture's TextureType is not \"Sprite\".");
                return false;
            }
            
            platformSettings.format = TextureImporterFormat.RGBA32;
            importerSettings.spritePixelsPerUnit = _pixelsPerUnit;
            importerSettings.filterMode = FilterMode.Point;

            LDtkProfiler.BeginSample("GetRawTextureData");
            NativeArray<Color32> pixels = copy.GetRawTextureData<Color32>();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("DetectUsedTilePixels constructor");
            DetectUsedTilePixels job = new DetectUsedTilePixels(_json, ref pixels);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("DetectUsedTilePixels.ScheduleParallel");
            int tilesLength = _json.CWid * _json.CHei;
            int innerLoopBatchCount = Mathf.Max(1, (tilesLength / System.Environment.ProcessorCount) + 1);
            JobHandle handle = job.ScheduleParallel(tilesLength, innerLoopBatchCount, default);
            JobHandle.ScheduleBatchedJobs();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("handle.Complete");
            handle.Complete();
            LDtkProfiler.EndSample();

            job.Pixels.Dispose();
            _validIds = job.TileIdsWithPixels;
            
            //GOAL: prepare only the sprites that matter for the texture generation
            _validSpritesCount = job.TileIdsWithPixels.Count(p => p);
            
            //Figure out which tiles should be built. This can be cached to track it after the texture generation is done
            spriteIndexToOutputIndex = new Dictionary<int, int>(_validSpritesCount);
            List<LDtkSpriteRect> validRects = new List<LDtkSpriteRect>(_validSpritesCount);
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_validIds[i])
                {
                    spriteIndexToOutputIndex.Add(i, validRects.Count);
                    validRects.Add(_sprites[i]);
                }
            }
            
            //todo this could be added to the job system
            SpriteImportData[] sprites = new SpriteImportData[validRects.Count + _additionalTiles.Count];
            
            //add usual tiles
            for (int i = 0; i < validRects.Count; i++)
            {
                LDtkSpriteRect rect = validRects[i];
                sprites[i] = TextureGeneration.ConvertFromSpriteRect(rect);
            }
            //add additional tiles
            for (int i = 0; i < _additionalTiles.Count; i++)
            {
                LDtkSpriteRect rect = _additionalTiles[i];
                int farI = validRects.Count + i;
                sprites[farI] = TextureGeneration.ConvertFromSpriteRect(rect);
            }
            
            LDtkProfiler.BeginSample("TextureGeneration.Generate");
            output = TextureGeneration.Generate(
                ImportContext, pixels, copy.width, copy.height, sprites,
                platformSettings, importerSettings, string.Empty, _secondaryTextures);
            LDtkProfiler.EndSample();
            
            return true;
        }
        
        private Texture2D LoadTextureAndMakeCopy()
        {
            //the reason to make a copy is that ReadRawPixels doesn't work otherwise, is a tricky procedure
            Texture2D copy;
            
#if LDTK_UNITY_ASEPRITE && UNITY_2021_3_OR_NEWER
            if (_srcAsepriteImporter)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(PathToTexture(assetPath));
                if (sprite == null)
                {
                    Logger.LogError($"Failed to load the aseprite sprite for \"{AssetName}\". Either the Aseprite file failed to import, or the aseprite file's import settings are configured to not generate a sprite.");
                    return null;
                }
                
                LDtkProfiler.BeginSample("GenerateAsepriteTexture");
                copy = GenerateTextureFromAseprite(sprite);
                LDtkProfiler.EndSample();
            }
            else
#endif
            {
                LDtkProfiler.BeginSample("LoadExternalTex");
                Texture2D srcTex = LoadExternalTex();
                LDtkProfiler.EndSample();
                
                LDtkProfiler.BeginSample("CopyTexture");
                copy = new Texture2D(srcTex.width, srcTex.height, TextureFormat.RGBA32, false, false);
                Graphics.CopyTexture(srcTex, copy);
                LDtkProfiler.EndSample();
            }

            return copy;
        }
        
        private Texture2D LoadExternalTex(bool forceLoad = false)
        {
            //this is important: in case the importer was destroyed via file delete
            if (this == null)
            {
                return null;
            }
            
            if (_cachedExternalTex == null || forceLoad)
            {
                _cachedExternalTex = AssetDatabase.LoadAssetAtPath<Texture2D>(PathToTexture(assetPath));
            }
            return _cachedExternalTex;
        }
        
        private Texture2D GenerateTextureFromAseprite(Sprite sprite)
        {
            Texture2D croppedTexture = new Texture2D(_json.PxWid, _json.PxHei, TextureFormat.RGBA32, false, false);

            Color32[] colors = new Color32[_json.PxWid * _json.PxHei];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color32(0, 0, 0, 0);
            }
            croppedTexture.SetPixels32(colors);

            Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, 
                (int)sprite.textureRect.y, 
                (int)sprite.textureRect.width, 
                (int)sprite.textureRect.height);
            croppedTexture.SetPixels(0 ,_json.PxHei - (int)sprite.rect.height, (int)sprite.rect.width, (int)sprite.rect.height, pixels);
            
            croppedTexture.Apply(false, false);

            return croppedTexture;
        }
        
        private static readonly int[] MaxSizes = new[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        private bool HasTextureIssue(TextureImporterPlatformSettings platformSettings)
        {
#if LDTK_UNITY_ASEPRITE && UNITY_2021_3_OR_NEWER
            AssetImporter importer = _srcAsepriteImporter != null ? (AssetImporter)_srcAsepriteImporter : _srcTextureImporter; 
#else
            AssetImporter importer = _srcTextureImporter;
#endif
            
            bool issue = false;

            // need proper resolution
            if (platformSettings.maxTextureSize < _json.PxWid || platformSettings.maxTextureSize < _json.PxHei)
            {
                int highest = Mathf.Max(_json.PxWid, _json.PxHei);

                int resolution = 16384;
                for (int i = 0; i < MaxSizes.Length; i++)
                {
                    int size = MaxSizes[i];
                    if (highest <= size)
                    {
                        resolution = size;
                        break;
                    }
                }

                issue = true;
                Logger.LogError($"The texture at \"{importer.assetPath}\" maxTextureSize needs to at least be {resolution}.\n(From {assetPath})", importer);
                //platformSettings.maxTextureSize = resolution;
            }

            //this is required or else the texture generator does not comply
            if (platformSettings.format != TextureImporterFormat.RGBA32)
            {
                issue = true;
                //platformSettings.format = TextureImporterFormat.RGBA32;
                Logger.LogError($"The texture at \"{importer.assetPath}\" needs to have a compression format of {TextureImporterFormat.RGBA32}\n(From {assetPath})", importer);
            }

            //need to read the texture to make our own texture generation result
            /*if (!textureImporter.isReadable)
            {
                issue = true;
                //textureImporter.isReadable = true;
                Logger.LogError($"The texture \"{textureImporter.assetPath}\" was not readable. Change it.", this);
            }*/

            
            return issue;
        }
    }
    
    /// <summary>
    /// Given an image's pixels, identify which cells are to be generated by checking what pixels are NOT clear.
    /// </summary>
    public struct DetectUsedTilePixels : IJobFor
    {
        [ReadOnly] public NativeArray<Color32> Pixels;
        [WriteOnly] public NativeArray<bool> TileIdsWithPixels;

        [ReadOnly] private readonly int _cWid;
        [ReadOnly] private readonly int _cHei;
        [ReadOnly] private readonly int _gridSize;
        [ReadOnly] private readonly int _padding;
        [ReadOnly] private readonly int _spacing;
        [ReadOnly] private readonly int _defPxWid;
        [ReadOnly] private readonly int _defPxHei;
        
        public DetectUsedTilePixels(TilesetDefinition def, ref NativeArray<Color32> pixels)
        {
            //input
            Pixels = pixels;
            _cWid = def.CWid;
            _cHei = def.CHei;
            _gridSize = def.TileGridSize;
            _padding = def.Padding;
            _spacing = def.Spacing;
            _defPxWid = def.PxWid;
            _defPxHei = def.PxHei;
            
            //output
            int size = def.CHei * def.CWid;
            TileIdsWithPixels = new NativeArray<bool>(size, Allocator.TempJob);
        }
            
        /// <summary>
        /// Each index corresponds to one tile.
        /// Figure out the cell coord, and check every pixel in it's rectangle.
        /// If any have an alpha that's not 0, it's a tile to build.
        /// </summary>
        public void Execute(int index)
        {
            int cX = index % _cWid;
            int cY = index / _cWid;
            
            cY = _cHei - cY - 1;
            
            //todo: double check if we try different gris size, spacing, and padding. might break on the Y.
            int rectXMin = cX * (_gridSize + _spacing) + _padding;
            int rectYMin = cY * (_gridSize + _spacing) + _padding;
            
            int rectWid = Mathf.Min(_gridSize, _defPxWid - rectXMin - _padding);
            int rectHei = Mathf.Min(_gridSize, _defPxHei - rectYMin - _padding);

            int rectXMax = rectXMin + rectWid;
            int rectYMax = rectYMin + rectHei;
            
            //check all pixels within the rect
            for (int y = rectYMin; y < rectYMax; y++)
            {
                int i = y * _defPxWid + rectXMin;
                for (int x = rectXMin; x < rectXMax; x++, i++)
                {
                    Color32 pixel = Pixels[i];
                    if (pixel.a != 0)
                    {
                        TileIdsWithPixels[index] = true;
                        return;
                    }
                }
            }
        }
    }
}