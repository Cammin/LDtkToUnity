namespace LDtkUnity.Editor
{
    public static class LDtkImporterConsts
    {
        public const int PROJECT_VERSION = 4;
        public const int LEVEL_VERSION = 0;

        public const string PROJECT_EXT = "ldtk";
        public const string LEVEL_EXT = "ldtkl";

        //choosing a negative import order so that any prefab changes respond correctly for the import process.
        public const int LEVEL_ORDER = -11;
        public const int PROJECT_ORDER = -10;
    }
}