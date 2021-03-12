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

            Tile[] allTilesFromAllTextures = CreateTilesFromSerializedTextures();
            SaveTilesToDatabase(allTilesFromAllTextures);
            SerializeNewTileFields(allTilesFromAllTextures);
        }

        
        
        private Tile[] CreateTilesFromSerializedTextures()
        {
            Texture2D[] list = GetAssetsFromSection<Texture2D>(LDtkProject.TILESETS);

            List<Tile> allTilesFromAllTextures = new List<Tile>();
            foreach (Texture2D texture in list)
            {
                Tile[] tiles = LDtkTileFactory.GenerateTilesForTextureMetas(texture);
                allTilesFromAllTextures.AddRange(tiles);
            }

            return allTilesFromAllTextures.ToArray();
        }
        
        private void SaveTilesToDatabase(Tile[] tiles)
        {
            if (tiles == null || tiles.Length <= 0)
            {
                Debug.Log("No tiles were given to save");
                return;
            }

            EditorUtility.DisplayProgressBar("Saving Tiles", "Solving Directory", 0);
            string directory = LDtkPathUtil.SiblingDirectoryOfAsset(Project) + "/Tiles";
            
            LDtkPathUtil.CreateDirectoryIfNotValidFolder(directory);
            
            EditorUtility.DisplayProgressBar("Saving Tiles", "Deleting old tiles", 0);
            //destroy all previous ones. despite the warning that appears, it seems to work
            
            
            
            string[] oldTiles = AssetDatabase.FindAssets("t:Tile", new[] {directory});
            string[] paths = oldTiles.Select(AssetDatabase.GUIDToAssetPath).ToArray();

            List<string> errorPaths = new List<string>();
            bool deleteAssets = AssetDatabase.DeleteAssets(paths, errorPaths);
            if (!deleteAssets)
            {
                Debug.Log("Delete problems " + errorPaths.Count);
            }
            
            //save them in assets
            try
            {
                EditorUtility.DisplayProgressBar("Saving Tiles", "", 0);
                AssetDatabase.StartAssetEditing();
                
                for (int i = 0; i < tiles.Length; i++)
                {
                    Tile tile = tiles[i];
                    string tileName = tile.name;
                    string fullPath = $"{directory}/{tileName}.asset";
                    
                    float ratio = ((float)i/tiles.Length);
                    EditorUtility.DisplayProgressBar("Saving Tiles", "Creating " + tileName, ratio*0.1f);

                    AssetDatabase.CreateAsset(tile, fullPath);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
            EditorUtility.DisplayProgressBar("Saving Tiles", "Finishing", 1);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.ClearProgressBar();
        }

        void SerializeNewTileFields(Tile[] allNewTiles)
        {
            ArrayProp.ClearArray();
            ArrayProp.arraySize = allNewTiles.Length;
            for (int i = 0; i < allNewTiles.Length; i++)
            {
                SerializedProperty objProp = ArrayProp.GetArrayElementAtIndex(i);
                SerializedProperty keyProp = objProp.FindPropertyRelative(LDtkAsset.PROP_KEY);
                SerializedProperty assetProp = objProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);

                Tile tile = allNewTiles[i];
                keyProp.stringValue = tile.name;
                assetProp.objectReferenceValue = tile;
            }
        }

        
    }
}