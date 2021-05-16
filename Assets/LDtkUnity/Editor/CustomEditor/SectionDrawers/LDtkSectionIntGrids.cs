using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkSectionIntGrids : LDtkSectionDrawer<LayerDefinition>
    {
        protected override string PropertyName => LDtkProjectImporter.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "The sprites assigned to IntGrid values determine the collision shape of them in the tilemap.\nLeave any fields empty for no collision.";
        protected override Texture GuiImage => LDtkIconUtility.LoadIntGridIcon();

        //private SerializedProperty TileCollectionProperty => SerializedObject.FindProperty("LDtkProjectImporter.INTGRID_TILES");//todo fix this
        
        public LDtkSectionIntGrids(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            //iterator is for figuring out which array index we should really be using, since any layer could have any amount of intgrid values
            LDtkDrawerIntGridValueIterator intGridValueIterator = new LDtkDrawerIntGridValueIterator();
            
            foreach (LayerDefinition def in defs)
            {
                //draw intgrid
                LDtkDrawerIntGrid intGridDrawer = new LDtkDrawerIntGrid(def, ArrayProp, intGridValueIterator);
                drawers.Add(intGridDrawer);
            }
        }

        protected override int GetSizeOfArray(LayerDefinition[] datas)
        {
            return datas.SelectMany(p => p.IntGridValues).Count();
        }

        protected override void DrawDropdownContent(LayerDefinition[] datas)
        {
            IntGridValuesVisibleField();
            base.DrawDropdownContent(datas);
            
            EditorGUILayout.Space();
            //TileCollectionField(); //todo
            //GenerateTileCollectionButton(datas);
        }
        
        private void IntGridValuesVisibleField()
        {
            SerializedProperty intGridVisibilityProp = SerializedObject.FindProperty(LDtkProjectImporter.INTGRID_VISIBLE);
            //Rect rect = EditorGUILayout.GetControlRect();

            GUIContent content = new GUIContent()
            {
                text = intGridVisibilityProp.displayName,
                tooltip = "Use this if rendering the IntGrid value colors is preferred"
            };
            
            EditorGUILayout.PropertyField(intGridVisibilityProp, content);
            SerializedObject.ApplyModifiedProperties();
        }

        /*private void TileCollectionField()
        {
            Rect rect = EditorGUILayout.GetControlRect();

            if (TileCollectionProperty.objectReferenceValue == null)
            {
                LDtkEditorGUI.DrawFieldWarning(rect, "The collection is not assigned. This is needed to create collision in the levels.");
            }
            
            EditorGUI.PropertyField(rect, TileCollectionProperty);
        }*/

        

        /*protected override bool HasSectionProblem()
        {
            return TileCollectionProperty.objectReferenceValue == null;
        }*/




        /*private void GenerateTileCollectionButton(LayerDefinition[] defs)
        {
            if (Importer == null)
            {
                return;
            }
        
            GUIContent content = new GUIContent()
            {
                text = "Generate Tile Collection",
                tooltip = "Generate Tile Collection, and auto assign the above field",
                image = LDtkIconUtility.GetUnityIcon("Tilemap")
            };

            
            if (!GUILayout.Button(content))
            {
                return;
            }

            GenerateIntGridTileCollection(defs);
        }*/
        


        /*private void GenerateIntGridTileCollection(LayerDefinition[] defs)
        {
            LDtkTileCollectionFactoryParts[] partsArray = GetParts(defs);

            string assetName = Importer.JsonFile.name + "_IntGridValues";
            LDtkTileCollectionFactory factory = new LDtkTileCollectionFactory(partsArray, assetName, ContructIntGridTile);
            factory.CreateAndSaveTileCollection();

            if (factory.SaveAssetsAndPingIfSuccessful())
            {
                TileCollectionProperty.objectReferenceValue = factory.Collection;
            }
        }*/

        /*private LDtkTileCollectionFactoryParts[] GetParts(LayerDefinition[] defs)
        {
            List<LDtkTileCollectionFactoryParts> partsList = new List<LDtkTileCollectionFactoryParts>();

            foreach (LayerDefinition layerDefinition in defs)
            {
                foreach (IntGridValueDefinition valueDefinition in layerDefinition.IntGridValues)
                {
                    string key = LDtkKeyFormatUtil.IntGridValueFormat(layerDefinition, valueDefinition);
                    Sprite gridValueSprite = Importer.GetIntGridValueTile(key);
                    
                    LDtkTileCollectionFactoryParts parts = new LDtkTileCollectionFactoryParts(key, gridValueSprite, valueDefinition.UnityColor);
                    partsList.Add(parts);
                }
            }

            return partsList.ToArray();
        }

        private static Tile ContructIntGridTile(LDtkTileCollectionFactoryParts parts)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = parts.Name;
            tile.sprite = parts.Sprite != null ? parts.Sprite : LDtkResourcesLoader.LoadDefaultTileSprite();
            tile.color = parts.Color;
            tile.colliderType = GetTypeForSprite(parts.Sprite);
            return tile;
        }*/

        /*private static Tile.ColliderType GetTypeForSprite(Sprite input)
        {
            if (input == null)
            {
                return Tile.ColliderType.None;
            }
            
            if (input.GetPhysicsShapeCount() == 0)
            {
                return Tile.ColliderType.None;
            }
            
            return Tile.ColliderType.Sprite;
        }*/
    }
}