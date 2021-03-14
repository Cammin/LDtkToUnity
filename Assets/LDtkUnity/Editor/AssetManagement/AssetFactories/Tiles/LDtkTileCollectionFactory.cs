using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public static class LDtkTileCollectionFactory
    {
        public static LDtkTileCollection CreateAndSaveTileCollection(Texture2D texture)
        {
            LDtkTileCollection collection = ScriptableObject.CreateInstance<LDtkTileCollection>();
            collection.name = texture.name + "_Tiles";

            if (!WriteToAssetDatabase(collection))
            {
                return null;
            }
            
            AddTilesToAssetFromTexture(collection, texture);
            
            return collection;

        }
        
        

        
        
        private static bool WriteToAssetDatabase(LDtkTileCollection tileCollection)
        {
            if (tileCollection == null)
            {
                Debug.Log("tileCollection null");
                return false;
            }

            string startFrom = Application.dataPath;
            if (AssetDatabase.Contains(Selection.activeObject))
            {
                string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                startFrom = LDtkPathUtil.AssetsPathToAbsolutePath(assetPath);
            }
            
            string directory = EditorUtility.OpenFolderPanel("Save Tile Collection", startFrom, "");

            if (string.IsNullOrEmpty(directory))
            {
                Debug.LogError("Did not write tile collection asset, no path specified");
                return false;
            }
            
            string fullPath = $"{directory}/{tileCollection.name}.asset";

            fullPath = LDtkPathUtil.AbsolutePathToAssetsPath(fullPath);

            AssetDatabase.CreateAsset(tileCollection, fullPath);
            return true;

            //DeleteExisting(directory);
        }
        
        private static void AddTilesToAssetFromTexture(LDtkTileCollection obj, Texture2D texture)
        {
            Tile[] tiles = LDtkTileFactory.GenerateTilesForTextureMetas(texture);
            AddTilesToAsset(obj, tiles);
        }
        
        private static void AddTilesToAsset(LDtkTileCollection obj, Tile[] tiles)
        {
            SerializedObject sObj = new SerializedObject(obj);
            SerializedProperty prop = sObj.FindProperty(LDtkTileCollection.PROP_TILE_LIST);

            prop.arraySize = tiles.Length;
            for (int i = 0; i < tiles.Length; i++)
            {
                Tile tile = tiles[i];
                AssetDatabase.AddObjectToAsset(tile, obj);
                
                SerializedProperty element = prop.GetArrayElementAtIndex(i);
                element.objectReferenceValue = tile;

                EditorUtility.SetDirty(tile);
                
            }

            sObj.ApplyModifiedProperties();

        }


        
        public static Object[] GetAllTiles(LDtkTileCollection obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return AssetDatabase.LoadAllAssetRepresentationsAtPath(path).ToArray();
        }


        private static void DeleteExisting(string directory)
        {
            string[] oldTiles = AssetDatabase.FindAssets("t:Tile", new[] {directory});
            string[] paths = oldTiles.Select(AssetDatabase.GUIDToAssetPath).ToArray();

            List<string> errorPaths = new List<string>();
            bool deleteAssets = AssetDatabase.DeleteAssets(paths, errorPaths);
            if (!deleteAssets)
            {
                Debug.Log("Delete problems " + errorPaths.Count);
            }
        }
    }
}