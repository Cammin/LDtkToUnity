using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_TILESET)]
    [ScriptedImporter(LDtkImporterConsts.TILESET_VERSION, LDtkImporterConsts.TILESET_EXT, LDtkImporterConsts.TILESET_ORDER)]
    internal sealed partial class LDtkTilesetImporter : LDtkJsonImporter<LDtkTilesetFile>
    {
        //public FilterMode _filterMode = FilterMode.Point;
        public List<LDtkSpriteRect> _sprites = new List<LDtkSpriteRect>();
        public SecondarySpriteTexture[] _secondaryTextures;
    
        private Texture2D _tex;
        private string _errorText;

        private TextureImporter _textureImporter;
        private LDtkTilesetFile _tilesetFile;
        private int _pixelsPerUnit = 16;
        private LDtkTilesetImporterData _tilesetData;
        private TilesetDefinition _tilesetJson;
        
        public static string[] _previousDependencies;
        
        private static string[] GatherDependenciesFromSourceFile(string path)
        {
            Debug.Log("tileset def gather");

            //this depends on the texture
            LDtkProfiler.BeginSample($"GatherDependenciesFromSourceFile/{Path.GetFileName(path)}");
            string texPath = PathToTexture(path);
            _previousDependencies = string.IsNullOrEmpty(texPath) ? Array.Empty<string>() : new []{texPath};
            LDtkProfiler.EndSample();
            return _previousDependencies;
        }
        protected override string[] GetGatheredDependencies() => _previousDependencies;
        

        protected override void Import()
        {
            Debug.Log("tileset def import");

            if (IsBackupFile())
            {
                FailImport();
                return;
            }
            
            Profiler.BeginSample("DeserializeAndAssign");
            if (!DeserializeAndAssign())
            {
                Profiler.EndSample();
                FailImport();
                return;
            }
            Profiler.EndSample();
            
            
            Profiler.BeginSample("GetTextureImporterPlatformSettings");
            TextureImporterPlatformSettings platformSettings = GetTextureImporterPlatformSettings();
            Profiler.EndSample();
            
            Profiler.BeginSample("GetTextureImporterPlatformSettings");
            if (CorrectTheTexture(_textureImporter, platformSettings))
            {
                //return because of texture importer corrections. we're going to import a 2nd time
                Profiler.EndSample();
                FailImport();
                return;
            }
            Profiler.EndSample();

            TextureGenerationOutput output = PrepareGenerate(platformSettings);

            Texture outputTexture = output.output;
            if (!outputTexture)
            {
                return;
            }
            
            outputTexture.name = AssetName;
            ImportContext.AddObjectToAsset("obj", outputTexture, LDtkIconUtility.LoadTilesetFileIcon());
            ImportContext.SetMainObject(outputTexture);

            foreach (Sprite spr in output.sprites)
            {
                AddOffsetToPhysicsShape(spr);
                ImportContext.AddObjectToAsset(spr.name, spr);
            }
        }

        
        
        //todo integrate this into base logic. and only display this asset in the importer inspector if this exists
        private void FailImport()
        {
            FailedImportObject o = ScriptableObject.CreateInstance<FailedImportObject>();
            o.Messages.Add(new ImportInfo(){Message = _errorText, Type = MessageType.Error});
            ImportContext.AddObjectToAsset("failedImport", o, LDtkIconUtility.LoadTilesetFileIcon());
            ImportContext.SetMainObject(o);
        }

        

        private TextureGenerationOutput PrepareGenerate(TextureImporterPlatformSettings platformSettings)
        {
            TextureImporterSettings importerSettings = new TextureImporterSettings();
            _textureImporter.ReadTextureSettings(importerSettings);
            importerSettings.spritePixelsPerUnit = _pixelsPerUnit;
            importerSettings.filterMode = FilterMode.Point;

            NativeArray<Color32> rawData = LoadTex().GetRawTextureData<Color32>();

            return TextureGeneration.Generate(
                ImportContext, rawData, _tilesetJson.PxWid, _tilesetJson.PxHei, _sprites.ToArray(),
                platformSettings, importerSettings, string.Empty, _secondaryTextures);
        }

        private TextureImporterPlatformSettings GetTextureImporterPlatformSettings()
        {
            string platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            TextureImporterPlatformSettings platformSettings = _textureImporter.GetPlatformTextureSettings(platform);
            return platformSettings.overridden ? platformSettings : _textureImporter.GetDefaultPlatformTextureSettings();
        }

        private bool DeserializeAndAssign()
        {
            //deserialize first. required for the path to the texture importer 
            try
            {
                _tilesetData = FromJson<LDtkTilesetImporterData>();
                _tilesetJson = _tilesetData.Def;
                _pixelsPerUnit = _tilesetData.PixelsPerUnit;
            }
            catch (Exception e)
            {
                _errorText = e.ToString();
                return false;
            }
            
            Profiler.BeginSample("GetTextureImporter");
            
            
            //LDtkDebug.LogError($"Path {path} is not valid. Is this tileset asset in a folder relative to the LDtk project file? Ensure that it's relativity is maintained if the project was moved also.");
            //string pathToTex = PathToTexture(assetPath);
            _textureImporter = (TextureImporter)GetAtPath(PathToTexture(assetPath));
            Profiler.EndSample();
            
            if (_textureImporter == null)
            {
                _errorText = $"Tried to build tileset {AssetName}, but the texture importer was not found. Is this tileset asset in a folder relative to the LDtk project file? Ensure that it's relativity is maintained if the project was moved also.";
                //LDtkDebug.LogError($"Tried to build tileset {AssetName}, but the texture importer was not found. Is this tileset asset in a folder relative to the LDtk project file? Ensure that it's relativity is maintained if the project was moved also.");
                return false;
            }

            Profiler.BeginSample("AddTilesetSubAsset");
            _tilesetFile = AddTilesetSubAsset();
            Profiler.EndSample();
            
            if (_tilesetFile == null)
            {
                _errorText = "Tried to build tileset, but the tileset json ScriptableObject was null";
                return false;
            }
            
            return true;
        }
        
        
        private LDtkTilesetFile AddTilesetSubAsset()
        {
            LDtkTilesetFile tilesetFile = ReadAssetText();
            ImportContext.AddObjectToAsset("tilesetFile", tilesetFile, LDtkIconUtility.LoadTilesetIcon());
            return tilesetFile;
        }

        private static string PathToTexture(string assetPath)
        {
            LDtkRelativeGetterTilesetTexture getter = new LDtkRelativeGetterTilesetTexture();
            string pathFrom = Path.Combine(assetPath, "..");
            pathFrom = LDtkPathUtility.CleanPath(pathFrom);
            string path = getter.GetPath(FromJson<LDtkTilesetImporterData>(assetPath).Def, pathFrom);
            //Debug.Log($"relative from {pathFrom}. path of texture importer was {path}");
            return !File.Exists(path) ? string.Empty : path;
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

        /*private void ForceUpdateSpriteDataNames()
        {
            foreach (LDtkSpriteRect spr in _sprites)
            {
                ForceUpdateSpriteDataName(spr);
            }
        }*/

        private void ForceUpdateSpriteDataName(SpriteRect spr)
        {
            spr.name = $"{AssetName}_{spr.rect.x}_{spr.rect.y}_{spr.rect.width}_{spr.rect.height}";
        }

        private bool CorrectTheTexture(TextureImporter textureImporter, TextureImporterPlatformSettings platformSettings)
        {
            bool issue = false;

            if (platformSettings.maxTextureSize < _tilesetJson.PxWid || platformSettings.maxTextureSize < _tilesetJson.PxHei)
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
        
            _tex = null;
            textureImporter.SetPlatformTextureSettings(platformSettings);
            textureImporter.SaveAndReimport();
            return true;
        }

        private Texture2D LoadTex(bool forceLoad = false)
        {
            //in case the importer was destroyed via file delete
            if (this == null)
            {
                return null;
            }
            
            if (_tex == null || forceLoad)
            {
                _tex = AssetDatabase.LoadAssetAtPath<Texture2D>(PathToTexture(assetPath));
            }
            return _tex;
        }
        
        private LDtkSpriteRect GetSpriteData(GUID guid)
        {
            LDtkSpriteRect data = _sprites.FirstOrDefault(x => x.spriteID == guid);
            Debug.Assert(data != null, $"Sprite data not found for GUID: {guid.ToString()}");
            return data;
        }

        private LDtkSpriteRect GetSpriteData(string spriteName)
        {
            LDtkSpriteRect data = _sprites.FirstOrDefault(x => x.name == spriteName);
            Debug.Assert(data != null, $"Sprite data not found for name: {spriteName}");
            return data;
        }


    }
}