namespace LDtkUnity.Editor
{
    internal static class LDtkImporterConsts
    {
        public const int PROJECT_VERSION = 24;
        public const int LEVEL_VERSION = 8;
        public const string LDTK_JSON_VERSION = "1.3.0";

        public const string PROJECT_EXT = "ldtk";
        public const string LEVEL_EXT = "ldtkl";

        //choosing a high import order so that any prefab changes respond correctly for the import process, whether prefabs are modified internally in unity or externally (eg. source control)
        
        //Some discoveries about it to make entity prefabs work as expected: https://forum.unity.com/threads/create-additional-prefab-assets-in-scriptedimporter-and-track-them.1158734/#post-7792380
        //Import order is not documented much, but prefab order is 500/501 https://forum.unity.com/threads/understanding-import-order-of-native-unity-asset-types.1187845/
        //projects are imported first, so that separate levels can load the project's imported assets. levels will directly load the json for levels instead of loading the imported asset.
        
        //Edit: making prefabs import before projects
        public const int PROJECT_ORDER = 95;
        public const int LEVEL_ORDER = 99; //99 is the secret parallel import value
        
        public const int DEFAULT_PPU = 16;
    }
}