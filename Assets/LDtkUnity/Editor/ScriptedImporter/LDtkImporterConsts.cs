namespace LDtkUnity.Editor
{
    public static class LDtkImporterConsts
    {
        public const int PROJECT_VERSION = 11;
        public const int LEVEL_VERSION = 0;

        public const string PROJECT_EXT = "ldtk";
        public const string LEVEL_EXT = "ldtkl";

        //choosing a high import order so that any prefab changes respond correctly for the import process, whether prefabs are modified internally in unity or externally (eg. source control)
        public const int LEVEL_ORDER = 10;
        public const int PROJECT_ORDER = 11;
        
        public const int DEFAULT_PPU = 16;
    }
}