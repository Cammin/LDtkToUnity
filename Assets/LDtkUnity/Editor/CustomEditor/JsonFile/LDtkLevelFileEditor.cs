using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelFile))]
    internal sealed class LDtkLevelFileEditor : LDtkJsonFileEditor<Level>
    {
        private int? _layerCount = null;
        private int? _intGridValueCount = null;
        private int? _autoTileCount = null;
        private int? _gridTileCount = null;
        private int? _entityCount = null;

        protected override Texture2D StaticPreview => LDtkIconUtility.LoadLevelIcon();

        private void OnEnable()
        {
            TryCacheJson();
            Tree = new LDtkTreeViewWrapper(JsonData);
            InitValues();
        }
        
        private void InitValues()
        {
            LayerInstance[] layers = JsonData.LayerInstances;
            
            _entityCount = layers.Where(p => p.IsEntitiesLayer).SelectMany(p => p.EntityInstances).Count();
            _gridTileCount = layers.Where(p => p.IsTilesLayer).SelectMany(p => p.GridTiles).Count();
            _autoTileCount = layers.Where(p => p.IsAutoLayer).SelectMany(p => p.AutoLayerTiles).Count();
            _intGridValueCount = layers.Where(p => p.IsIntGridLayer).Select(p => p.IntGridValueCount).Sum();
            _layerCount = layers.Length;
        }
        
        protected override void DrawInspectorGUI()
        {
            DrawCountOfItems(_layerCount, "Layer", "Layers", LDtkIconUtility.LoadLayerIcon());
            DrawCountOfItems(_intGridValueCount, "Int Grid Value", "Int Grid Values", LDtkIconUtility.LoadIntGridIcon());
            DrawCountOfItems(_entityCount, "Entity", "Entities", LDtkIconUtility.LoadEntityIcon());
            DrawCountOfItems(_gridTileCount, "Grid Tile", "Grid Tiles", LDtkIconUtility.LoadTilesetIcon());
            DrawCountOfItems(_autoTileCount, "Auto Tile", "Auto Tiles", LDtkIconUtility.LoadAutoLayerIcon());
        }
    }
}