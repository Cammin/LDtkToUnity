using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using System.Collections.Generic;

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
    /// The new way of creating sprites are doing to be localized to individual tileset definitions so that we can make the separate
    /// </summary>
    internal sealed class LDtkTilesetDefExporter
    {
        private readonly int _pixelsPerUnit;
        private readonly LDtkArtifactAssets _artifacts;
        private readonly AssetImportContext _ctx;
        
        private LDtkLoadedBackgroundDict _dict;


        //todo converting from sprite actions into giving to the tileset definition, and MOVING the tile actions to the artifact factory for the 


        public LDtkTilesetDefExporter(AssetImportContext ctx, int pixelsPerUnit, LDtkArtifactAssets artifacts)
        {
            _pixelsPerUnit = pixelsPerUnit;
            _artifacts = artifacts;
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

                            //exclude the rects that are gonna be auto generated by the tileset import process anyways 
                            int gridSize = rect.Tileset.TileGridSize;
                            if (rect.W == gridSize && rect.W == gridSize)
                            {
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
        
        private void ExportTilesetDefinition(TilesetDefinition def, Dictionary<int, HashSet<RectInt>> rectsToGenerate)
        {
            Debug.Log($"attempt export");
            //bool AreFileContentsEqual(byte[] bytes, string destPath) =>
            string directoryName = Path.GetDirectoryName(_ctx.assetPath);
            string projectName = Path.GetFileNameWithoutExtension(_ctx.assetPath);
            
            if (directoryName == null)
            {
                LDtkDebug.Log($"Issue exporting a tileset definition; Path was invalid: {_ctx.assetPath}");
                return;
            }
            
            string path = Path.Combine(directoryName, projectName, def.Identifier) + ".ldtkt";

            LDtkPathUtility.TryCreateDirectoryForFile(path);
            Debug.Log($"Make a tileset def at path: {path}");
            
            LDtkTilesetDefinition data = new LDtkTilesetDefinition()
            {
                Def = def,
                Ppu = _pixelsPerUnit,
                Rects = rectsToGenerate.TryGetValue(def.Uid, out HashSet<RectInt> value) ? value.ToList() : null
            };

            byte[] bytes = data.ToJson();


            bool existsBeforehand = File.Exists(path);
            byte[] oldHash = Array.Empty<byte>();
            if (existsBeforehand)
            {
                oldHash = GetFileHash(path);
            }


            Debug.Log("write");
            File.WriteAllBytes(path, bytes);

            byte[] newHash = GetFileHash(path);

            if (!existsBeforehand || !oldHash.SequenceEqual(newHash))
            {
                Debug.Log("FORCE reimport because of new tile def data");
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }

            Debug.Log("code after the import??");
            //_ctx.DependsOnArtifact(path);
        }
        
        /// <summary>
        /// This may be used in the actual sprite generation step instead of here
        /// </summary>
        private List<RectInt> GetStandardSpriteRectsForDefinition(TilesetDefinition def)
        {
            List<RectInt> rects = new List<RectInt>();
            //Debug.Log($"The tileset {def.Identifier} uses {usedTiles.Count} unique tiles");
            int padding = def.Padding;
            int gridSize = def.TileGridSize;
            int spacing = def.Spacing;
            
            for (int y = padding; y < def.PxHei - padding; y += gridSize + spacing)
            {
                for (int x = padding; x < def.PxWid - padding; x += gridSize + spacing)
                {
                    if (x + gridSize > def.PxWid || y + gridSize > def.PxHei)
                    {
                        continue;
                    }

                    RectInt slice = new RectInt(x, y, gridSize, gridSize);
                    rects.Add(slice);
                }
            }

            return rects;
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

        /*private void SetupFieldInstanceSlices(TilesetRectangle rectangle, Texture2D texAsset)
        {
            
        }*/
        
        
        

        /*private void SetupTilesetCreations(TilesetDefinition def, Texture2D texAsse)
{
    //Debug.Log($"The tileset {def.Identifier} uses {usedTiles.Count} unique tiles");
    int padding = def.Padding;
    int gridSize = def.TileGridSize;
    int spacing = def.Spacing;
    
    for (int y = padding; y < def.PxHei - padding; y += gridSize + spacing)
    {
        for (int x = padding; x < def.PxWid - padding; x += gridSize + spacing)
        {
            if (x + gridSize > def.PxWid || y + gridSize > def.PxHei)
            {
                continue;
            }

            RectInt slice = new RectInt(x, y, gridSize, gridSize);
            _spriteActions.Add(() => CreateSprite(texAsset, slice, _pixelsPerUnit));
            
        }
    }
}*/
        
        private void SetupAllTilesetSlices(LdtkJson json)
        {
            foreach (TilesetDefinition def in json.Defs.Tilesets)
            {
                //SetupTilesetSlicesForDefinition(def);
            }
        }
        private void SetupTilesetSlicesForDefinition(TilesetDefinition def)
        {
            /*Texture2D texAsset = null;
            if (def.IsEmbedAtlas)
            {
                texAsset = LDtkProjectSettings.InternalIconsTexture;
                if (texAsset == null)
                {
                    LDtkDebug.LogWarning("The project uses the internal icons tileset but the texture is not assigned or found. Add it in Project Settings > LDtk To Unity.");
                    return;
                }
            }
            else
            {
                texAsset = _dict.Get(def.RelPath);
                if (texAsset == null)
                {
                    //LDtkDebug.LogError($"Didn't load texture at path \"{def.RelPath}\" for tileset {def.Identifier}");
                    return;
                }
            }
            //just a safety net
            if (texAsset == null)
            {
                return;
            }
            if (!ValidateTextureWithTilesetDef(def, texAsset))
            {
                return;
            }*/
            
        }
        
        private bool ValidateTextureWithTilesetDef(TilesetDefinition def, Texture2D tex)
        {
            if (def.PxWid == tex.width && def.PxHei == tex.height)
            {
                return true;
            }
            
            LDtkDebug.LogError($"The imported texture's size was unexpected for \"{tex.name}\" when trying trying to make tiles for the tileset: \"{def.Identifier}\". " +
                               $"Try increasing the texture's max size in it's import settings.\n" +
                               $"Expected {def.PxWid}x{def.PxHei} but was {tex.width}x{tex.height}");
            return false;
        }
    }

    internal class LDtkTilesetSliceCreator
    {
        public int _pixelsPerUnit;
        public Texture2D _tex;
    }
}