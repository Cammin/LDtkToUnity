using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkTileCollectionFactory
    {
        private readonly Sprite[] _srcSprites;
        private readonly string _name;
        private readonly TileCreationAction _action;

        private List<Tile> _inputTiles;
        
        public LDtkTileCollection Collection { get; private set; }
        
        public LDtkTileCollectionFactory(Sprite[] srcSprites, string name, TileCreationAction action)
        {
            _srcSprites = srcSprites;
            _name = name;
            _action = action;
        }

        public void CreateAndSaveTileCollection()
        {
            Collection = GetOrCreateAsset();

            if (Collection == null)
            {
                return;
            }
            
            _inputTiles = LDtkTileFactory.GenerateTilesForSprites(_srcSprites, _action).ToList();
            AddOrOverwriteTilesToCollection();
            
            SerializeAssetListFromRepresentations();

            LDtkEditorUtil.Dirty(Collection);
        }

        public bool SaveAssetsAndPingIfSuccessful()
        {
            if (!Collection)
            {
                return false;
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(Collection);

            return true;
        }

        //overwrite if the asset already exists
        private LDtkTileCollection GetOrCreateAsset()
        {

            string directory = LDtkPathUtil.GetDirectoryOfSelectedPath("Save Tile Collection");
            if (string.IsNullOrEmpty(directory))
            {
                return null;
            }
            
            //formulate path
            string fullPath = $"{directory}{_name}.asset";
            fullPath = LDtkPathUtil.AbsolutePathToAssetsPath(fullPath);

            LDtkTileCollection tileCollection = (LDtkTileCollection)AssetDatabase.LoadMainAssetAtPath(fullPath);
            
            //if the asset didnt exist, then save a copy
            if (tileCollection == null)
            {
                tileCollection = ScriptableObject.CreateInstance<LDtkTileCollection>();
                tileCollection.name = _name;
                
                AssetDatabase.CreateAsset(tileCollection, fullPath);
            }
            
            //safety check in case something went wrong
            if (tileCollection == null)
            {
                Debug.Log("tileCollection null");
            }
            
            return tileCollection;
        }

        private void AddOrOverwriteTilesToCollection()
        {
            
            
            List<Tile> currentTilesInAsset = GetAllTiles(Collection).Cast<Tile>().ToList();
            
            //old tiles are what already exists in the tile collection.
            //new tiles are what was made from a fresh sprite sheet, and ready to replace/create, and then delete 

            foreach (Tile inputTile in _inputTiles)
            {
                Tile tile = GetNewOrCurrentTileFromCollection(currentTilesInAsset, inputTile);
                
                //-if any old tile assets have a matching name, change their sprite to the new one we are comparing their name with
                if (tile.sprite == inputTile.sprite)
                {
                    //Debug.Log($"Tile sprite {tile.sprite.name} already matches the sprite asset");
                    continue;
                }
                
                //Debug.Log($"Update tile sprite {tile.sprite.name} into {inputTile.name}");
                tile.sprite = inputTile.sprite;

            }
            
            //afterward, delete(clean) any old assets that had no name related to the new ones.
            List<Tile> oldUnusedTiles = currentTilesInAsset.Where(currentTile => _inputTiles.All(inputTile => currentTile.name != inputTile.name)).ToList();
            foreach (Tile oldUnusedTile in oldUnusedTiles)
            {
                //Debug.Log($"Removed obsolete asset {oldUnusedTile.name}");
                AssetDatabase.RemoveObjectFromAsset(oldUnusedTile);
            }

        }

        private Tile GetNewOrCurrentTileFromCollection(List<Tile> currentTilesInAsset, Tile inputTile)
        {
            //solve tiles by matching name.
            Tile matchingCurrentTile = currentTilesInAsset.FirstOrDefault(oldTile => oldTile.name == inputTile.name);

            //there is a matching name, UPDATE
            if (matchingCurrentTile != null)
            {
                return matchingCurrentTile;
            }

            //found with no matching name, CREATE
            AssetDatabase.AddObjectToAsset(inputTile, Collection);
            return inputTile;
        }

        private void SerializeAssetListFromRepresentations()
        {
            if (Collection == null)
            {
                Debug.LogError("Collection null");
                return;
            }
            
            SerializedObject sObj = new SerializedObject(Collection);
            SerializedProperty prop = sObj.FindProperty(LDtkTileCollection.PROP_TILE_LIST);

            Object[] tiles = GetAllTiles(Collection);
            
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
            if (obj == null)
            {
                Debug.LogError("Collection null");
                return default;
            }
            
            string path = AssetDatabase.GetAssetPath(obj);
            return AssetDatabase.LoadAllAssetRepresentationsAtPath(path).ToArray();
        }
    }
}