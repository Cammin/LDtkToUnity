using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkIconLoader
    {
        private const string ROOT = "Icons/";
        
        private const string PROJECT = "ProjectIcon";
        private const string LEVEL = "LevelIcon";
        private const string SIMPLE = "SimpleIcon";
        private const string ENTITY = "EntityIcon";
        private const string TILESET = "TilesetIcon";
        private const string AUTO_LAYER = "AutoLayerIcon";
        private const string FILE = "FileIcon";
        private const string ENUM = "EnumIcon";
        private const string WORLD = "WorldIcon";
        private const string INT_GRID = "IntGridIcon";

        private static Texture2D _cachedProjectIcon;
        private static Texture2D _cachedLevelIcon;
        private static Texture2D _cachedSimpleIcon;
        private static Texture2D _cachedEntityIcon;
        private static Texture2D _cachedTilesetIcon;
        private static Texture2D _cachedAutoLayerIcon;
        private static Texture2D _cachedFileIcon;
        private static Texture2D _cachedEnumIcon;
        private static Texture2D _cachedWorldIcon;
        private static Texture2D _cachedIntGridIcon;
        
        public static Texture2D LoadProjectIcon() => LoadIcon(PROJECT, _cachedProjectIcon);
        public static Texture2D LoadLevelIcon() => LoadIcon(LEVEL, _cachedLevelIcon);
        public static Texture2D LoadSimpleIcon() => LoadIcon(SIMPLE, _cachedSimpleIcon);
        public static Texture2D LoadFileIcon() => LoadIcon(FILE, _cachedFileIcon);
        
        public static Texture2D LoadAutoLayerIcon() => LoadIcon(AUTO_LAYER, _cachedAutoLayerIcon, true);
        public static Texture2D LoadEntityIcon() => LoadIcon(ENTITY, _cachedEntityIcon, true);
        public static Texture2D LoadEnumIcon() => LoadIcon(ENUM, _cachedEnumIcon, true);
        public static Texture2D LoadIntGridIcon() => LoadIcon(INT_GRID, _cachedIntGridIcon, true);
        public static Texture2D LoadTilesetIcon() => LoadIcon(TILESET, _cachedTilesetIcon, true);
        public static Texture2D LoadWorldIcon() => LoadIcon(WORLD, _cachedWorldIcon, true);

        private static Texture2D LoadIcon(string fileName, Texture2D cached, bool lightThemeSkinPossible = false)
        {
            if (cached == null)
            {
                bool isLightTheme = lightThemeSkinPossible && !EditorGUIUtility.isProSkin;
                string darkTheme = isLightTheme ? "" : "d_";
                string path = $"{ROOT}{darkTheme}{fileName}.png";
                cached = LDtkInternalLoader.Load<Texture2D>(path);
            }
            return cached;
        }

        //TODO eventually get this ran to prevent too much memory being used (though the textures are pretty small anyways)
        public static void Dispose()
        {
            _cachedProjectIcon = null;
            _cachedLevelIcon = null;
            _cachedSimpleIcon = null;
            _cachedEntityIcon = null;
            _cachedTilesetIcon = null;
            _cachedAutoLayerIcon = null;
            _cachedFileIcon = null;
            _cachedEnumIcon = null;
            _cachedWorldIcon = null;
            _cachedIntGridIcon = null;
        }

        public static Texture GetUnityIcon(string name)
        {
            string tilemapIcon = EditorGUIUtility.isProSkin ? $"d_{name} Icon" : $"{name} Icon";
            return EditorGUIUtility.IconContent(tilemapIcon).image;
        }
    }
}