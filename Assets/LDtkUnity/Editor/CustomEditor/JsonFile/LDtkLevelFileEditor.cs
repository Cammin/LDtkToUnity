using System.Linq;
using UnityEditor;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelFile))]
    public class LDtkLevelFileEditor : LDtkJsonFileEditor<Level>
    {
        private int? _layerCount = null;
        private int? _intGridValueCount = null;
        private int? _autoTileCount = null;
        private int? _gridTileCount = null;
        private int? _entityCount = null;
        
        protected override void DrawInspectorGUI(Level level)
        {
            LayerInstance[] layers = level.LayerInstances;
            
            if (layers == null)
            {
                return;
            }

            DrawLayerCount(layers);
            DrawIntGridValueCount(layers);
            DrawAutoTileCount(layers);
            DrawGridTileCount(layers);
            DrawEntityCount(layers);
        }

        private void DrawEntityCount(LayerInstance[] layers)
        {
            _entityCount = layers.Where(p => p.IsEntitiesLayer).SelectMany(p => p.EntityInstances)
                .Count();
            string entityName = _entityCount == 1 ? "Entity" : "Entities";
            EditorGUILayout.LabelField($"{_entityCount} {entityName}");
        }

        private void DrawGridTileCount(LayerInstance[] layers)
        {
            _gridTileCount = layers.Where(p => p.IsTilesLayer).SelectMany(p => p.GridTiles)
                .Count();
            string tileName = _gridTileCount == 1 ? "Grid Tile" : "Grid Tiles";
            EditorGUILayout.LabelField($"{_gridTileCount} {tileName}");
        }

        private void DrawAutoTileCount(LayerInstance[] layers)
        {
            if (_autoTileCount == null)
            {
                _autoTileCount = layers.Where(p => p.IsAutoLayer).SelectMany(p => p.AutoLayerTiles)
                    .Count();
            }
            string tileName = _autoTileCount == 1 ? "Auto Tile" : "Auto Tiles";
            EditorGUILayout.LabelField($"{_autoTileCount} {tileName}");
        }

        private void DrawIntGridValueCount(LayerInstance[] layers)
        {
            if (_intGridValueCount == null)
            {
                _intGridValueCount = layers.Where(p => p.IsIntGridLayer).Select(p => p.IntGridValueCount).Sum();
            }
            string intGridValueName = _intGridValueCount == 1 ? "Int Grid Value" : "Int Grid Values";
            EditorGUILayout.LabelField($"{_intGridValueCount} {intGridValueName}");
        }

        private void DrawLayerCount(LayerInstance[] layers)
        {
            if (_layerCount == null)
            {
                _layerCount = layers.Length;
            }
            string layerName = _layerCount == 1 ? "Layer" : "Layers";
            EditorGUILayout.LabelField($"{_layerCount} {layerName}");
        }
    }
}