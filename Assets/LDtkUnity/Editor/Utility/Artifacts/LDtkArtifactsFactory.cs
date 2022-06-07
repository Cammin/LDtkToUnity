using System;
using System.Collections.Generic;
using System.Linq;
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
    /// </summary>
    internal class LDtkArtifactsFactory //move all the project importer fluff responsibility here when ready. probably after integrating the feature fully
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
            //cache every possible artifact in the project. todo this is not optimized for atlas size, but necessary for now. might be able to dig into json to optimise this as a bool toggle option in inspector
            //this would be all tiles, all sprites, and the background texture.

            Profiler.BeginSample("TextureDict.LoadAllProjectTextures");
            LoadAllProjectTextures(json); //loads all tileset textures and ALSO Level backgrounds!
            Profiler.EndSample();
            
            Profiler.BeginSample("SetupAllTilesetSlices");
            SetupAllTilesetSlices(json);
            Profiler.EndSample();

            Profiler.BeginSample("GetAllFieldSlices");
            List<TilesetRectangle> fieldSlices = GetAllFieldSlices(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("SetupAllFieldSlices");
            SetupAllFieldSlices(fieldSlices);
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

        private void SetupAllBackgroundSlices(LdtkJson json) //todo make sure we arent causing any duplicates, hopefully.
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
                Texture2D texAsset = _dict.Get(rectangle.Tileset.RelPath);
                if (texAsset == null)
                {
                    Debug.LogError($"Didn't load texture at path \"{rectangle.Tileset.RelPath}\" when setting up field slices");
                    continue;
                }
                
                if (!ValidateTextureWithTilesetDef(rectangle.Tileset, texAsset))
                {
                    continue;
                }

                SetupFieldInstanceSlices(rectangle, texAsset);
            }
        }

        private void SetupAllTilesetSlices(LdtkJson json)
        {
            foreach (TilesetDefinition def in json.Defs.Tilesets)
            {
                if (def.IsEmbedAtlas)
                {
                    //todo eventually handle this
                    continue;
                }

                Texture2D texAsset = _dict.Get(def.RelPath);
                if (texAsset == null)
                {
                    //LDtkDebug.LogError($"Didn't load texture at path \"{def.RelPath}\" for tileset {def.Identifier}");
                    continue;
                }

                if (!ValidateTextureWithTilesetDef(def, texAsset))
                {
                    continue;
                }
                
                SetupTilesetCreations(def, texAsset);
            }
        }

        private bool ValidateTextureWithTilesetDef(TilesetDefinition def, Texture2D tex)
        {
            if (def.PxWid == tex.width && def.PxHei == tex.height)
            {
                return true;
            }
            
            LDtkDebug.LogError($"The imported texture's size was unexpected for \"{tex.name}\" when trying trying to make tiles for the tileset: \"{def.Identifier}\" Try increasing the texture's max size in it's import settings.");
            return false;
        }

        private void LoadAllProjectTextures(LdtkJson json)
        {
            _dict = new LDtkLoadedTextureDict(_ctx.assetPath);
            _dict.LoadAll(json);
        }

        /// <summary>
        /// Important: when we get the slices inside levels, we need to dig into json for some of these.
        /// Because we are looking for tile instances in levels, 
        /// </summary>
        private List<TilesetRectangle> GetAllFieldSlices(LdtkJson json)
        {
            Dictionary<string, TilesetRectangle> fieldSlices = new Dictionary<string, TilesetRectangle>();
            
            foreach (World world in json.UnityWorlds)
            {
                foreach (Level level in world.Levels)
                {
                    HandleLevel(level);
                }
            }
            
            void HandleLevel(Level level)
            {
                //Entity tile fields. If external levels, then dig into it. If in our own json, then we can safely get them from the layer instances in the json.
                if (json.ExternalLevels)
                {
                    string path = new LDtkRelativeGetterLevels().GetPath(level, _ctx.assetPath);
                    if (!LDtkJsonDigger.GetUsedFieldTiles(path, out List<FieldInstance> fields))
                    {
                        LDtkDebug.LogError($"Couldn't get entity tile field instance for level: {level.Identifier}");
                        return;
                    }
                    
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
                
                foreach (FieldInstance levelFieldInstance in level.FieldInstances) 
                {
                    TryAddFieldInstance(levelFieldInstance);
                }
                
                foreach (LayerInstance layer in level.LayerInstances)
                {
                    if (!layer.IsEntitiesLayer)
                    {
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
                foreach (TilesetRectangle rect in rects) //the expected value here is a string of the field.Value
                {
                    //Debug.Log($"Element {element}");
                    if (rect == null)
                    {
                        LDtkDebug.LogError($"A FieldInstance element was null for {field.Identifier}");
                        continue;
                    }
                    //don't end up adding duplicates that were normally generated from the tileset naturally
                    long gridSize = rect.Tileset.TileGridSize;
                    if (rect.W == gridSize && rect.H == gridSize)
                    {
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
            
            return fieldSlices.Values.ToList();
        }
        
        private TilesetRectangle[] GetTilesetRectanglesFromField(FieldInstance field)
        {
            if (field.Value is TilesetRectangle[] rectangles)
            {
                return rectangles;
            }

            List<TilesetRectangle> rects = new List<TilesetRectangle>();
            object[] stringElements = LDtkFieldsFactory.GetElements((fieldInstance, o) => fieldInstance?.Value, field, field.Definition.IsArray);
            foreach (object element in stringElements)
            {
                //Debug.Log(element);
                if (element == null)
                {
                    continue;
                }
                
                string stringElement = element.ToString();
                if (stringElement.StartsWith("["))
                {
                    TilesetRectangle[] deserializedArray = TilesetRectangle.FromJsonToArray(stringElement);

                    foreach (TilesetRectangle deserializedElement in deserializedArray)
                    {
                        if (deserializedElement != null)
                        {
                            rects.Add(deserializedElement);
                        }
                    }
                    
                    continue;
                }

                rects.Add(TilesetRectangle.FromJson(stringElement));
            }

            return rects.ToArray();
        }
        
        private void SetupTilesetCreations(TilesetDefinition def, Texture2D texAsset)
        {
            for (long x = def.Padding; x < def.PxWid - def.Padding; x += def.TileGridSize + def.Spacing)
            {
                for (long y = def.Padding; y < def.PxHei - def.Padding; y += def.TileGridSize + def.Spacing)
                {
                    Vector2Int coord = new Vector2Int((int)x, (int)y);

                    int gridSize = (int)def.TileGridSize;
                    Rect slice = new Rect(coord.x, coord.y, gridSize, gridSize);
                    
                    _spriteActions.Add(() => CreateSprite(texAsset, slice, _pixelsPerUnit));
                    _tileActions.Add(() => CreateTile(texAsset, slice));
                }
            }
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

        /// <summary>
        /// Creates a tile during the import process. Does additionally creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        private void CreateTile(Texture2D srcTex, Rect srcPos)
        {
            if (!InitialCreationCheck(srcTex))
            {
                return;
            }
            LDtkTileArtifactFactory tileFactory = CreateTileFactory(srcTex, srcPos);
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
        
        private LDtkTileArtifactFactory CreateTileFactory(Texture2D srcTex, Rect srcPos)
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