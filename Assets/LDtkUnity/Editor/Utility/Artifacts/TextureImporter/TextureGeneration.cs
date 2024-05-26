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
            SpriteImportData[] sprites,
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
            LDtkProfiler.BeginSample("ImportTexture");
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
                var textureWrapSettings = textureImporterSettings.ExtractTextureWrapSettings();
                
                Debug.Assert(textureImporterSettings.textureType == TextureImporterType.Sprite, "Texture type must be Sprite");
                
                var textureSpriteSettings = textureImporterSettings.ExtractTextureSpriteSettings();
                textureSpriteSettings.packingTag = spritePackingTag;
                textureSpriteSettings.qualifyForPacking = !string.IsNullOrEmpty(spritePackingTag);
                textureSpriteSettings.spriteSheetData = new SpriteImportData[sprites.Length];
                textureSettings.npotScale = TextureImporterNPOTScale.None;
                textureSettings.secondaryTextures = secondarySpriteTextures;
                textureSpriteSettings.spriteSheetData = sprites;
                output = TextureGeneratorHelper.GenerateTextureSprite(imageData, textureWidth, textureHeight, textureSettings, platformSettings, textureSpriteSettings, textureAlphaSettings, textureMipmapSettings, textureWrapSettings);
           
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to generate Texture2D. Possibly texture size is too big to be generated. Error: {e}", ctx.mainObject);
            }
            finally
            {
                LDtkProfiler.EndSample();    
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
