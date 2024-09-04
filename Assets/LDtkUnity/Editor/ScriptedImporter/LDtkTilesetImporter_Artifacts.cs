using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    /// <summary>
    /// After the texture is generated, read that data in the output and add to an artifacts asset
    /// </summary>
    internal sealed partial class LDtkTilesetImporter
    {
        private LDtkArtifactAssetsTileset MakeAndCacheArtifacts(TextureGenerationOutput output)
        {
            LDtkArtifactAssetsTileset artifacts = ScriptableObject.CreateInstance<LDtkArtifactAssetsTileset>();
            artifacts.name = $"_{_definition.Def.Identifier}_Artifacts";
            
            LDtkProfiler.BeginSample("InitLists");
            artifacts._sprites = new Sprite[_sprites.Count];
            artifacts._tiles = new LDtkTilesetTile[_sprites.Count];
            artifacts._additionalSprites = new Sprite[_additionalTiles.Count];
            artifacts._overrideTextureMultiplier = _overrideTextureMultiplier;
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("CustomDataToDictionary");
            var customData = _definition.Def.CustomDataToDictionary();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("EnumTagsToDictionary");
            var enumTags = _definition.Def.EnumTagsToDictionary();
            LDtkProfiler.EndSample();
            
            //The challenge here is that we have a full list of tiles, but only some of them are valid.
            //We need to iterate all of them to get the sprites, but only create tiles for the valid ones
            //We have the validIDs array to know which are valid
            LDtkProfiler.BeginSample("IterateSpriteOutput");
            for (int i = 0; i < _sprites.Count; i++)
            {
                //always generate a tile, but sometimes we will not want to generate a sprite
                LDtkProfiler.BeginSample("CreateLDtkTilesetTile");
                LDtkTilesetTile newTilesetTile = ScriptableObject.CreateInstance<LDtkTilesetTile>();
                newTilesetTile.name = _sprites[i].name;
                newTilesetTile._tileId = i;
                newTilesetTile.hideFlags = HideFlags.HideInHierarchy;
                if (customData.TryGetValue(i, out string cd))
                {
                    newTilesetTile._customData = cd;
                }
                if (enumTags.TryGetValue(i, out List<string> et))
                {
                    newTilesetTile._enumTagValues = et;
                }
                LDtkProfiler.EndSample();
                
                LDtkProfiler.BeginSample("AddTileToAsset");
                ImportContext.AddObjectToAsset(newTilesetTile.name, newTilesetTile);
                artifacts._tiles[i] = newTilesetTile;
                LDtkProfiler.EndSample();
                
                //sometimes a sprite is not generates if it's only thin air
                if (!_validIds[i])
                {
                    continue;
                }
                
                int outputIndex = spriteIndexToOutputIndex[i];
                Sprite spr = output.sprites[outputIndex];
                //No hideFlags so sprites can be added to a sprite atlas
                
                LDtkProfiler.BeginSample("AddOffsetToPhysicsShape");
                AddOffsetToPhysicsShape(spr, i);
                LDtkProfiler.EndSample();
                
                LDtkProfiler.BeginSample("AddSpriteInfoToTile");
                newTilesetTile._sprite = spr;
                newTilesetTile._type = GetColliderTypeForSprite(spr);
                LDtkProfiler.EndSample();
                
                LDtkProfiler.BeginSample("AddSpriteToAsset");
                ImportContext.AddObjectToAsset(spr.name, spr);
                artifacts._sprites[i] = spr;
                LDtkProfiler.EndSample();
            }
            LDtkProfiler.EndSample();
            
            
            LDtkProfiler.BeginSample("IterateAdditionalSprites");
            for (int i = 0; i < _additionalTiles.Count; i++)
            {
                LDtkProfiler.BeginSample("AddSpriteToAsset");
                Sprite spr = output.sprites[_validSpritesCount + i];
                //need to reveal sprites in the hierarchy, so they can be added to a sprite atlas
                //spr.hideFlags = HideFlags.HideInHierarchy;
                ImportContext.AddObjectToAsset(spr.name, spr);
                LDtkProfiler.EndSample();
                
                LDtkProfiler.BeginSample("AddAdditionalSprite");
                artifacts._additionalSprites[i] = spr;
                LDtkProfiler.EndSample();
            }
            LDtkProfiler.EndSample();

            //don't need this anymore after we're done using it to check the generation result
            _validIds.Dispose();
            
            LDtkProfiler.BeginSample("TryParseCustomData");
            //process these after all the tiles are created because we might reference other tiles for animation
            foreach (var tile in artifacts._tiles)
            {
                TryParseCustomData(artifacts._sprites, tile);
            }
            LDtkProfiler.EndSample();

            
            
            return artifacts;
        }
        
        //todo really look at this function and understand if it's truly nessesary.
        //experimnent with using it on and off and checking how builds behave as a result.
        //will they log like this: https://forum.unity.com/threads/sprite-outline-generation-failed-could-not-read-texture-pixel-data-when-building-the-game.861775/
        private void AddOffsetToPhysicsShape(Sprite spr, int tileId)
        {
            if (spr == null)
            {
                return;
            }
            
            LDtkProfiler.BeginSample("GetSpriteData");
            LDtkSpriteRect spriteData = _sprites[tileId];
            //LDtkSpriteRect spriteData = GetSpriteData(spr.name);
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("GetOutlines");
            List<Vector2[]> srcShapes = spriteData.GetOutlines();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("MakeNewShapes");
            List<Vector2[]> newShapes = new List<Vector2[]>();
            foreach (Vector2[] srcOutline in srcShapes)
            {
                Vector2[] newOutline = new Vector2[srcOutline.Length];
                for (int ii = 0; ii < srcOutline.Length; ii++)
                {
                    Vector2 point = srcOutline[ii];
                    point += spr.rect.size * 0.5f;
                    newOutline[ii] = point;
                }
                newShapes.Add(newOutline);
            }
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("OverridePhysicsShape");
            spr.OverridePhysicsShape(newShapes);
            LDtkProfiler.EndSample();
        }
        
        Tile.ColliderType GetColliderTypeForSprite(Sprite spr)
        {
            int shapeCount = spr.GetPhysicsShapeCount();
            if (shapeCount == 0)
            {
                return Tile.ColliderType.None;
            }
            if (shapeCount == 1)
            {
                List<Vector2> list = new List<Vector2>();
                spr.GetPhysicsShape(0, list);
                if (IsShapeSetForGrid(list))
                {
                    return Tile.ColliderType.Grid;
                }
            }
            return Tile.ColliderType.Sprite;
        }
        private static Vector2 GridCheck1 = new Vector2(-0.5f, -0.5f);
        private static Vector2 GridCheck2 = new Vector2(-0.5f, 0.5f);
        private static Vector2 GridCheck3 = new Vector2(0.5f, 0.5f);
        private static Vector2 GridCheck4 = new Vector2(0.5f, -0.5f);
        public static bool IsShapeSetForGrid(List<Vector2> shape)
        {
            return shape.Count == 4 &&
                   shape.Any(p => p == GridCheck1) &&
                   shape.Any(p => p == GridCheck2) &&
                   shape.Any(p => p == GridCheck3) &&
                   shape.Any(p => p == GridCheck4);
        }
        
        private void TryParseCustomData(Sprite[] sprites, LDtkTilesetTile tile)
        {
            string customData = tile._customData;
            if (customData.IsNullOrEmpty())
            {
                return;
            }
            
            string[] lines = customData.Split('\n');
            foreach (string line in lines)
            {
                //animatedSprites
                {
                    if (ParseAndGetTokensAsInt(tile, line, "animatedSprites", out int[] spriteIdTokens))
                    {
                        tile._animatedSprites = new Sprite[spriteIdTokens.Length];
                        for (int i = 0; i < spriteIdTokens.Length; i++)
                        {
                            int spriteId = spriteIdTokens[i];

                            if (spriteId < 0 || spriteId >= sprites.Length)
                            {
                                Logger.LogWarning($"Issue parsing animatedSprites for tile \"{tile.name}\". Tile ID {spriteId} is out of range");
                                continue;
                            }
                            
                            tile._animatedSprites[i] = sprites[spriteId];
                        }
                        continue;
                    }
                }

                //animationSpeed
                {
                    if (ParseAndGetTokensAsFloat(tile, line, "animationSpeed", out float[] speedTokens))
                    {
                        if (speedTokens.Length == 1)
                        {
                            tile._animationSpeedMin = speedTokens[0];
                            tile._animationSpeedMax = speedTokens[0];
                            continue;
                        }
                    
                        if (speedTokens.Length == 2)
                        {
                            tile._animationSpeedMin = speedTokens[0];
                            tile._animationSpeedMax = speedTokens[1];
                            continue;
                        }
                    
                        Logger.LogWarning($"Issue parsing animationSpeed for tile \"{tile.name}\". Expected 1 or 2 decimal numbers but there were {speedTokens.Length}");
                        continue;
                    }
                }

                //animationStartTime
                {
                    if (ParseAndGetTokensAsFloat(tile, line, "animationStartTime", out float[] startTimeTokens))
                    {
                        if (startTimeTokens.Length == 1)
                        {
                            tile._animationStartTimeMin = startTimeTokens[0];
                            tile._animationStartTimeMax = startTimeTokens[0];
                            continue;
                        }
                    
                        if (startTimeTokens.Length == 2)
                        {
                            tile._animationStartTimeMin = startTimeTokens[0];
                            tile._animationStartTimeMax = startTimeTokens[1];
                            continue;
                        }
                    
                        Logger.LogWarning($"Issue parsing animationStartTime for tile \"{tile.name}\". Expected 1 or 2 decimal numbers but there were {startTimeTokens.Length}");
                        continue;
                    }
                }

                //animationStartFrame
                {
                    if (ParseAndGetTokensAsInt(tile, line, "animationStartFrame", out int[] startFrameTokens))
                    {
                        if (startFrameTokens.Length == 1)
                        {
                            tile._animationStartFrameMin = startFrameTokens[0];
                            tile._animationStartFrameMax = startFrameTokens[0];
                            continue;
                        }
                    
                        if (startFrameTokens.Length == 2)
                        {
                            tile._animationStartFrameMin = startFrameTokens[0];
                            tile._animationStartFrameMax = startFrameTokens[1];
                            continue;
                        }
                    
                        Logger.LogWarning($"Issue parsing animationStartFrame for tile \"{tile.name}\". Expected 1 or 2 ints but there were {startFrameTokens.Length}");
                        continue;
                    }
                }
            }
        }

        private bool ParseAndGetTokensAsInt(LDtkTilesetTile tile, string line, string keyword, out int[] tokens)
        {
            if (!ParseAndGetTokensAsString(line, keyword, out string[] tokenStrings))
            {
                tokens = null;
                return false;
            }
            
            tokens = new int[tokenStrings.Length];
            for (int i = 0; i < tokenStrings.Length; i++)
            {
                string tokenString = tokenStrings[i];
                if (int.TryParse(tokenString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intVaue))
                {
                    tokens[i] = intVaue;
                    continue;
                }
                Logger.LogWarning($"Issue parsing \"{keyword}\"'s token {i} for tile \"{tile.name}\". Couldn't parse into int: \"{tokenString}\"");
            }
            return true;
        }

        private bool ParseAndGetTokensAsFloat(LDtkTilesetTile tile, string line, string keyword, out float[] tokens)
        {
            if (!ParseAndGetTokensAsString(line, keyword, out string[] tokenStrings))
            {
                tokens = null;
                return false;
            }
            
            tokens = new float[tokenStrings.Length];
            for (int i = 0; i < tokenStrings.Length; i++)
            {
                string tokenString = tokenStrings[i];
                if (float.TryParse(tokenString, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue))
                {
                    tokens[i] = floatValue;
                    continue;
                }
                Logger.LogWarning($"Issue parsing \"{keyword}\"'s token for tile \"{tile.name}\". Couldn't parse into float: \"{tokenString}\"");
            }
            return true;
        }

        private bool ParseAndGetTokensAsString(string line, string keyword, out string[] tokens)
        {
            if (!line.StartsWith(keyword))
            {
                tokens = null;
                return false;
            }
            
            string strippedOfKeyword = line.Replace(keyword, "");
            tokens = strippedOfKeyword.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = tokens[i].Trim();
            }

            return true;
        }
        
        public LDtkArtifactAssetsTileset LoadArtifacts(LDtkDebugInstance projectCtx)
        {
            if (_artifacts)
            {
                return _artifacts;
            }
            
            LDtkProfiler.BeginSample($"LoadMainAssetAtPath<LDtkArtifactAssetsTileset> {AssetName}");
            _artifacts = AssetDatabase.LoadAssetAtPath<LDtkArtifactAssetsTileset>(assetPath);
            LDtkProfiler.EndSample();
            
            //It's possible that the artifact assets don't exist, either because the texture importer failed to import, or the artifact assets weren't produced due to being an aseprite file or otherwise
            if (_artifacts == null)
            {
                LDtkDebug.LogError($"Loading artifacts didn't work for getting tileset sprite artifacts. You should investigate the tileset file at \"{assetPath}\"", projectCtx);
                return null;
            }
            
            return _artifacts;
        }
    }
}