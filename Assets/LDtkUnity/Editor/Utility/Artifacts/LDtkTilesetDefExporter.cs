using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    /// <summary>
    /// This is the master class for creating tiles+sprite, and backgrounds. It's also ideally the only time we should load textures.
    /// This also will create and export the tileset definition file.
    ///
    /// The previous method just had made sprites based on any first occurrence.
    /// The new way of creating sprites are doing to be localized to individual tileset definitions so that we can make the separate importing
    /// </summary>
    internal sealed class LDtkTilesetDefExporter
    {
        private readonly int _pixelsPerUnit;
        private readonly AssetImportContext _ctx;

        //todo converting from sprite actions into giving to the tileset definition, and MOVING the tile actions to the artifact factory for the 


        public LDtkTilesetDefExporter(AssetImportContext ctx, int pixelsPerUnit)
        {
            _pixelsPerUnit = pixelsPerUnit;
            _ctx = ctx;
        }

        
        /// <summary>
        /// Figure out what malformed tiles should generate, and then export a tileset definition file for every tileset definition of this project
        /// </summary>
        public void ExportTilesetDefinitions(LdtkJson json)
        {
            Profiler.BeginSample("GetFieldSlicesAndUsedTileSlices");
            var fieldSlices = GetAllMalformedSpriteSlicesInProject(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("Export Tileset Definitions");
            foreach (TilesetDefinition def in json.Defs.Tilesets)
            {
                Profiler.BeginSample($"Export {def.Identifier}");
                ExportTilesetDefinition(def, fieldSlices);
                Profiler.EndSample();
            }
            Profiler.EndSample();
        }
        
        /// <summary>
        ///Dict of (TilesetUid => Rects)
        /// 
        /// Gets any and all information of used tiles. tiles from level/entity fields. these are the tiles that could have a size that deviates from the tileset definition's gridSize
        /// Important: when we get the slices inside levels, we need to dig into json for some of these.
        /// Because we are looking for tile instances in levels, 
        /// Technically we only need to grab the malformed tiles; the ones that are not in equal width/height of the layer's gridsize.
        /// This is because all of the gridsize tiles are going to be generated regardless.
        /// The instances where this can be the case are: editor visual for enum def value, editor visual for entity, and tile fields from levels/entities
        /// </summary>
        private Dictionary<int, HashSet<RectInt>> GetAllMalformedSpriteSlicesInProject(LdtkJson json)
        {
            Dictionary<int, HashSet<RectInt>> fieldSlices = new Dictionary<int, HashSet<RectInt>>();

            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels)
                {
                    //Entity tile fields. If external levels, then dig into the level. If in our own json, then we can safely get them from the layer instances in the json.
                    if (json.ExternalLevels)
                    {
                        string levelPath = new LDtkRelativeGetterLevels().GetPath(level, _ctx.assetPath);
                        List<FieldInstance> fields = new List<FieldInstance>();
                        if (!LDtkJsonDigger.GetUsedFieldTiles(levelPath, ref fields))
                        {
                            LDtkDebug.LogError($"Couldn't get entity tile field instance for level: {level.Identifier}");
                            continue;
                        }
                    
                        foreach (FieldInstance field in fields)
                        {
                            TryAddFieldInstance(field);
                        }
                        continue;
                    }

                    //NOTICE: depending on performance from directly getting json data instead of digging, i'll release this back.
                    //else it's not external levels and can ge grabbed from the json data for better performance
                    //Level field instances are still available in project json even with separate levels. They are both available in project and separate level files

                    //level's field instances
                    foreach (FieldInstance levelFieldInstance in level.FieldInstances) 
                    {
                        TryAddFieldInstance(levelFieldInstance);
                    }
                
                    //entity field instances
                    foreach (LayerInstance layer in level.LayerInstances)
                    {
                        foreach (EntityInstance entity in layer.EntityInstances)
                        {
                            foreach (FieldInstance entityField in entity.FieldInstances)
                            {
                                TryAddFieldInstance(entityField);
                            }
                        }
                    }
                    
                    void TryAddFieldInstance(FieldInstance field)
                    {
                        if (!field.IsTile)
                        {
                            return;
                        }

                        TilesetRectangle[] rects = GetTilesetRectanglesFromField(field);
                        if (rects == null)
                        {
                            return;
                        }
                        foreach (TilesetRectangle rect in rects) //the expected value here is a string of the field.Value
                        {
                            //Debug.Log($"Element {element}");
                            if (rect == null)
                            {
                                LDtkDebug.LogError($"A FieldInstance element was null for {field.Identifier}");
                                continue;
                            }

                            //deny adding duplicated to avoid identifier uniqueness
                            int key = rect.TilesetUid;
                            if (!fieldSlices.ContainsKey(key))
                            {
                                fieldSlices.Add(key, new HashSet<RectInt>());
                            }
                            fieldSlices[key].Add(rect.UnityRectInt);
                        }
                    }
                }
            }
            return fieldSlices;
        }
        
        private TilesetRectangle[] GetTilesetRectanglesFromField(FieldInstance field)
        {
            //if it's prebuilt via the json digger
            if (field.Value is TilesetRectangle[] arrayRects)
            {
                return arrayRects;
            }
            
            bool isArray = field.Definition.IsArray;
            
            //if it was a single null value
            if (field.Value == null)
            {
                if (isArray)
                {
                    Debug.LogError($"Null array? This is never supposed to be reached.");
                }

                return null;
            }
            

            if (isArray)
            {
                if (field.Value is List<object> rectangles)
                {
                    return rectangles.Select(LDtkParsedTile.ConvertDict).Where(p => p != null).ToArray();
                }

                Debug.LogError($"This is never supposed to be reached. {field.Identifier} {field.Value.GetType()}");
                return null;
            }

            if (field.Value is Dictionary<string, object> dict)
            {
                return new TilesetRectangle[] {LDtkParsedTile.ConvertDict(dict)};
            }
            
            //this could be if we did a json dig to construct this array
            if (field.Value is TilesetRectangle[] rects)
            {
                return rects;
            }

            Debug.LogError($"This is never supposed to be reached. {field.Identifier} {field.Value.GetType()}");
            return null;
        }
        
        public static string TilesetExportPath(string projectImporterPath, TilesetDefinition def)
        {
            string directoryName = Path.GetDirectoryName(projectImporterPath);
            string projectName = Path.GetFileNameWithoutExtension(projectImporterPath);

            if (directoryName == null)
            {
                LDtkDebug.Log($"Issue formulating a tileset definition path; Path was invalid for: {projectImporterPath}");
                return null;
            }
            
            return Path.Combine(directoryName, projectName, def.Identifier) + '.' + LDtkImporterConsts.TILESET_EXT;
        }

        private void ExportTilesetDefinition(TilesetDefinition def, Dictionary<int, HashSet<RectInt>> rectsToGenerate)
        {
            string writePath = TilesetExportPath(_ctx.assetPath, def);
            
            LDtkPathUtility.TryCreateDirectoryForFile(writePath);

            LDtkTilesetDefinition data = new LDtkTilesetDefinition()
            {
                Def = def,
                Ppu = _pixelsPerUnit,
            };

            if (rectsToGenerate.TryGetValue(def.Uid, out HashSet<RectInt> rects))
            {
                Debug.Log($"Got field slices for def {def.Identifier}: {rects.Count}");
                data.Rects = rects.Select(LDtkTilesetDefinition.TilesetRect.FromRectInt).ToList();
            }
            
            byte[] bytes;
            try
            {
                bytes = data.ToJson();
            }
            catch (Exception e)
            {
                LDtkDebug.LogError($"Failed to ToJson a tileset definition of {def.Identifier}: {e}");
                return;
            }

            bool existsBeforehand = File.Exists(writePath);
            byte[] oldHash = Array.Empty<byte>();
            if (existsBeforehand)
            {
                oldHash = GetFileHash(writePath);
            }

            File.WriteAllBytes(writePath, bytes);
            byte[] newHash = GetFileHash(writePath);

            if (!existsBeforehand || !oldHash.SequenceEqual(newHash))
            {
                Debug.Log("FORCE reimport because of new tile def data");
                AssetDatabase.ImportAsset(writePath, ImportAssetOptions.ForceSynchronousImport);
            }
        }
        
        private byte[] GetFileHash(string fileName)
        {
            HashAlgorithm sha1 = HashAlgorithm.Create();
            using (FileStream stream = new FileStream(fileName,FileMode.Open,FileAccess.Read))
                return sha1.ComputeHash(stream);
        }
        
        //todo might be usable later for aseprite
        private static Texture2D CreateTex(TilesetDefinition def, Texture2D srcTex)
        {
            int gridSize = def.TileGridSize;
            int w = def.PxWid;
            int h = def.PxHei;

            Texture2D newTex = new Texture2D(srcTex.width + gridSize, srcTex.height + gridSize, srcTex.format, 0, false);
            newTex.alphaIsTransparency = true;

            Graphics.CopyTexture(srcTex, 0, 0, 0, 0, srcTex.width, srcTex.height, newTex, 0, 0, 0, gridSize);
            newTex.Apply();
            
            for (int y = gridSize; y < newTex.height; y++)
            {
                //Color c = newTex.GetPixel(5, h - 10);
                Color c = newTex.GetPixel(w-1, y);
                //Debug.Log($"got pixel at {w},{y} being {c}");
                for (int x = w; x < newTex.width; x++)
                {
                    //Debug.Log($"set px at {x}, {y}");
                    newTex.SetPixel(x, y, c);
                }
            }
            for (int x = 0; x < w; x++)
            {
                Color c = newTex.GetPixel(x, gridSize+1);
                for (int y = 0; y < gridSize; y++)
                {
                    newTex.SetPixel(x, y, c);
                }
            }

            Color bottomLeftColor = newTex.GetPixel(w-1, 0);
            for (int x = w; x < newTex.width; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    newTex.SetPixel(x, y, bottomLeftColor);
                }
            }
            
            newTex.Apply();
            return newTex;
        }
    }
}