using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using Object = UnityEngine.Object;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

#pragma warning disable 0414
#pragma warning disable 0649

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_PROJECT)]
    [ScriptedImporter(LDtkImporterConsts.PROJECT_VERSION, LDtkImporterConsts.PROJECT_EXT, LDtkImporterConsts.PROJECT_ORDER)]
    internal class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        public const string JSON = nameof(_jsonFile);

        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string ATLAS = nameof(_atlas);
        public const string CUSTOM_LEVEL_PREFAB = nameof(_customLevelPrefab);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string USE_COMPOSITE_COLLIDER = nameof(_useCompositeCollider);
        public const string CREATE_BACKGROUND_COLOR = nameof(_createBackgroundColor);
        
        public const string INTGRID = nameof(_intGridValues);
        public const string ENTITIES = nameof(_entities);
        
        public const string ENUM_GENERATE = nameof(_enumGenerate);
        public const string ENUM_PATH = nameof(_enumPath);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        
        
        /// <summary>
        /// This is cached into the meta file upon an import. Could be null if the import was a failure. Invisible to the inspector.
        /// </summary>
        [SerializeField] private LDtkProjectFile _jsonFile;

        [SerializeField] private int _pixelsPerUnit = -1;
        [SerializeField] private SpriteAtlas _atlas;
        [SerializeField] private GameObject _customLevelPrefab = null;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _useCompositeCollider = true;
        [SerializeField] private bool _createBackgroundColor = true;
        
        [SerializeField] private LDtkAssetIntGridValue[] _intGridValues = Array.Empty<LDtkAssetIntGridValue>();
        
        [SerializeField] private LDtkAssetEntity[] _entities = Array.Empty<LDtkAssetEntity>();
        
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;

        
        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public SpriteAtlas Atlas => _atlas;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject CustomLevelPrefab => _customLevelPrefab;
        public bool UseCompositeCollider => _useCompositeCollider;
        public bool CreateBackgroundColor => _createBackgroundColor;

        private LDtkArtifactAssets _artifacts;
        private bool _hadTextureProblem;
        private static string[] _previousDependencies;
        
        //this will run upon standard reset, but also upon the meta file generation during the first import
        private void Reset()
        {
            OnResetPPU();
        }

        private static string[] GatherDependenciesFromSourceFile(string path)
        {
            _previousDependencies = LDtkProjectDependencyFactory.GatherProjectDependencies(path);
            return _previousDependencies;
        }

        protected override string[] GetGatheredDependencies() => _previousDependencies;

        protected override void Import()
        {
            if (IsBackupFile())
            {
                BufferEditorCache();
                return;
            }
            
            _hadTextureProblem = false;
            
            Profiler.BeginSample("CreateJsonAsset");
            CreateJsonAsset();
            Profiler.EndSample();
            
            if (!TryGetJson(out LdtkJson json))
            {
                Debug.LogError("LDtk: Json deserialization error. Not importing.");
                BufferEditorCache();
                return;
            }
            
            CacheDefs(json);

            Profiler.BeginSample("SetupAllAssetDependencies");
            SetupAllAssetDependencies();
            Profiler.EndSample();
            
            Profiler.BeginSample("CheckOutdatedJsonVersion");
            CheckOutdatedJsonVersion(json.JsonVersion, AssetName);
            Profiler.EndSample();

            Profiler.BeginSample("SetPixelsPerUnit");
            SetPixelsPerUnit((int) json.DefaultGridSize); //if for whatever reason (or backwards compatibility), if the ppu is -1 in any capacity
            Profiler.EndSample();
            
            Profiler.BeginSample("CreateArtifactAsset");
            CreateArtifactAsset();
            Profiler.EndSample();
            
            Profiler.BeginSample("CacheRecentImporter");
            LDtkParsedTile.CacheRecentImporter(this);
            Profiler.EndSample();
            
            Profiler.BeginSample("CreateAllArtifacts");
            CreateAllArtifacts(json); //this function depends on cached defs.
            Profiler.EndSample();

            Profiler.BeginSample("MainBuild");
            MainBuild(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("TryGenerateEnums");
            TryGenerateEnums(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("HideArtifactAssets");
            HideArtifactAssets();
            Profiler.EndSample();
            
            Profiler.BeginSample("TryPrepareSpritePacking");
            TryPrepareSpritePacking(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("BufferEditorCache");
            BufferEditorCache();
            Profiler.EndSample();

            Profiler.BeginSample("CheckDefaultEditorBehaviour");
            CheckDefaultEditorBehaviour();
            Profiler.EndSample();
            
            ReleaseDefs();
        }

        

        private static void CheckDefaultEditorBehaviour()
        {
            if (EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
            {
                Debug.LogWarning("LDtk: It is encouraged to use 2D project mode while using LDtkToUnity. Change it in \"Project Settings > Editor > Default Behaviour Mode\"");
            }
        }

        public static void CheckOutdatedJsonVersion(string jsonVersion, string assetName)
        {
            jsonVersion = Regex.Replace(jsonVersion, "[^0-9.]", "");
            if (!Version.TryParse(jsonVersion, out Version version))
            {
                LDtkDebug.LogError($"This json asset \"{assetName}\" couldn't parse it's version \"{jsonVersion}\", post an issue to the developer");
                return;
            }

            Version minimumRecommendedVersion = new Version(LDtkImporterConsts.LDTK_JSON_VERSION);
            if (version < minimumRecommendedVersion)
            {
                Debug.LogWarning($"LDtk: ({version}<{minimumRecommendedVersion}) The version of the project \"{assetName}\" is {version}. It's recommended to update your project to at least {minimumRecommendedVersion} to minimise issues.");
            }
        }

        private void SetupAllAssetDependencies() //todo don't setup dependencies here anymore. setup up the ones that are only necessary from the builders. 
        {
            //trigger a reimport if any of these involved assets are saved or otherwise changed in source control
            //SetupAssetDependencies(_intGridValues.Distinct().Cast<ILDtkAsset>().ToArray());
            //SetupAssetDependencies(_entities.Distinct().Cast<ILDtkAsset>().ToArray());

            if (_customLevelPrefab != null)
            {
                Dependencies.AddDependency(_customLevelPrefab);
            }
        }

        private void TryPrepareSpritePacking(LdtkJson json)
        {
            //allow the sprites to be gettable in the AssetDatabase properly; only after the import process
            if (_hadTextureProblem)
            {
                Debug.LogWarning($"LDtk: Did not pack tile textures for {assetPath}, a previous tile error was encountered.");
                return;
            }
            
            if (_atlas == null || !LDtkProjectImporterAtlasPacker.UsesSpriteAtlas(json))
            {
                return;
            }
            
            LDtkProjectImporterAtlasPacker packer = new LDtkProjectImporterAtlasPacker(_atlas, assetPath);
            packer.TryPack();
        }

        private void HideArtifactAssets()
        {
            //need to keep the sprites visible in the project view if using sprite atlas
            if (_atlas == null)
            {
                _artifacts.HideSprites();
            }

            _artifacts.HideTiles();
            
            _artifacts.HideBackgrounds();
        }

        private bool TryGetJson(out LdtkJson json)
        {
            json = _jsonFile.FromJson;
            if (json != null)
            {
                return true;
            }

            ImportContext.LogImportError("LDtk: Json import error");
            return false;

        }

        private void CreateJsonAsset()
        {
            _jsonFile = ReadAssetText();
            _jsonFile.name += "_Json";
            ImportContext.AddObjectToAsset("jsonFile", _jsonFile, (Texture2D)LDtkIconUtility.LoadListIcon());
        }
        
        private void BufferEditorCache()
        {
            EditorApplication.delayCall += () =>
            {
                LDtkJsonEditorCache.ForceRefreshJson(assetPath);
            };
        }

        private void MainBuild(LdtkJson json)
        {
            LDtkProjectImporterFactory factory = new LDtkProjectImporterFactory(this, Dependencies);
            factory.Import(json);
        }

        private void CreateArtifactAsset()
        {
            //the bank for storing the auto-generated items.
            _artifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            _artifacts.name = AssetName + "_Assets";
            
            ImportContext.AddObjectToAsset("artifacts", _artifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Tilemap"));
        }

        private void TryGenerateEnums(LdtkJson json)
        {
            //generate enums
            if (!_enumGenerate || json.Defs.Enums.IsNullOrEmpty())
            {
                return;
            }
            
            LDtkProjectImporterEnumGenerator enumGenerator = new LDtkProjectImporterEnumGenerator(json.Defs.Enums, ImportContext, _enumPath, _enumNamespace);
            enumGenerator.Generate();
        }

        public void AddArtifact(Object obj)
        {
            if (ImportContext == null)
            {
                //import context may just be null because the level importer is running over it. we can safely not require to add a dependency
                return;
            }
            
            if (_artifacts.AddArtifact(obj))
            {
                ImportContext.AddObjectToAsset(obj.name, obj);
            }
        }

        public void AddBackgroundArtifact(Sprite obj)
        {
            //todo this should not be set up here, it should be managed from the addalldependencies
            _artifacts.AddBackground(obj);
            ImportContext.AddObjectToAsset(obj.name, obj);
        }

        private void SetupAssetDependencies(ILDtkAsset[] assets)
        {
            //dependencies. reimport if any of these assets change
            if (assets.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (ILDtkAsset asset in assets)
            {
                if (asset.Asset == null)
                {
                    continue;
                }

                Dependencies.AddDependency(asset.Asset);
            }
        }

        public LDtkIntGridTile GetIntGridValueTile(string key)
        {
            return GetAssetByIdentifier(_intGridValues, key);
        }
        public GameObject GetEntity(string key)
        {
            return GetAssetByIdentifier(_entities, key);
        }

        private T GetAssetByIdentifier<T>(IEnumerable<LDtkAsset<T>> input, string key) where T : Object
        {
            if (input == null)
            {
                ImportContext.LogImportError("LDtk: Tried getting an asset from the build data but the array was null. Is the project asset properly saved?");
                return default;
            }

            foreach (LDtkAsset<T> asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    ImportContext.LogImportError($"LDtk: A field in the build data is null.");
                    continue;
                }

                if (asset.Key != key)
                {
                    continue;
                }

                if (asset.Asset == null)
                {
                    continue;
                }
                
                return (T)asset.Asset;
            }
            
            return default;
        }

        //todo this responsibility can be into a subclass.
        private void CreateAllArtifacts(LdtkJson json)
        {
            //cache every possible artifact in the project. todo this is not optimized for atlas size, but necessary for now. might be able to dig into json to optimise this as a bool toggle option in inspector
            //this would be all tiles, all sprites, and the background texture.
            //todo needs background texture prep so that levels can get them

            Profiler.BeginSample("TextureDict.LoadAll");
            LDtkLoadedTextureDict dict = new LDtkLoadedTextureDict(assetPath); //loads all tileset textures and 
            dict.LoadAll(json);
            Profiler.EndSample();
            
            List<Action> spriteActions = new List<Action>();
            List<Action> tileActions = new List<Action>();
            
            Profiler.BeginSample("SetupAllTileSlices");
            TilesetDefinition[] defs = json.Defs.Tilesets;
            foreach (TilesetDefinition def in defs)
            {
                if (def.IsEmbedAtlas)
                {
                    //todo eventually handle this
                    continue;
                }
                
                Texture2D texAsset = dict.Get(def.RelPath);
                if (texAsset == null)
                {
                    //LDtkDebug.LogError($"Didn't load texture at path \"{def.RelPath}\" for tileset {def.Identifier}");
                    continue;
                }
                
                Dependencies.AddDependency(texAsset);
                SetupTilesetCreations(def, texAsset, ref spriteActions, ref tileActions);
            }
            Profiler.EndSample();

            
            Profiler.BeginSample("GetAllFieldSlices");
            List<TilesetRectangle> fieldSlices = GetAllFieldSlices(json);
            Profiler.EndSample();
            
            Profiler.BeginSample("SetupAllFieldSlices");
            foreach (TilesetRectangle rectangle in fieldSlices)
            {
                //Debug.Log($"Process FieldSlice: {rectangle}");
                Texture2D texAsset = dict.Get(rectangle.Tileset.RelPath);
                if (texAsset == null)
                {
                    Debug.LogError($"Didn't load texture at path \"{rectangle.Tileset.RelPath}\" when setting up field slices");
                    continue;
                }

                SetupFieldInstanceSlices(rectangle, texAsset, ref spriteActions);
            }
            Profiler.EndSample();

            //todo setup background sprites here. might also need to be handled different because it's considered a background sprite instead?

            //sprites, THEN tiles
            Profiler.BeginSample("SpriteActions");
            for (int i = 0; i < spriteActions.Count; i++)
            {
                Action action = spriteActions[i];
                action.Invoke();
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("TileActions");
            for (int i = 0; i < tileActions.Count; i++)
            {
                tileActions[i].Invoke();
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("AddDependencies");
            foreach (Texture2D texture in dict.Textures) //add dependencies on textures so that when any texture is changed, then the sprites will regenerate for them
            {
                Dependencies.AddDependency(texture);
            }
            Profiler.EndSample();
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
                    string path = new LDtkRelativeGetterLevels().GetPath(level, assetPath);
                    if (!LDtkJsonParser.GetUsedTileSprites(path, out List<FieldInstance> fields))
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

                //lets be specific here. We want to generate artifacts.
                //we want to generate sprites.
                
                //when we get elements, we simply want the string of the  
                
                //ensure we have a definition

                //we require this in order to take in any single or array into flat out turn into an array no matter what to make this simple for us.

                TilesetRectangle[] rects = GetTilesetRectanglesFromField(field);

                //todo take it from here. the string element needs to be optimised by being a string initially. how we split into elements just like the dug json? with LDtkFieldsFactory.GetElements?
                
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
            object[] stringElements = LDtkFieldsFactory.GetElements(GetInstanceValue, field, field.Definition.IsArray);
            foreach (object element in stringElements)
            {
                //Debug.Log(element);
                if (element == null)
                {
                    continue;
                }
                
                string stringElement = element.ToString();
                if (stringElement.StartsWith('['))
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

        private object GetInstanceValue(FieldInstance field, object value) => field?.Value;

        private void SetupTilesetCreations(TilesetDefinition def, Texture2D texAsset, ref List<Action> spriteActions, ref List<Action> tileActions)
        {
            for (long x = def.Padding; x < def.PxWid - def.Padding; x += def.TileGridSize + def.Spacing)
            {
                for (long y = def.Padding; y < def.PxHei - def.Padding; y += def.TileGridSize + def.Spacing)
                {
                    Vector2Int coord = new Vector2Int((int)x, (int)y);

                    int gridSize = (int)def.TileGridSize;
                    RectInt slice = new RectInt(coord.x, coord.y, gridSize, gridSize);
                    
                    spriteActions.Add(() => CreateSprite(texAsset, slice, _pixelsPerUnit));
                    tileActions.Add(() => CreateTile(texAsset, slice, _pixelsPerUnit));
                }
            }
        }
        private void SetupFieldInstanceSlices(TilesetRectangle rectangle, Texture2D texAsset, ref List<Action> spriteActions)
        {
            RectInt slice = rectangle.UnityRect;
            Texture2D tex = texAsset;
            spriteActions.Add(() => CreateSprite(tex, slice, _pixelsPerUnit));
        }
        private void SetupLevelBackgroundSlices(ref List<Action> spriteActions)
        {
            
        }
        
        /// <summary>
        /// Creates a tile during the import process. Does additionally creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        private void CreateTile(Texture2D srcTex, RectInt srcPos, int pixelsPerUnit)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return;
            }
            if (srcTex == null)
            {
                LDtkDebug.LogError("CreateTile srcTex was null, not making tile");
                return;
            }
            
            LDtkTileArtifactFactory tileFactory = CreateTileFactory(srcTex, srcPos, pixelsPerUnit);
            tileFactory.TryCreateTile();
        }
        
        public TileBase GetTile(Texture2D srcTex, RectInt srcPos, int pixelsPerUnit)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return null;
            }
            if (srcTex == null)
            {
                LDtkDebug.LogError("GetTile srcTex was null, not getting tile");
                return null;
            }
            
            LDtkTileArtifactFactory tileFactory = CreateTileFactory(srcTex, srcPos, pixelsPerUnit);
            TileBase tile = tileFactory.TryGetTile();
            if (tile != null)
            {
                return tile;
            }
            
            LDtkDebug.LogError("LDtk: Tried retrieving a Tile from the importer's assets, but was null.");
            _hadTextureProblem = true;
            return tile;
        }

        private LDtkTileArtifactFactory CreateTileFactory(Texture2D srcTex, RectInt srcPos, int pixelsPerUnit)
        {
            string assetName = LDtkKeyFormatUtil.GetSpriteOrTileAssetName(srcTex, srcPos);
            LDtkSpriteArtifactFactory spriteFactory = CreateSpriteFactory(srcTex, srcPos, pixelsPerUnit);
            return new LDtkTileArtifactFactory(this, _artifacts, spriteFactory, assetName);
        }
        private LDtkSpriteArtifactFactory CreateSpriteFactory(Texture2D srcTex, RectInt srcPos, int pixelsPerUnit)
        {
            string assetName = LDtkKeyFormatUtil.GetSpriteOrTileAssetName(srcTex, srcPos);
            return new LDtkSpriteArtifactFactory(this, _artifacts, srcTex, srcPos, pixelsPerUnit, assetName);
        }

        /// <summary>
        /// Creates a sprite as an artifact if the certain rect sprite wasn't made before
        /// </summary>
        public void CreateSprite(Texture2D srcTex, RectInt srcPos, int pixelsPerUnit)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return;
            }
            if (srcTex == null)
            {
                LDtkDebug.LogError("CreateSprite srcTex was null, not creating sprite");
                return;
            }
            
            LDtkSpriteArtifactFactory spriteFactory = CreateSpriteFactory(srcTex, srcPos, pixelsPerUnit);
            spriteFactory.TryCreateSprite();
        }
        
        /// <summary>
        /// IMPORTANT: Because we already pre-generated the sliced sprites, this doesn't and should not generate any assets.
        /// </summary>
        public Sprite GetSprite(TilesetRectangle tileRect)
        {
            if (_artifacts == null)
            {
                LDtkDebug.LogError("Project importer's artifact assets was null, this needs to be cached");
                return null;
            }

            TilesetDefinition tileset = tileRect.Tileset; //todo consider where this should properly be. we have lots of bits of data here that could just be contained into the key format util class, or in the tile parser class?
            string fileName = Path.GetFileNameWithoutExtension(tileset.RelPath);
            //Debug.Log($"filename for getting a tile field sprite was {fileName}. this should be the same name as the texture.");
            string assetName = LDtkKeyFormatUtil.GetSpriteOrTileAssetName(fileName, tileRect.UnityRect, (int)tileset.PxHei);
            Sprite sprite = _artifacts.GetIndexedSprite(assetName);
            if (sprite != null)
            {
                return sprite;
            }
            
            Debug.LogError("LDtk: Tried retrieving a Sprite from the importer's artifacts, but was null.");
            _hadTextureProblem = true;
            return sprite;
        }

        private void OnResetPPU()
        {
            if (_pixelsPerUnit > 0)
            {
                return;
            }
            
            if (!LDtkJsonParser.GetDefaultGridSize(assetPath, out int defaultGridSize))
            {
                //if problem, then default to what LDtk also defaults to upon a new project
                _pixelsPerUnit = LDtkImporterConsts.DEFAULT_PPU;
                return;
            }
            SetPixelsPerUnit(defaultGridSize);
        }

        private void SetPixelsPerUnit(int ppu)
        {
            if (_pixelsPerUnit > 0)
            {
                return;
            }
            
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty ppuProp = serializedObject.FindProperty(PIXELS_PER_UNIT);
            ppuProp.intValue = ppu;
            serializedObject.ApplyModifiedProperties();
        }
        
        public IEnumerable<TileBase> GetIntGridTiles()
        {
            return _intGridValues.Select(p => p.Asset).Cast<TileBase>().ToArray();
        }

        public void CacheArtifacts()
        {
            if (_artifacts != null)
            {
                return;
            }
            
            _artifacts = AssetDatabase.LoadAssetAtPath<LDtkArtifactAssets>(assetPath);

            if (_artifacts == null)
            {
                LDtkDebug.LogError("Artifacts was null during the import, this should never happen");
            }
        }
    }
}
