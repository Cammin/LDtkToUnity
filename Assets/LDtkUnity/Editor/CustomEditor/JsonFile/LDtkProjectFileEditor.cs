using System.Linq;
using UnityEditor;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProjectFile))]
    public class LDtkProjectFileEditor : LDtkJsonFileEditor<LdtkJson>
    {
        protected override void DrawInspectorGUI(LdtkJson project)
        {
            DrawVersion(project);

            Level[] levels = project.Levels;
            
            if (levels == null)
            {
                return;
            }
            
            DrawCountOfItems(levels.Length, "Level", "Levels");
            
            DrawDefinitions(project.Defs);
        }
        
        private void DrawDefinitions(Definitions defs)
        {
            DrawCountOfItems(defs.Layers.Length, 
                "Layer", "Layers");
            DrawCountOfItems(defs.LevelFields.Length, 
                "Level Fields", "Level Fields");
            
            DrawCountOfItems(defs.Entities.Length, 
                "Entity", "Entities");
            DrawCountOfItems(defs.Entities.SelectMany(p => p.FieldDefs).Count(), 
                "Entity Field", "Entity Fields");
            
            DrawCountOfItems(defs.Enums.Length, 
                "Enum", "Enums");
            DrawCountOfItems(defs.Enums.SelectMany(p => p.Values).Count(), 
                "Enum Value", "Enum Values");
            
            DrawCountOfItems(defs.Tilesets.Length, 
                "Tileset", "Tilesets");
        }

        private static void DrawVersion(LdtkJson project)
        {
            string version = $"Json Version: {project.JsonVersion}";
            EditorGUILayout.LabelField(version);
        }
        
        private void DrawCountOfItems(int count, string single, string plural)
        {
            string naming = count == 1 ? single : plural;
            EditorGUILayout.LabelField($"{count} {naming}");
        }
    }
}