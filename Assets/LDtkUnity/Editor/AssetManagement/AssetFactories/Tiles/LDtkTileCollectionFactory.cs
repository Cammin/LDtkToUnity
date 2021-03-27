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
            LDtkTileCollection collection = GetOrCreateAsset(name);

            if (collection == null)
            {
                return null;
            }
            
            Tile[] tiles = LDtkTileFactory.GenerateTilesForSprites(sprites, action);
            AddOrOverwriteTilesToCollection(collection, tiles.ToList());
            
            SerializeAssetListFromRepresentations(collection);

            LDtkEditorUtil.Dirty(collection);
            
            return collection;
        }

        //overwrite if the asset already exists
        private static LDtkTileCollection GetOrCreateAsset(string name)
        {

            string directory = LDtkPathUtil.GetDirectoryOfSelectedPath("Save Tile Collection");
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

        private static void AddOrOverwriteTilesToCollection(LDtkTileCollection tileCollection, List<Tile> inputTiles)
        {
            List<Tile> currentTilesInAsset = GetAllTiles(tileCollection).Cast<Tile>().ToList();
            
            //old tiles are what already exists in the tile collection.
            //new tiles are what was made from a fresh sprite sheet, and ready to replace/create, and then delete 

            foreach (Tile inputTile in inputTiles)
            {
                Tile tile = GetNewOrCurrentTileFromCollection(tileCollection, currentTilesInAsset, inputTile);
                
                //-if any old tile assets have a matching name, change their sprite to the new one we are comparing their name with
                tile.sprite = inputTile.sprite;

            }
            
            //-afterward, delete(clean) any old assets that had no name related to the new ones.
            List<Tile> oldUnusedTiles = currentTilesInAsset.Where(currentTile => inputTiles.All(inputTile => currentTile.name != inputTile.name)).ToList();
            foreach (Tile oldUnusedTile in oldUnusedTiles)
            {
                AssetDatabase.RemoveObjectFromAsset(oldUnusedTile);
            }

        }

        private static Tile GetNewOrCurrentTileFromCollection(LDtkTileCollection tileCollection, List<Tile> currentTilesInAsset, Tile inputTile)
        {
            //solve tiles by matching name.
            Tile matchingCurrentTile = currentTilesInAsset.FirstOrDefault(oldTile => oldTile.name == inputTile.name);

            //there is a matching name, UPDATE
            if (matchingCurrentTile != null)
            {
                return matchingCurrentTile;
            }

            //found with no matching name, CREATE
            AssetDatabase.AddObjectToAsset(inputTile, tileCollection);
            return inputTile;
        }

        private static void SerializeAssetListFromRepresentations(LDtkTileCollection obj)
        {

            
            SerializedObject sObj = new SerializedObject(obj);
            SerializedProperty prop = sObj.FindProperty(LDtkTileCollection.PROP_TILE_LIST);

            Object[] tiles = GetAllTiles(obj);
            
            //clear in case this is a overwrite
            prop.ClearArray();

            prop.arraySize = tiles.Length;
            for (int i = 0; i < tiles.Length; i++)
            {
                Object tile = tiles[i];

                SerializedProperty element = prop.GetArrayElementAtIndex(i);
                element.objectReferenceValue = tile;
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