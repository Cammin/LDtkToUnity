using UnityEditor;

namespace LDtkUnity.Editor.AssetManagement
{
    public class LDtkSerializedProjectAssets
    {
        public readonly SerializedObject Object;
        
        public readonly SerializedObject JsonProject;
        
        public readonly SerializedProperty IntGridArray;
        public readonly SerializedObject TilesetArray;
        public readonly SerializedObject EntityArray;
        
        public readonly SerializedObject Grid;
        public readonly SerializedObject IntGridTilesVisible;

        public LDtkSerializedProjectAssets(SerializedObject o, SerializedObject jsonProject, SerializedProperty intGridArray, SerializedObject tilesetArray, SerializedObject entityArray, SerializedObject grid, SerializedObject intGridTilesVisible)
        {
            Object = o;
            JsonProject = jsonProject;
            IntGridArray = intGridArray;
            TilesetArray = tilesetArray;
            EntityArray = entityArray;
            Grid = grid;
            IntGridTilesVisible = intGridTilesVisible;
        }
    }
}