namespace LDtkUnity.Editor
{
    internal static class LDtkImporterConsts
    {
        public const int PROJECT_VERSION = 30;
        public const int LEVEL_VERSION = 14;
        public const int TILESET_VERSION = 5;
        public const string MINIMUM_JSON_VERSION = "1.5.0";
        public const string EXPORT_APP_VERSION_REQUIRED = "1.5.3.0";

        public const string PROJECT_EXT = "ldtk";
        public const string LEVEL_EXT = "ldtkl";
        public const string TILESET_EXT = "ldtkt";

        public const int DEFAULT_PPU = 16;
        private const int SCRIPTED_IMPORTER_ORDER = 1000;
        
        //Some discoveries about it to make entity prefabs work as expected: https://forum.unity.com/threads/create-additional-prefab-assets-in-scriptedimporter-and-track-them.1158734/#post-7792380
        //projects are imported first, so that separate levels can load the project's imported assets. levels will directly load the json for levels instead of loading the imported asset.
        
        //Import order https://forum.unity.com/threads/understanding-import-order-of-native-unity-asset-types.1187845/#post-9171509
        //Important to reimport before prefabs (1500)
        //99 is the secret parallel import value, but doesnt appear to work. maybe in a future update
        public const int TILESET_ORDER = 1094 - SCRIPTED_IMPORTER_ORDER;
        public const int PROJECT_ORDER = 1095 - SCRIPTED_IMPORTER_ORDER;
        public const int LEVEL_ORDER = 1099 - SCRIPTED_IMPORTER_ORDER; 
        
    }
}