using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProjectFile))]
    internal sealed class LDtkProjectFileEditor : LDtkJsonFileEditor<LdtkJson>
    {
        private string _jsonVersion = null;
        private int? _worldCount = null;
        private int? _levelCount = null;
        private int? _layerCount = null;
        private int? _levelFieldsCount = null;
        private int? _entityCount = null;
        private int? _entityFieldsCount = null;
        private int? _enumCount = null;
        private int? _enumValueCount = null;
        private int? _tilesetCount = null;

        protected override Texture2D StaticPreview => LDtkIconUtility.LoadListIcon();

        private void OnEnable()
        {
            TryCacheJson();
            InitTree();
            InitValues();
        }

        private void InitTree()
        {
            string path = AssetDatabase.GetAssetPath(target);
            string assetName = Path.GetFileNameWithoutExtension(path);
            Tree = new LDtkTreeViewWrapper(JsonData, assetName);
        }

        private void InitValues()
        {
            Definitions defs = JsonData.Defs;

            _jsonVersion = JsonData.JsonVersion;
            _worldCount = JsonData.UnityWorlds.Length;
            _levelCount = JsonData.UnityWorlds.SelectMany(p => p.Levels).Count();
            _levelFieldsCount = defs.LevelFields.Length;
            _layerCount = defs.Layers.Length;
            _entityCount = defs.Entities.Length;
            _entityFieldsCount = defs.Entities.SelectMany(p => p.FieldDefs).Count();
            _enumCount = defs.Enums.Length;
            _enumValueCount = defs.Enums.SelectMany(p => p.Values).Count();
            _tilesetCount = defs.Tilesets.Length;
        }

        protected override void DrawInspectorGUI()
        {
            if (JsonData.ExternalLevels)
            {
                EditorGUILayout.HelpBox($"This json asset does not contain any level layer instances due to using external levels. Deserialize the specific levels instead.", MessageType.Info);
                LDtkEditorGUIUtility.DrawDivider();
            }
            
            DrawText($"Json Version: {_jsonVersion}", LDtkIconUtility.LoadListIcon());
            
            DrawCountOfItems(_worldCount, "World", "Worlds", LDtkIconUtility.LoadWorldIcon());
            
            DrawCountOfItems(_levelCount, "Level", "Levels", LDtkIconUtility.LoadLevelIcon());
            if (_levelCount > 0)
            {
                DrawCountOfItems(_levelFieldsCount, "Level Fields", "Level Fields", LDtkIconUtility.LoadLevelIcon());
            }
            
            DrawCountOfItems(_layerCount, "Layer Definition", "Layer Definitions", LDtkIconUtility.LoadLayerIcon());
            
            DrawCountOfItems(_entityCount, "Entity Definition", "Entity Definitions", LDtkIconUtility.LoadEntityIcon());
            if (_entityFieldsCount > 0)
            {
                DrawCountOfItems(_entityFieldsCount, "Entity Field", "Entity Fields", LDtkIconUtility.LoadEntityIcon());
            }
            
            DrawCountOfItems(_enumCount, "Enum", "Enums", LDtkIconUtility.LoadEnumIcon());
            if (_enumValueCount > 0)
            {
                DrawCountOfItems(_enumValueCount, "Enum Value", "Enum Values", LDtkIconUtility.LoadEnumIcon());
            }
            
            DrawCountOfItems(_tilesetCount, "Tileset", "Tilesets", LDtkIconUtility.LoadTilesetIcon());
        }
    }
}