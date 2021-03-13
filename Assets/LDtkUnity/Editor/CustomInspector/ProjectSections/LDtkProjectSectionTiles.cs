using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionTiles : LDtkProjectSectionDrawer<TilesetDefinition>
    {
        public LDtkProjectSectionTiles(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override string PropertyName => LDtkProject.TILES;
        protected override string GuiText => "Tiles";
        protected override string GuiTooltip => "Tiles";
        protected override Texture2D GuiImage => (Texture2D)EditorGUIUtility.IconContent("Tile Icon").image;

        protected override void GetDrawers(TilesetDefinition[] defs, List<LDtkContentDrawer<TilesetDefinition>> drawers)
        {
            //do nothing for this
        }

        protected override int GetSizeOfArray(TilesetDefinition[] datas)
        {
            return -1;
        }

        protected override void DrawDropdownContent(TilesetDefinition[] datas)
        {
            GenerateTilesButton();



            string label = $"{ArrayProp.arraySize} Total Referenced Tiles";
            
            EditorGUILayout.LabelField(label);
            
        }

        private void GenerateTilesButton()
        {
            GUIContent buttonContent = new GUIContent()
            {
                text = "Generate Tiles",
                tooltip = "For each texture's sprites, generate a tile and save it as a reference in this project. Any old tiles are deleted in this process",
                //image = GuiImage
            };
            
            if (!GUILayout.Button(buttonContent))
            {
                return;
            }

            LDtkTileCollection[] collections = CreateTileCollectionsFromSerializedTextures();

            foreach (LDtkTileCollection collection in collections)
            {
                SaveTileCollectionToDatabase(collection);
                
            }
            
            //todo this is where we left off
            //LDtkTileCollectionEditor.SaveAllTiles(collections, tiles);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            SerializeNewTileFields(collections);
            
        }

        
        
        private LDtkTileCollection[] CreateTileCollectionsFromSerializedTextures()
        {
            Texture2D[] list = GetAssetsFromSection<Texture2D>(LDtkProject.TILESETS);
            
            return list.Select(CreateTileCollectionFromTexture).ToArray();
        }
        
        private LDtkTileCollection CreateTileCollectionFromTexture(Texture2D texture)
        {
            Tile[] tiles = LDtkTileFactory.GenerateTilesForTextureMetas(texture);
            LDtkTileCollection collection = CreateCollection(tiles, texture.name);
            return collection;
        }
        private LDtkTileCollection CreateCollection(Tile[] tiles, string objectName)
        {
            LDtkTileCollection collection = ScriptableObject.CreateInstance<LDtkTileCollection>();
            collection.name = objectName;
            
            return collection;
        }

        
        
        private void SaveTileCollectionToDatabase(LDtkTileCollection tileCollection)
        {
            if (tileCollection == null)
            {
                Debug.Log("tileCollection null");
                return;
            }

            string directory = "Assets";//LDtkPathUtil.SiblingDirectoryOfAsset(Project);
            
            //LDtkPathUtil.CreateDirectoryIfNotValidFolder(directory);
            
            //EditorUtility.DisplayProgressBar("Saving Tiles", "Deleting old tiles", 0);
            //destroy all previous ones. despite the warning that appears, it seems to work
            
            
            
            /*string[] oldTiles = AssetDatabase.FindAssets("t:Tile", new[] {directory});
            string[] paths = oldTiles.Select(AssetDatabase.GUIDToAssetPath).ToArray();

            List<string> errorPaths = new List<string>();
            bool deleteAssets = AssetDatabase.DeleteAssets(paths, errorPaths);
            if (!deleteAssets)
            {
                Debug.Log("Delete problems " + errorPaths.Count);
            }*/
            
            //save them in assets
            string fullPath = $"{directory}/{tileCollection.name}.asset";
            try
            {
                AssetDatabase.StartAssetEditing();
                AssetDatabase.CreateAsset(tileCollection, fullPath);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            
        }

        void SerializeNewTileFields(LDtkTileCollection[] allCollections)
        {
            ArrayProp.ClearArray();
            ArrayProp.arraySize = allCollections.Length;
            for (int i = 0; i < allCollections.Length; i++)
            {
                SerializedProperty objProp = ArrayProp.GetArrayElementAtIndex(i);
                SerializedProperty keyProp = objProp.FindPropertyRelative(LDtkAsset.PROP_KEY);
                SerializedProperty assetProp = objProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);

                LDtkTileCollection collection = allCollections[i];
                keyProp.stringValue = collection.name;
                assetProp.objectReferenceValue = collection;
            }
        }

        
    }
}