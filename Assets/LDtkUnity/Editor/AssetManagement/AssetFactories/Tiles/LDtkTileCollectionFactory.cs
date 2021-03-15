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
        public static LDtkTileCollection CreateAndSaveTileCollection(Sprite[] sprites, string name, TileCreationAction action)
        {
            LDtkTileCollection collection = ScriptableObject.CreateInstance<LDtkTileCollection>();
            collection.name = name;

            if (!WriteToAssetDatabase(collection))
            {
                return null;
            }
            
            Tile[] tiles = LDtkTileFactory.GenerateTilesForSprites(sprites, action);
            AddTilesToAsset(collection, tiles);

            LDtkEditorUtil.Dirty(collection);
            
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
                startFrom = Path.GetDirectoryName(startFrom);
            }
            
            string directory = EditorUtility.OpenFolderPanel("Save Tile Collection", startFrom, "");

            if (string.IsNullOrEmpty(directory))
            {
                Debug.LogError("LDtk: Did not write tile collection asset, no path specified");
                return false;
            }

            
            //if the path involves a hidden unity folder (maybe symbolic link reasons), then it will break. Ensure crashes cannot happen
            directory += '/';
            if (directory.Contains("~/"))
            {
                Debug.LogError("LDtk: Chosen directory contains a '~' as the end of a folder name, which could break unity folder paths. Consider renaming the folder.");
                return false;
            }

            if (!directory.Contains(Application.dataPath))
            {
                Debug.LogError("LDtk: Chosen directory is outside the Unity project.");
                return false;
            }
            
            string fullPath = $"{directory}{tileCollection.name}.asset";

            fullPath = LDtkPathUtil.AbsolutePathToAssetsPath(fullPath);

            AssetDatabase.CreateAsset(tileCollection, fullPath);
            return true;

            //DeleteExisting(directory);
        }
        
        
        
        private static void AddTilesToAsset(LDtkTileCollection obj, Tile[] tiles)
        {
            if (tiles == null || tiles.Length <= 0)
            {
                Debug.LogWarning("No tiles added");
                return;
            }
            
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
    }
}