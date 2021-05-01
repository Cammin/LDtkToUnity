using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkTileCollectionFactory
    {
        private readonly LDtkTileCollectionFactoryParts[] _srcParts;
        private readonly string _name;
        private readonly TileCreationAction _action;

        private List<Tile> _inputTiles;
        
        public LDtkArtifactAssets Collection { get; private set; }

        public LDtkTileCollectionFactory(LDtkTileCollectionFactoryParts[] srcParts, string name, TileCreationAction action)
        {
            _srcParts = srcParts;
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
            
            _inputTiles = GenerateTilesForSprites().ToList();
            
            AddOrOverwriteTilesToCollection();
            
            SerializeAssetListFromRepresentations();

            LDtkEditorUtil.Dirty(Collection);
        }

        private Tile[] GenerateTilesForSprites()
        {
            if (_srcParts == null || _srcParts.Length == 0)
            {
                Debug.LogError("Sprite array is null");
                return null;
            }

            return _srcParts.Select(_action.Invoke).ToArray();
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
        private LDtkArtifactAssets GetOrCreateAsset()
        {

            string directory = LDtkPathUtility.GetDirectoryOfSelectedPath("Save Tile Collection");
            if (string.IsNullOrEmpty(directory))
            {
                return null;
            }
            
            //formulate path
            string fullPath = $"{directory}{_name}.asset";
            fullPath = LDtkPathUtility.AbsolutePathToAssetsPath(fullPath);

            LDtkArtifactAssets artifactAssets = (LDtkArtifactAssets)AssetDatabase.LoadMainAssetAtPath(fullPath);
            
            //if the asset didnt exist, then save a copy
            if (artifactAssets == null)
            {
                artifactAssets = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
                artifactAssets.name = _name;
                
                AssetDatabase.CreateAsset(artifactAssets, fullPath);
            }
            
            //safety check in case something went wrong
            if (artifactAssets == null)
            {
                Debug.Log("tileCollection null");
            }
            
            return artifactAssets;
        }

        private void AddOrOverwriteTilesToCollection()
        {
            List<Tile> currentTilesInAsset = GetAllTiles(Collection).Cast<Tile>().ToList();

            foreach (Tile inputTile in _inputTiles)
            {
                Tile tile = GetNewOrCurrentTileFromCollection(currentTilesInAsset, inputTile);
                
                if (tile.sprite == inputTile.sprite)
                {
                    continue;
                }
                tile.sprite = inputTile.sprite;
            }
            
            //afterward, delete(clean) any old assets that had no name related to the new ones.
            List<Tile> oldUnusedTiles = currentTilesInAsset.Where(currentTile => _inputTiles.All(inputTile => currentTile.name != inputTile.name)).ToList();
            foreach (Tile oldUnusedTile in oldUnusedTiles)
            {
                AssetDatabase.RemoveObjectFromAsset(oldUnusedTile);
            }

        }

        private Tile GetNewOrCurrentTileFromCollection(List<Tile> currentTilesInAsset, Tile inputTile)
        {
            Tile matchingCurrentTile = currentTilesInAsset.FirstOrDefault(oldTile => oldTile.name == inputTile.name);
            
            if (matchingCurrentTile != null)
            {
                return matchingCurrentTile;
            }
            
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
            SerializedProperty prop = sObj.FindProperty(LDtkArtifactAssets.PROP_TILE_LIST);

            //important to save assets before trying to serialize them into a list
            AssetDatabase.SaveAssets();
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

        public static Object[] GetAllTiles(LDtkArtifactAssets obj)
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