using System;
using System.Collections.Generic;
using System.Linq;
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
    /// </summary>
    internal sealed class LDtkArtifactsFactory //move all the project importer fluff responsibility here when ready. probably after integrating the feature fully
    {
        private readonly int _pixelsPerUnit;
        private readonly LDtkArtifactAssets _artifacts;
        private readonly AssetImportContext _ctx;
        
        private LDtkLoadedTextureDict _dict;
        private readonly HashSet<string> _uniqueSprites = new HashSet<string>();
        private readonly HashSet<string> _uniqueTiles = new HashSet<string>();
        
        private readonly List<Action> _spriteActions = new List<Action>();
        private readonly List<Action> _tileActions = new List<Action>();
        private readonly List<Action> _backgroundActions = new List<Action>();

        public LDtkArtifactsFactory(AssetImportContext ctx, int pixelsPerUnit, LDtkArtifactAssets artifacts)
        {
            _pixelsPerUnit = pixelsPerUnit;
            _artifacts = artifacts;
            _ctx = ctx;
        }

        public void CreateAllArtifacts(LdtkJson json)
        {
            //cache every possible artifact in the project.
            //this would be all tiles, all sprites, and the background texture.

            Profiler.BeginSample("TextureDict.LoadAllProjectTextures");
            LoadAllProjectTextures(json); //loads all tileset textures and ALSO Level backgrounds!
            Profiler.EndSample();
            
            Profiler.BeginSample("GetFieldSlicesAndUsedTileSlices");
            SliceData fieldSlices = GetAllFieldSlices(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("SetupAllTilesetSlices");
            SetupAllTilesetSlices(json, fieldSlices.UsedTileIds);
            Profiler.EndSample();

            Profiler.BeginSample("SetupAllFieldSlices");
            SetupAllFieldSlices(fieldSlices.FieldSlices);
            Profiler.EndSample();

            Profiler.BeginSample("SetupAllBackgroundSlices");
            SetupAllBackgroundSlices(json);
            Profiler.EndSample();

            Profiler.BeginSample($"BackgroundActions {_backgroundActions.Count}");
            BackgroundActions();
            Profiler.EndSample();
            
            //sprites, THEN tiles. tiles depend on sprites first
            Profiler.BeginSample($"SpriteActions {_spriteActions.Count}");
            SpriteActions();
            Profiler.EndSample();
            
            Profiler.BeginSample($"TileActions {_tileActions.Count}");
            TileActions();
            Profiler.EndSample();
        }

        private void TileActions()
        {
            for (int i = 0; i < _tileActions.Count; i++)
            {
                _tileActions[i].Invoke();
            }
        }

        private void SpriteActions()
        {
            for (int i = 0; i < _spriteActions.Count; i++)
            {
                _spriteActions[i].Invoke();
            }
        }
        private void BackgroundActions()
        {
            for (int i = 0; i < _backgroundActions.Count; i++)
            {
                _backgroundActions[i].Invoke();
            }
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

        private void SetupAllFieldSlices(List<TilesetRectangle> fieldSlices)
        {
            foreach (TilesetRectangle rectangle in fieldSlices)
            {
                //Debug.Log($"Process FieldSlice: {rectangle}");

                Texture2D texAsset = null;
                if (rectangle.Tileset.IsEmbedAtlas)
                {
                    texAsset = LDtkProjectSettings.InternalIconsTexture;
                    if (texAsset == null)
                    {
                        LDtkDebug.LogError($"A Tile field uses the LDtk internal icons texture but it's not assigned in the project settings.");
                        continue;
                    }
                }
                else
                {
                    texAsset = _dict.Get(rectangle.Tileset.RelPath);
                    if (texAsset == null)
                    {
                        LDtkDebug.LogError($"Didn't load texture at path \"{rectangle.Tileset.RelPath}\" when setting up field slices {_dict.Textures.Count()}");
                        continue;
                    }
                }
                
                
                if (!ValidateTextureWithTilesetDef(rectangle.Tileset, texAsset))
                {
                    continue;
                }

                SetupFieldInstanceSlices(rectangle, texAsset);
            }
        }
        
        private void SetupAllTilesetSlices(LdtkJson json, Dictionary<string, HashSet<int>> usedTileIds)
        {
            foreach (TilesetDefinition def in json.Defs.Tilesets)
            {
                Texture2D texAsset = null;
                if (def.IsEmbedAtlas)
                {
                    texAsset = LDtkProjectSettings.InternalIconsTexture;
                    if (texAsset == null)
                    {
                        //LDtkDebug.LogWarning("The project uses the internal icons tileset but the texture is not assigned or found. Add it in Project Settings > LDtk To Unity.");
                        continue;
                    }
                }
                else
                {
                    texAsset = _dict.Get(def.RelPath);
                    if (texAsset == null)
                    {
                        //LDtkDebug.LogError($"Didn't load texture at path \"{def.RelPath}\" for tileset {def.Identifier}");
                        continue;
                    }
                }

                //just a safety net
                if (texAsset == null)
                {
                    continue;
                }

                if (!ValidateTextureWithTilesetDef(def, texAsset))
                {
                    continue;
                }

                HashSet<int> idsForTileset = new HashSet<int>();
                foreach (LayerDefinition layerDef in json.Defs.Layers)
                {
                    string key = layerDef.Identifier;
                    if (usedTileIds.ContainsKey(key))
                    {
                        foreach (int id in usedTileIds[key])
                        {
                            idsForTileset.Add(id);
                        }
                    }
                }
                
                SetupTilesetCreations(def, texAsset, idsForTileset);
            }
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

        private void LoadAllProjectTextures(LdtkJson json)
        {
            _dict = new LDtkLoadedTextureDict(_ctx.assetPath);
            _dict.LoadAll(json);
        }

        public class SliceData
        {
            public List<TilesetRectangle> FieldSlices = new List<TilesetRectangle>();
            public Dictionary<string, HashSet<int>> UsedTileIds = new Dictionary<string, HashSet<int>>();
        }
        
        /// <summary>
        /// Important: when we get the slices inside levels, we need to dig into json for some of these.
        /// Because we are looking for tile instances in levels, 
        /// </summary>
        private SliceData GetAllFieldSlices(LdtkJson json)
        {
            Dictionary<string, TilesetRectangle> fieldSlices = new Dictionary<string, TilesetRectangle>();
            Dictionary<string, HashSet<int>> usedTileIds = new Dictionary<string, HashSet<int>>();
            
            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels)
                {
                    HandleLevel(level);
                }
            }
            
            void HandleLevel(Level level)
            {
                //Entity tile fields. If external levels, then dig into the level. If in our own json, then we can safely get them from the layer instances in the json.
                if (json.ExternalLevels)
                {
                    string levelPath = new LDtkRelativeGetterLevels().GetPath(level, _ctx.assetPath);
                    List<FieldInstance> fields = new List<FieldInstance>();
                    if (!LDtkJsonDigger.GetUsedFieldTiles(levelPath, ref fields))
                    {
                        LDtkDebug.LogError($"Couldn't get entity tile field instance for level: {level.Identifier}");
                        return;
                    }
                    
                    LDtkJsonDigger.GetUsedTilesetSprites(levelPath, ref usedTileIds);
                    
                    foreach (FieldInstance field in fields)
                    {
                        //Debug.Log($"TryAddFieldInstance {field}");
                        TryAddFieldInstance(field);
                    }
                    return;
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
                    string layerIdentifier = layer.Identifier;
                    if (!usedTileIds.ContainsKey(layerIdentifier))
                    {
                        usedTileIds.Add(layerIdentifier, new HashSet<int>());
                    }

                    if (!layer.IsEntitiesLayer)
                    {
                        foreach (TileInstance tileInstance in layer.GridTiles)
                        {
                            usedTileIds[layerIdentifier].Add(tileInstance.T);
                        }
                        foreach (TileInstance tileInstance in layer.AutoLayerTiles)
                        {
                            usedTileIds[layerIdentifier].Add(tileInstance.T);
                        }
                        continue;
                    }

                    foreach (EntityInstance entity in layer.EntityInstances)
                    {
                        foreach (FieldInstance entityField in entity.FieldInstances)
                        {
                            TryAddFieldInstance(entityField);
                        }
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
                    string key = rect.ToString();
                    if (fieldSlices.ContainsKey(key))
                    {
                        continue;
                    }
                    
                    //Debug.Log($"Added element {element}");
                    fieldSlices.Add(key, rect);
                }
            }

            SliceData sliceData = new SliceData
            {
                FieldSlices = fieldSlices.Values.ToList(),
                UsedTileIds = usedTileIds
            };
            return sliceData;
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
        
        private void SetupTilesetCreations(TilesetDefinition def, Texture2D texAsset, HashSet<int> usedTiles)
        {
            //Debug.Log($"The tileset {def.Identifier} uses {usedTiles.Count} unique tiles");
            int id = -1;
            long padding = def.Padding;
            long gridSize = def.TileGridSize;
            long spacing = def.Spacing;
            
            for (long y = padding; y < def.PxHei - padding; y += gridSize + spacing)
            {
                for (long x = padding; x < def.PxWid - padding; x += gridSize + spacing)
                {
                    id++;
                    if (!usedTiles.Contains(id) || x + gridSize > def.PxWid || y + gridSize > def.PxHei)
                    {
                        continue;
                    }

                    Rect slice = new Rect(x, y, gridSize, gridSize);
                    _spriteActions.Add(() => CreateSprite(texAsset, slice, _pixelsPerUnit));
                    _tileActions.Add(() => CreateTile(texAsset, slice));
                }
            }
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

        private void SetupFieldInstanceSlices(TilesetRectangle rectangle, Texture2D texAsset)
        {
            Rect slice = rectangle.UnityRect;
            Texture2D tex = texAsset;
            _spriteActions.Add(() => CreateSprite(tex, slice, _pixelsPerUnit));
        }
        private void SetupBackgroundSlices(Level level, Texture2D texAsset)
        {
            Level lvl = level;
            Texture2D tex = texAsset;
            _backgroundActions.Add(() => CreateLevelBackground(tex, lvl, _pixelsPerUnit));
        }
        
        private void CreateTile(Texture2D srcTex, Rect srcPos)
        {
            if (!InitialCreationCheck(srcTex))
            {
                return;
            }
            LDtkTileArtifactFactory tileFactory = CreateArtTileFactory(srcTex, srcPos);
            tileFactory?.TryCreateTile();
        }

        /// <summary>
        /// Creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        private void CreateSprite(Texture2D srcTex, Rect srcPos, int pixelsPerUnit)
        {
            if (!InitialCreationCheck(srcTex))
            {
                return;
            }
            LDtkSpriteArtifactFactory spriteFactory = CreateSpriteFactory(srcTex, srcPos, pixelsPerUnit);
            spriteFactory?.TryCreateSprite();
        }
        
        /// <summary>
        /// Creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        private void CreateLevelBackground(Texture2D srcTex, Level lvl, int pixelsPerUnit)
        {
            if (!InitialCreationCheck(srcTex))
            {
                return;
            }
            LDtkBackgroundArtifactFactory bgFactory = CreateBackgroundFactory(srcTex, lvl, pixelsPerUnit);
            bgFactory?.TryCreateBackground(); //this can be null in case there were duplicate assets being made
        }
        
        private bool InitialCreationCheck(Texture2D srcTex)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return false;
            }
            if (srcTex == null)
            {
                LDtkDebug.LogError("CreateAsset srcTex was null, not making tile");
                return false;
            }

            return true;
        }
        
        private LDtkTileArtifactFactory CreateArtTileFactory(Texture2D srcTex, Rect srcPos)
        {
            string assetName = LDtkKeyFormatUtil.GetCreateSpriteOrTileAssetName(srcPos, srcTex);
            if (!_uniqueTiles.Add(assetName))
            {
                return null;
            }
            return new LDtkTileArtifactFactory(_ctx, _artifacts, assetName);
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
        private LDtkBackgroundArtifactFactory CreateBackgroundFactory(Texture2D srcTex, Level level, int pixelsPerUnit)
        {
            string assetName = LDtkKeyFormatUtil.GetCreateSpriteOrTileAssetName(level.BgPos.UnityCropRect, srcTex);
            if (!_uniqueSprites.Add(assetName))
            {
                return null;
            }
            return new LDtkBackgroundArtifactFactory(_ctx, _artifacts, assetName, srcTex, pixelsPerUnit, level);
        }
    }
}