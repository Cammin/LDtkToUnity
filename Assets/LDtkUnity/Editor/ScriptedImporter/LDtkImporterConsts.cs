namespace LDtkUnity.Editor
{
    public static class LDtkImporterConsts
    {
        public const int PROJECT_VERSION = 12;
        public const int LEVEL_VERSION = 0;

        public const string PROJECT_EXT = "ldtk";
        public const string LEVEL_EXT = "ldtkl";

        //choosing a high import order so that any prefab changes respond correctly for the import process, whether prefabs are modified internally in unity or externally (eg. source control)
        //import order is not documented much, but prefab order is 500/501 https://forum.unity.com/threads/understanding-import-order-of-native-unity-asset-types.1187845/
        public const int LEVEL_ORDER = 10;
        public const int PROJECT_ORDER = 502;
        
        public const int DEFAULT_PPU = 16;
    }
}