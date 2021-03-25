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
            LDtkTileCollection collection = GetOrCreateAsset(name);

            if (collection == null)
            {
                return null;
            }
            
            Tile[] tiles = LDtkTileFactory.GenerateTilesForSprites(sprites, action);
            AddTilesToAsset(collection, tiles);

            LDtkEditorUtil.Dirty(collection);
            
            return collection;
        }

        private static string GetDirectoryOfSelectedPath()
        {
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
                return "";
            }

            //if the path involves a hidden unity folder (maybe symbolic link reasons), then it will break. Ensure crashes cannot happen
            directory += '/';
            if (directory.Contains("~/"))
            {
                Debug.LogError("LDtk: Chosen directory contains a '~' as the end of a folder name, which could break unity folder paths. Consider renaming the folder.");
                return "";
            }

            if (!directory.Contains(Application.dataPath))
            {
                Debug.LogError("LDtk: Chosen directory is outside the Unity project.");
                return "";
            }
            
            return directory;
        }

        //overwrite if the asset already exists
        private static LDtkTileCollection GetOrCreateAsset(string name)
        {

            string directory = GetDirectoryOfSelectedPath();
            if (string.IsNullOrEmpty(directory))
            {
                return null;
            }
            
            //formulate path
            string fullPath = $"{directory}{name}.asset";
            fullPath = LDtkPathUtil.AbsolutePathToAssetsPath(fullPath);

            LDtkTileCollection tileCollection = (LDtkTileCollection)AssetDatabase.LoadMainAssetAtPath(fullPath);
            
            //if the asset didnt exist, then save a copy
            if (tileCollection == null)
            {
                tileCollection = ScriptableObject.CreateInstance<LDtkTileCollection>();
                tileCollection.name = name;
                
                AssetDatabase.CreateAsset(tileCollection, fullPath);
            }
            
            //safety check in case something went wrong
            if (tileCollection == null)
            {
                Debug.Log("tileCollection null");
            }
            
            return tileCollection;
        }

        //INSTEAD OF DELETING THE Tile ASSET, MAINTAIN THE REFERENCE for scene-Tilemaps BY JUST REPLACING THE OLD SPRITES AND MAINTAIN THE SPRITES IF POSSIBLE
        private static void DeleteAllObjectsFromCollection()
        {
            
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
            
            //clear in case this is a overwrite
            prop.ClearArray();
            //AssetDatabase.RemoveObjectFromAsset();
            
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