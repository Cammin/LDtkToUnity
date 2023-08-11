using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal static class TextureGeneration
    {
        public static TextureGenerationOutput Generate(AssetImportContext ctx,
            NativeArray<Color32> imageData,
            int textureWidth,
            int textureHeight,
            LDtkSpriteRect[] sprites,
            //in List<TextureImporterPlatformSettings> allPlatformSettings,
            TextureImporterPlatformSettings platformSettings,
            in TextureImporterSettings textureImporterSettings,
            string spritePackingTag, 
            SecondarySpriteTexture[] secondarySpriteTextures
        )
        {
            if (!imageData.IsCreated || imageData.Length == 0)
                return new TextureGenerationOutput();

            var output = new TextureGenerationOutput();
            UnityEngine.Profiling.Profiler.BeginSample("ImportTexture");
            try
            {
                //var platformSettings = TextureImporterUtilities.GetPlatformTextureSettings(ctx.selectedBuildTarget, in allPlatformSettings);

                var textureSettings = textureImporterSettings.ExtractTextureSettings();
                textureSettings.assetPath = ctx.assetPath;
                textureSettings.enablePostProcessor = true;
                textureSettings.containsAlpha = true;
                textureSettings.hdr = false;

                var textureAlphaSettings = textureImporterSettings.ExtractTextureAlphaSettings();
                var textureMipmapSettings = textureImporterSettings.ExtractTextureMipmapSettings();
                var textureCubemapSettings = textureImporterSettings.ExtractTextureCubemapSettings();
                var textureWrapSettings = textureImporterSettings.ExtractTextureWrapSettings();
                
                switch (textureImporterSettings.textureType)
                {
                    case TextureImporterType.Default:
                        output = TextureGeneratorHelper.GenerateTextureDefault(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureAlphaSettings, textureMipmapSettings, textureCubemapSettings, textureWrapSettings);
                        break;
                    case TextureImporterType.NormalMap:
                        var textureNormalSettings = textureImporterSettings.ExtractTextureNormalSettings();
                        output = TextureGeneratorHelper.GenerateNormalMap(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureNormalSettings, textureMipmapSettings, textureCubemapSettings, textureWrapSettings);
                        break;
                    case TextureImporterType.GUI:
                        output = TextureGeneratorHelper.GenerateTextureGUI(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureAlphaSettings, textureMipmapSettings, textureWrapSettings);
                        break;
                    case TextureImporterType.Sprite:
                        var textureSpriteSettings = textureImporterSettings.ExtractTextureSpriteSettings();
                        textureSpriteSettings.packingTag = spritePackingTag;
                        textureSpriteSettings.qualifyForPacking = !string.IsNullOrEmpty(spritePackingTag);
                        textureSpriteSettings.spriteSheetData = new SpriteImportData[sprites.Length];
                        textureSettings.npotScale = TextureImporterNPOTScale.None;
                        textureSettings.secondaryTextures = secondarySpriteTextures;
                        
                        for (var i = 0; i < sprites.Length; ++i)
                            textureSpriteSettings.spriteSheetData[i] = ConvertFromSpriteRect(sprites[i]);
                        
                        output = TextureGeneratorHelper.GenerateTextureSprite(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureSpriteSettings, textureAlphaSettings, textureMipmapSettings, textureWrapSettings);

                        break;
                    case TextureImporterType.Cursor:
                        output = TextureGeneratorHelper.GenerateTextureCursor(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureAlphaSettings, textureMipmapSettings, textureWrapSettings);
                        break;
                    case TextureImporterType.Cookie:
                        output = TextureGeneratorHelper.GenerateCookie(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureAlphaSettings, textureMipmapSettings, textureCubemapSettings, textureWrapSettings);
                        break;
                    case TextureImporterType.Lightmap:
                        output = TextureGeneratorHelper.GenerateLightmap(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureMipmapSettings, textureWrapSettings);
                        break;
                    case TextureImporterType.SingleChannel:
                        output = TextureGeneratorHelper.GenerateTextureSingleChannel(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureAlphaSettings, textureMipmapSettings, textureCubemapSettings, textureWrapSettings);
                        break;
                    default:
                        Debug.LogAssertion("Unknown texture type for import");
                        output = default(TextureGenerationOutput);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to generate Texture2D. Possibly texture size is too big to be generated. Error: {e}", ctx.mainObject);
            }
            finally
            {
                UnityEngine.Profiling.Profiler.EndSample();    
            }
            
            return output;
        }
        
        public static SpriteImportData ConvertFromSpriteRect(SpriteRect value)
        {
            var output = new SpriteImportData
            {
                name = value.name,
                alignment = value.alignment,
                rect = value.rect,
                border = value.border,
                pivot = value.pivot,
                spriteID = value.spriteID.ToString()
            };
            //output.tessellationDetail = value.tessellationDetail;
            /*if (value.spriteOutline != null)
                output.outline = value.spriteOutline.Select(x => x.outline).ToList();*/

            return output;
        }
    }
}
    /*internal static class TextureImporterUtilities
    {
        public static TextureImporterPlatformSettings GetPlatformTextureSettings(BuildTarget buildTarget, in List<TextureImporterPlatformSettings> platformSettings)
        {
            var buildTargetName = TexturePlatformSettingsHelper.GetBuildTargetGroupName(buildTarget);
            TextureImporterPlatformSettings settings = null;
            settings = platformSettings.SingleOrDefault(x => x.name == buildTargetName && x.overridden == true);
            settings = settings ?? platformSettings.SingleOrDefault(x => x.name == TexturePlatformSettingsHelper.defaultPlatformName);

            if (settings == null)
            {
                settings = new TextureImporterPlatformSettings();
                settings.name = buildTargetName;
                settings.overridden = false;
                UpdateWithDefaultSettings(ref settings);
            }
            return settings;
        }

        public static void UpdateWithDefaultSettings(ref TextureImporterPlatformSettings platformSettings)
        {
            platformSettings.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }*/
