using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkTileCollection))]
    public class LDtkTileCollectionEditor : UnityEditor.Editor
    {
        private Object[] _tiles;

        public override void OnInspectorGUI()
        {
            _tiles = LDtkTileCollectionFactory.GetAllTiles((LDtkTileCollection)target);
            
            if (_tiles == null)
            {
                return;
            }
            
            GUIContent content = new GUIContent($"{_tiles.Length} Tiles");
            EditorGUILayout.LabelField(content);
        }
    }
}