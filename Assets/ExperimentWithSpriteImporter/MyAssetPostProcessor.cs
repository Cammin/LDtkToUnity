using System;
using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Editor;
using Unity.Collections;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using Object = UnityEngine.Object;

/*namespace ExperimentWithSpriteImporter
{
    [ScriptedImporter(1, "thing")]
    public class MyAssetPostProcessor : ScriptedImporter, 
        ISpriteEditorDataProvider, 
        ITextureDataProvider, 
        ISpritePhysicsOutlineDataProvider, 
        ISecondaryTextureDataProvider
    {
        public const string texturePath = "Assets/Samples/Samples/atlas/Inca_front_by_Kronbits-extended.png";
        public const string textureName = "Inca_front_by_Kronbits-extended";
        public const int texWidth = 320;
        public const int texHeight = 224;
    
        public int ppu = 16;
        public FilterMode filterMode = FilterMode.Point;
        public List<LDtkSpriteRect> sprites = new List<LDtkSpriteRect>();
        public SecondarySpriteTexture[] secondaryTextures;
    
        private Texture2D tex;

    
        public SpriteImportMode spriteImportMode => SpriteImportMode.Multiple;
        public float pixelsPerUnit => ppu;
        public Object targetObject => this;

        private static string[] GatherDependenciesFromSourceFile(string path)
        {
            return new []{texturePath};
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (ppu < 1)
            {
                ppu = 1;
            }
        
            TextureImporter textureImporter = (TextureImporter)GetAtPath(texturePath);

            GameObject obj = new GameObject();
            ctx.AddObjectToAsset("obj", obj);
        
            //PLATFORM
            string platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            TextureImporterPlatformSettings platformSettings = textureImporter.GetPlatformTextureSettings(platform);
            if (!platformSettings.overridden)
            {
                platformSettings = textureImporter.GetDefaultPlatformTextureSettings();
            }

            if (CorrectTheTexture(textureImporter, platformSettings))
            {
                return;
            }

            //IMPORT
            TextureImporterSettings importerSettings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(importerSettings);
            importerSettings.spritePixelsPerUnit = ppu;
            importerSettings.filterMode = filterMode;
        
            NativeArray<Color32> rawData = LoadTex().GetRawTextureData<Color32>();
        
            TextureGenerationOutput output = TextureGeneration.Generate(
                ctx, rawData, texWidth, texHeight, sprites.ToArray(), 
                platformSettings, importerSettings, string.Empty, secondaryTextures);

            Texture outputTexture = output.output;
            if (outputTexture)
            {
                outputTexture.name = textureName;
                ctx.AddObjectToAsset("tex", outputTexture);
                GameObject child1 = new GameObject("child");
                child1.transform.parent = obj.transform;
                foreach (var spr in output.sprites)
                {
                    AddOffsetToPhysicsShape(spr);

                    ctx.AddObjectToAsset(spr.name, spr);
                
                    GameObject child = new GameObject(spr.name);
                    child.transform.parent = child1.transform;
                    child.transform.position = spr.rect.position / ppu;

                    SpriteRenderer render = child.AddComponent<SpriteRenderer>();
                    render.sprite = spr;

                    //it will set itself like the physics shape
                    PolygonCollider2D coll = child.AddComponent<PolygonCollider2D>();
                }

                /#1#/set back onto the serialized data in the importer
            for (int i = 0; i < output.sprites.Length; i++)
            {
                Sprite spr = output.sprites[i];
                sprites[i].name = spr.name;
            }#1#
            }
        
        
            ctx.SetMainObject(obj);
        }

        private void AddOffsetToPhysicsShape(Sprite spr)
        {
            List<Vector2[]> srcShapes = GetSpriteData(spr.name).GetOutlines();
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
            spr.OverridePhysicsShape(newShapes);
        }

        private void ForceUpdateSpriteDataNames()
        {
            foreach (LDtkSpriteRect spr in sprites)
            {
                ForceUpdateSpriteDataName(spr);
            }
        }

        private static void ForceUpdateSpriteDataName(SpriteRect spr)
        {
            spr.name = $"{textureName}_{spr.rect.x}_{spr.rect.y}_{spr.rect.width}_{spr.rect.height}";
        }

        private bool CorrectTheTexture(TextureImporter textureImporter, TextureImporterPlatformSettings platformSettings)
        {
            bool issue = false;

            if (platformSettings.maxTextureSize < texWidth || platformSettings.maxTextureSize < texHeight)
            {
                issue = true;
                platformSettings.maxTextureSize = 8192;
                Debug.Log($"The texture {textureImporter.assetPath} maxTextureSize was greater than it's resolution. This was automatically fixed.");
            }

            if (platformSettings.format != TextureImporterFormat.RGBA32)
            {
                issue = true;
                platformSettings.format = TextureImporterFormat.RGBA32;
                Debug.Log($"The texture {textureImporter.assetPath} format was not {TextureImporterFormat.RGBA32}. This was automatically fixed.");
            }

            if (!textureImporter.isReadable)
            {
                issue = true;
                textureImporter.isReadable = true;
                Debug.Log($"The texture {textureImporter.assetPath} was not readable. This was automatically fixed.");
            }

            if (!issue)
            {
                return false;
            }
        
            tex = null;
            textureImporter.SetPlatformTextureSettings(platformSettings);
            textureImporter.SaveAndReimport();
            return true;
        }


        public SpriteRect[] GetSpriteRects()
        {
            ForceUpdateSpriteDataNames();
            return sprites.Select(x => new LDtkSpriteRect(x) as SpriteRect).ToArray();
        }
        public void SetSpriteRects(SpriteRect[] spriteRects)
        {
            //var spriteImportData = sprites;
        
            //remove those that have become null or otherwise deleted and/or irrelevant
            sprites.RemoveAll(data => spriteRects.FirstOrDefault(x => x.spriteID == data.spriteID) == null);
            foreach (var sr in spriteRects)
            {
                ForceUpdateSpriteDataName(sr);
                
                var importData = sprites.FirstOrDefault(x => x.spriteID == sr.spriteID);
                if (importData == null)
                {
                    sprites.Add(new LDtkSpriteRect(sr));
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

        public void Apply()
        {
            //Debug.Log("On Apply");
            // Do this so that asset change save dialog will not show
            var originalValue = EditorPrefs.GetBool("VerifySavingAssets", false);
            EditorPrefs.SetBool("VerifySavingAssets", false);
            AssetDatabase.ForceReserializeAssets(new string[] { assetPath }, ForceReserializeAssetsOptions.ReserializeMetadata);
            EditorPrefs.SetBool("VerifySavingAssets", originalValue);
        }

        public void InitSpriteEditorDataProvider() { }

        public T GetDataProvider<T>() where T : class
        {
            return this as T;
        }

        public bool HasDataProvider(Type type)
        {
            return type.IsAssignableFrom(GetType());
        }

        #region TextureDataProvider
        public Texture2D LoadTex(bool forceLoad = false)
        {
            if (tex == null || forceLoad)
            {
                //AssetDatabase.Refresh();
                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            }
            Debug.Assert(tex);
            return tex;
        }

        void ITextureDataProvider.GetTextureActualWidthAndHeight(out int width, out int height)
        {
            width = LoadTex().width;
            height = LoadTex().height;
        }

        Texture2D ITextureDataProvider.GetReadableTexture2D() => LoadTex();
        Texture2D ITextureDataProvider.texture => LoadTex();
        Texture2D ITextureDataProvider.previewTexture => LoadTex();
        #endregion
        
        internal LDtkSpriteRect GetSpriteData(GUID guid)
        {
            LDtkSpriteRect data = sprites.FirstOrDefault(x => x.spriteID == guid);
            Debug.Assert(data != null, $"Sprite data not found for GUID:{guid.ToString()}");
            return data;
        }
        internal LDtkSpriteRect GetSpriteData(string name)
        {
            LDtkSpriteRect data = sprites.FirstOrDefault(x => x.name == name);
            Debug.Assert(data != null, $"Sprite data not found for name:{name.ToString()}");
            return data;
        }

        //OUTLINE
        List<Vector2[]> ISpritePhysicsOutlineDataProvider.GetOutlines(GUID guid) =>
            GetSpriteData(guid).GetOutlines();

        void ISpritePhysicsOutlineDataProvider.SetOutlines(GUID guid, List<Vector2[]> data) =>
            GetSpriteData(guid).SetOutlines(data);

        float ISpritePhysicsOutlineDataProvider.GetTessellationDetail(GUID guid) => 
            GetSpriteData(guid).tessellationDetail;

        void ISpritePhysicsOutlineDataProvider.SetTessellationDetail(GUID guid, float value) => 
            GetSpriteData(guid).tessellationDetail = value;

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
     #1#
        SecondarySpriteTexture[] ISecondaryTextureDataProvider.textures
        {
            get => secondaryTextures;
            set => secondaryTextures = value;
        }
    }
}*/