using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal sealed partial class LDtkTilesetImporter :
        ISpriteEditorDataProvider, 
        ITextureDataProvider, 
        ISpritePhysicsOutlineDataProvider, 
        ISecondaryTextureDataProvider
    {
        SpriteImportMode ISpriteEditorDataProvider.spriteImportMode => SpriteImportMode.Multiple;
        float ISpriteEditorDataProvider.pixelsPerUnit => _pixelsPerUnit;
        Object ISpriteEditorDataProvider.targetObject => this;

        SpriteRect[] ISpriteEditorDataProvider.GetSpriteRects()
        {
            ForceUpdateSpriteDataNames();
            return _sprites.Select(x => new LDtkSpriteRect(x) as SpriteRect).ToArray();
        }

        void ISpriteEditorDataProvider.SetSpriteRects(SpriteRect[] spriteRects)
        {
            //remove those that have become null or otherwise deleted and/or irrelevant
            _sprites.RemoveAll(data => spriteRects.FirstOrDefault(x => x.spriteID == data.spriteID) == null);
            foreach (var sr in spriteRects)
            {
                ForceUpdateSpriteDataName(sr);
                
                LDtkSpriteRect importData = _sprites.FirstOrDefault(x => x.spriteID == sr.spriteID);
                if (importData == null)
                {
                    _sprites.Add(new LDtkSpriteRect(sr));
                }
                else
                {
                    importData.name = sr.name;
                    importData.alignment = sr.alignment;
                    importData.border = sr.border;
                    importData.pivot = sr.pivot;
                    importData.rect = sr.rect;
                }
            }
        }

        void ISpriteEditorDataProvider.Apply()
        {
            // Do this so that asset change save dialog will not show
            var originalValue = EditorPrefs.GetBool("VerifySavingAssets", false);
            EditorPrefs.SetBool("VerifySavingAssets", false);
            AssetDatabase.ForceReserializeAssets(new string[] { assetPath }, ForceReserializeAssetsOptions.ReserializeMetadata);
            EditorPrefs.SetBool("VerifySavingAssets", originalValue);
        }

        void ISpriteEditorDataProvider.InitSpriteEditorDataProvider() 
        { }

        T ISpriteEditorDataProvider.GetDataProvider<T>() => 
            this as T;

        bool ISpriteEditorDataProvider.HasDataProvider(Type type) => 
            type.IsAssignableFrom(GetType());

        void ITextureDataProvider.GetTextureActualWidthAndHeight(out int width, out int height)
        {
            width = LoadTex().width;
            height = LoadTex().height;
        }

        Texture2D ITextureDataProvider.GetReadableTexture2D() => LoadTex();
        Texture2D ITextureDataProvider.texture => LoadTex();
        Texture2D ITextureDataProvider.previewTexture => LoadTex();

        
        List<Vector2[]> ISpritePhysicsOutlineDataProvider.GetOutlines(GUID guid) =>
            GetSpriteData(guid).GetOutlines();

        void ISpritePhysicsOutlineDataProvider.SetOutlines(GUID guid, List<Vector2[]> data) =>
            GetSpriteData(guid).SetOutlines(data);

        float ISpritePhysicsOutlineDataProvider.GetTessellationDetail(GUID guid) => 
            GetSpriteData(guid).tessellationDetail;

        void ISpritePhysicsOutlineDataProvider.SetTessellationDetail(GUID guid, float value) => 
            GetSpriteData(guid).tessellationDetail = value;

        SecondarySpriteTexture[] ISecondaryTextureDataProvider.textures
        {
            get => _secondaryTextures;
            set => _secondaryTextures = value;
        }
        
        /*
     *
     * byte[] bytes = File.ReadAllBytes(texturePath);
        NativeArray<byte> native = new NativeArray<byte>(bytes, Allocator.Temp);

        var encoded = ImageConversion.EncodeNativeArrayToPNG(native, GraphicsFormat.R8G8B8A8_UNorm, width, height);
        NativeArray<Color32> bytesToColors = ByteToColorArray(native);

        static NativeArray<Color32> ByteToColorArray(in NativeArray<byte> data)
        {
            NativeArray<Color32> image = new NativeArray<Color32>(data.Length / 4, Allocator.Persistent);
            for (var i = 0; i < image.Length; ++i)
            {
                var dataIndex = i * 4;
                image[i] = new Color32(
                    data[dataIndex],
                    data[dataIndex + 1],
                    data[dataIndex + 2],
                    data[dataIndex + 3]);
            }
            return image;
        }

        Debug.Log($"native: {native.Length}");
        Debug.Log($"encoded: {encoded.Length}");
        Debug.Log($"bytesToColors: {bytesToColors.Length}");
        Debug.Log($"expect: {width*height}");
     */

    }
}