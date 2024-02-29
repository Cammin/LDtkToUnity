using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkTilesetFile))]
    internal sealed class LDtkTilesetFileEditor : LDtkJsonFileEditor<LDtkTilesetDefinitionWrapper>
    {
        private string _relPath = null;
        private int? _tileCount = null;
        private int? _tags = null;
        private int? _enumTags = null;
        private int? _customData = null;

        protected override Texture2D StaticPreview => LDtkIconUtility.LoadTilesetIcon();

        private void OnEnable()
        {
            TryCacheJson();
            InitValues();
        }
        
        private void InitValues()
        {
            TilesetDefinition def = JsonData.Def;
            
            _relPath = def.RelPath;
            _tileCount = def.CWid * def.CHei;
            _tags = def.Tags.Length;
            _enumTags = def.EnumTags.Length;
            _customData = def.CustomData.Length;
        }
        
        protected override void DrawInspectorGUI()
        {
            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
            {
                DrawText($"{_relPath}", (Texture2D)LDtkIconUtility.GetUnityIcon("Folder"));
            }
            DrawCountOfItems(_tileCount, "Tile", "Tiles", LDtkIconUtility.LoadTilesetIcon());
            DrawCountOfItems(_tags, "Tag", "Tags", (Texture2D)LDtkIconUtility.GetUnityIcon("FilterByLabel", ""));
            DrawCountOfItems(_enumTags,"Enum Association", "Enum Associations", LDtkIconUtility.LoadEnumIcon());
            DrawCountOfItems(_customData, "Custom Data", "Custom Datas", LDtkIconUtility.LoadListIcon());
        }
    }
}