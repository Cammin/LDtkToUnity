using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    #if !UNITY_2021_2_OR_NEWER
    [InitializeOnLoad]
    #endif
    internal static class LDtkIconUtility
    {
        private const string PATH_ROOT = "Icons/";
        private const string PATH_LIGHT = PATH_ROOT + "Light/";
        private const string PATH_DARK = PATH_ROOT + "Dark/";
        private const string PATH_MISC = PATH_ROOT + "Misc/";
        
        private const string AUTO_LAYER = "AutoLayer";
        private const string ENTITY = "Entity";
        private const string ENUM = "Enum";
        private const string INT_GRID = "IntGrid";
        private const string LAYER = "Layer";
        private const string LEVEL = "Level";
        private const string LIST = "List";
        private const string TILESET = "Tileset";
        private const string WORLD = "World";
        
        private const string FAV = "Fav";
        private const string LEVEL_FILE = "LevelFile";
        private const string LEVEL_FILE_ERROR = "LevelFile_Error";
        private const string PROJECT_FILE = "ProjectFile";
        private const string PROJECT_FILE_ERROR = "ProjectFile_Error";
        private const string TILESET_FILE = "TilesetFile";
        private const string TILESET_FILE_ERROR = "TilesetFile_Error";
        private const string SIMPLE = "Simple";
        
        private const string SQUARE = "Square";
        private const string CIRCLE = "Circle";
        private const string CROSS = "Cross";
        
        private static readonly Dictionary<string, Texture2D> CachedIcons = new Dictionary<string, Texture2D>();
        
#if UNITY_2021_2_OR_NEWER
        private class Cacher : AssetPostprocessor
        {
            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
            {
                if (didDomainReload)
                {
                    CacheAllIcons(); //recommended from https://forum.unity.com/threads/what-is-asset-database-v2.680170/#post-7638085
                }
            }
        }
#else
        static LDtkIconUtility()
        {
            EditorApplication.delayCall += CacheAllIcons;
        }
#endif
        
        private static void CacheAllIcons()
        {
            CacheIcon(AUTO_LAYER, true);
            CacheIcon(ENTITY, true);
            CacheIcon(ENUM, true);
            CacheIcon(INT_GRID, true);
            CacheIcon(LAYER, true);
            CacheIcon(LEVEL, true);
            CacheIcon(LIST, true);
            CacheIcon(TILESET, true);
            CacheIcon(WORLD, true);
            CacheIcon(FAV);
            CacheIcon(LEVEL_FILE);
            CacheIcon(PROJECT_FILE);
            CacheIcon(TILESET_FILE);
            CacheIcon(SIMPLE);
            CacheIcon(SQUARE);
            CacheIcon(CIRCLE);
            CacheIcon(CROSS);
        }

        private static void CacheIcon(string fileName, bool hasThemedSkin = false)
        {
            if (hasThemedSkin)
            {
                CacheIcon(PATH_LIGHT, fileName);
                CacheIcon(PATH_DARK, fileName, true);
                return;
            }
            CacheIcon(PATH_MISC, fileName);
        }

        private static void CacheIcon(string rootPath, string fileName, bool darkTheme = false)
        {
            string path = $"{rootPath}{fileName}.png";
            Texture2D tex = LDtkInternalUtility.Load<Texture2D>(path);

            if (tex != null)
            {
                string key = darkTheme ? $"d_{fileName}" : fileName;
                if (!CachedIcons.ContainsKey(key))
                {
                    CachedIcons.Add(key, tex);
                }
                return;
            }

            LDtkDebug.LogError($"Did not cache icon {path}");
        }

        public static Texture2D LoadAutoLayerIcon() => LoadIcon(AUTO_LAYER, true);
        public static Texture2D LoadEntityIcon() => LoadIcon(ENTITY, true);
        public static Texture2D LoadEnumIcon() => LoadIcon(ENUM, true);
        public static Texture2D LoadIntGridIcon() => LoadIcon(INT_GRID, true);
        public static Texture2D LoadLayerIcon() => LoadIcon(LAYER, true);
        public static Texture2D LoadLevelIcon() => LoadIcon(LEVEL, true);
        public static Texture2D LoadListIcon() => LoadIcon(LIST, true);
        public static Texture2D LoadTilesetIcon() => LoadIcon(TILESET, true);
        public static Texture2D LoadWorldIcon() => LoadIcon(WORLD, true);
        
        public static Texture2D LoadFavIcon() => LoadIcon(FAV);
        public static Texture2D LoadLevelFileIcon(bool error = false) => LoadIcon(error ? LEVEL_FILE_ERROR : LEVEL_FILE);
        public static Texture2D LoadProjectFileIcon(bool error = false) => LoadIcon(error ? PROJECT_FILE_ERROR : PROJECT_FILE);
        public static Texture2D LoadTilesetFileIcon(bool error = false) => LoadIcon(error ? TILESET_FILE_ERROR : TILESET_FILE);
        public static Texture2D LoadSimpleIcon() => LoadIcon(SIMPLE);
        public static Texture2D LoadSquareIcon() => LoadIcon(SQUARE);
        public static Texture2D LoadCircleIcon() => LoadIcon(CIRCLE);
        public static Texture2D LoadCrossIcon() => LoadIcon(CROSS);
        
        private static Texture2D LoadIcon(string fileName, bool hasThemedSkin = false)
        {
            StringBuilder sb = new StringBuilder(fileName);

            if (hasThemedSkin && EditorGUIUtility.isProSkin)
            {
                sb.Insert(0, "d_");
            }

            string key = sb.ToString();
            
            if (!CachedIcons.ContainsKey(key))
            {
                CacheIcon(fileName, hasThemedSkin);
            }
            
            if (CachedIcons.ContainsKey(key))
            {
                return CachedIcons[key];
            }

            LDtkDebug.LogError($"Did not load the icon {key}");
            return null;
        }

        public static Texture GetUnityIcon(string name, string ending = " Icon")
        {
            string tilemapIcon = EditorGUIUtility.isProSkin ? $"d_{name}{ending}" : $"{name}{ending}";
            return EditorGUIUtility.IconContent(tilemapIcon).image;
        }
        
        public static Texture2D GetIconForLayerInstance(LayerInstance instance)
        {
            if (instance.IsIntGridLayer)
            {
                return LoadIntGridIcon();
            }
            if (instance.IsEntitiesLayer)
            {
                return LoadEntityIcon();
            }
            if (instance.IsTilesLayer)
            {
                return LoadTilesetIcon();
            }
            if (instance.IsAutoLayer)
            {
                return LoadAutoLayerIcon();
            }

            return null;
        }
        
    }
}