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
            return 1;//ArrayProp.arraySize;
        }

        protected override void DrawDropdownContent(TilesetDefinition[] datas)
        {
            GenerateTilesButton();
                
            for (int i = 0; i < ArrayProp.arraySize; i++)
            {
                
            }
            
        }

        private void GenerateTilesButton()
        {
            GUIContent buttonContent = new GUIContent()
            {
                text = "Generate Tiles",
                tooltip = "For each texture's sprites, generate a tile and save it as a reference in this project. Any old tiles are deleted in this process",
                image = GuiImage
            };
            
            if (!GUILayout.Button(buttonContent))
            {
                return;
            }
            
            Debug.Log("Generate Tiles");

            Tile[] allTilesFromAllTextures = CreateTilesFromSerializedTextures();
            SerializeNewTileFields(allTilesFromAllTextures);
            SaveTilesToDatabase(allTilesFromAllTextures);
        }

        private Tile[] CreateTilesFromSerializedTextures()
        {
            List<Tile> allTilesFromAllTextures = new List<Tile>();
            SerializedProperty textureArrayProp = SerializedObject.FindProperty(LDtkProject.TILESETS);
            for (int i = 0; i < textureArrayProp.arraySize; i++)
            {
                SerializedProperty objProp = textureArrayProp.GetArrayElementAtIndex(i);
                SerializedProperty keyProp = objProp.FindPropertyRelative(LDtkAsset.PROP_KEY);
                SerializedProperty assetProp = objProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);
                Texture2D texture = (Texture2D) assetProp.objectReferenceValue;

                if (texture == null)
                {
                    Debug.LogWarning($"A texture was null in {keyProp.stringValue}");
                    continue;
                }

                Tile[] tiles = LDtkTileFactory.GenerateTilesForTextureMetas(texture);
                allTilesFromAllTextures.AddRange(tiles);
            }

            return allTilesFromAllTextures.ToArray();
        }

        void SerializeNewTileFields(Tile[] allNewTiles)
        {
            ArrayProp.ClearArray();
            ArrayProp.arraySize = allNewTiles.Length;
            for (int i = 0; i < ArrayProp.arraySize; i++)
            {
                SerializedProperty objProp = ArrayProp.GetArrayElementAtIndex(i);
                SerializedProperty keyProp = objProp.FindPropertyRelative(LDtkAsset.PROP_KEY);
                SerializedProperty assetProp = objProp.FindPropertyRelative(LDtkAsset.PROP_ASSET);

                Tile tile = allNewTiles[i];
                keyProp.stringValue = tile.name;
                assetProp.objectReferenceValue = tile;
            }
        }

        private void SaveTilesToDatabase(Tile[] tiles)
        {
            string directory = LDtkPathUtil.SiblingDirectoryOfAsset(Project) + "/Tiles";
            
            LDtkPathUtil.CreateDirectoryIfNotValidFolder(directory);
            
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
                AssetDatabase.StartAssetEditing();
                foreach (Tile tile in tiles)
                {
                    string fullPath = $"{directory}/{tile.name}.asset";
                    AssetDatabase.CreateAsset(tile, fullPath);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}