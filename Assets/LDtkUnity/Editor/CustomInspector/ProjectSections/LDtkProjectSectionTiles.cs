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

        protected override string PropertyName => LDtkProject.TILE_COLLECTIONS;
        protected override string GuiText => "Tiles";
        protected override string GuiTooltip => "Tiles";
        protected override Texture2D GuiImage => (Texture2D)EditorGUIUtility.IconContent("Tile Icon").image;

        protected override void GetDrawers(TilesetDefinition[] defs, List<LDtkContentDrawer<TilesetDefinition>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                TilesetDefinition definition = defs[i];
                SerializedProperty tileCollection = ArrayProp.GetArrayElementAtIndex(i);
                LDtkDrawerTileCollection drawer = new LDtkDrawerTileCollection(definition, tileCollection, definition.Identifier);
                drawers.Add(drawer);
            }
        }
        
        

        /*protected override int GetSizeOfArray(TilesetDefinition[] datas)
        {
            return -1;
        }*/

        /*protected override void DrawDropdownContent(TilesetDefinition[] datas)
        {
            GenerateTilesButton();



            string label = $"{ArrayProp.arraySize} Total Referenced Tiles";
            
            EditorGUILayout.LabelField(label);
            
        }*/

        
        
        /*private LDtkTileCollection[] CreateTileCollectionsFromSerializedTextures()
        {
            Texture2D[] list = GetAssetsFromSection<Texture2D>(LDtkProject.TILESETS);
            
            return list.Select().ToArray();
        }

        private void SerializeNewTileFields(LDtkTileCollection[] allCollections)
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
        }*/
    }
}