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
            _tiles = GetAllTiles((LDtkTileCollection)target);
            
            if (_tiles == null)
            {
                return;
            }
            
            GUIContent content = new GUIContent($"{_tiles.Length} Tiles");
            EditorGUILayout.LabelField(content);
        }
        
        public static Object[] GetAllTiles(LDtkTileCollection obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return AssetDatabase.LoadAllAssetRepresentationsAtPath(path).ToArray();
        }

        public static void SaveAllTiles(LDtkTileCollection obj, Tile[] tiles)
        {
            SerializedObject sObj = new SerializedObject(obj);
            SerializedProperty prop = sObj.FindProperty(LDtkTileCollection.PROP_TILE_LIST);

            prop.arraySize = tiles.Length;
            for (int i = 0; i < tiles.Length; i++)
            {
                Tile tile = tiles[i];
                AssetDatabase.AddObjectToAsset(tile, obj);
                
                SerializedProperty element = prop.GetArrayElementAtIndex(i);
                element.objectReferenceValue = tiles[i];
            }

            sObj.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}