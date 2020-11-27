using UnityEngine;

namespace LDtkUnity.Editor.AssetLoading
{
    public static class LDtkIconLoader
    {
        private const string ROOT = "LDtkUnity/Icons/";
        
        private const string PROJECT = "LDtkProjectIcon.png";
        private const string ENTITY = "EntityIcon.png";
        private const string TILESET = "TilesetIcon.png";
        private const string AUTO_LAYER = "AutoLayerIcon.png";
        private const string FILE = "FileIcon.png";
        private const string ENUM = "EnumIcon.png";
        private const string WORLD = "WorldIcon.png";
        private const string INT_GRID = "IntGridIcon.png";

        public static Texture2D LoadProjectIcon() => LoadIcon(PROJECT);
        public static Texture2D LoadEntityIcon() => LoadIcon(ENTITY);
        public static Texture2D LoadTilesetIcon() => LoadIcon(TILESET);
        public static Texture2D LoadAutoLayerIcon() => LoadIcon(AUTO_LAYER);
        public static Texture2D LoadFileIcon() => LoadIcon(FILE);
        public static Texture2D LoadEnumIcon() => LoadIcon(ENUM);
        public static Texture2D LoadWorldIcon() => LoadIcon(WORLD);
        public static Texture2D LoadIntGridIcon() => LoadIcon(INT_GRID);

        private static Texture2D LoadIcon(string path) => LDtkEditorAssetLoader.Load<Texture2D>(ROOT + path);
    }
}