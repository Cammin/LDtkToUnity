using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionIntGrids : LDtkProjectSectionDrawer<LayerDefinition>
    {
        protected override string PropertyName => LDtkProject.INTGRID;
        protected override string GuiText => "IntGrids";
        protected override string GuiTooltip => "The sprites assigned to IntGrid values determine the collision shape of them in the tilemap. Leave any fields empty for no collision.";
        protected override Texture GuiImage => LDtkIconLoader.LoadIntGridIcon();

        private SerializedProperty TileCollectionProperty => SerializedObject.FindProperty(LDtkProject.INTGRID_TILES);
        
        public LDtkProjectSectionIntGrids(SerializedObject serializedObject) : base(serializedObject)
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
            TileCollectionField();
            GenerateTileCollectionButton();
        }
        
        private void IntGridValuesVisibleField()
        {
            SerializedProperty intGridVisibilityProp = SerializedObject.FindProperty(LDtkProject.INTGRID_VISIBLE);
            //Rect rect = EditorGUILayout.GetControlRect();

            GUIContent content = new GUIContent()
            {
                text = intGridVisibilityProp.displayName,
                tooltip = "Use this if rendering the IntGrid value colors is preferred"
            };
            
            EditorGUILayout.PropertyField(intGridVisibilityProp, content);
            SerializedObject.ApplyModifiedProperties();
        }

        private void TileCollectionField()
        {
            Rect rect = EditorGUILayout.GetControlRect();

            if (TileCollectionProperty.objectReferenceValue == null)
            {
                //this is more boilerplate. make common functionality out of this field, and any others from the root of the project drawings.
                float labelWidth = LDtkDrawerUtil.LabelWidth(rect.width);
                Vector2 pos = new Vector2(rect.xMin + labelWidth, rect.yMin + rect.height / 2);

                const string tooltip = "The collection is not assigned. This is needed to create collision in the levels.";
                LDtkDrawerUtil.DrawWarning(pos, tooltip, TextAnchor.MiddleRight);
            }
            
            EditorGUI.PropertyField(rect, TileCollectionProperty);
        }

        protected override bool HasSectionProblem()
        {
            return TileCollectionProperty.objectReferenceValue == null;
        }


        private void GenerateTileCollectionButton()
        {
            GUIContent content = new GUIContent()
            {
                text = "Generate Tile Collection",
                tooltip = "Generate Tile Collection, and auto assign the above field",
                image = LDtkIconLoader.GetUnityIcon("Tilemap")
            };

            
            if (!GUILayout.Button(content))
            {
                return;
            }

            Sprite[] sprites = GetSprites();

            string assetName = Project.ProjectJson.name + "_CollisionTiles";
            LDtkTileCollection tileCollection = LDtkTileCollectionFactory.CreateAndSaveTileCollection(sprites, assetName, ContructIntGridTile);

            if (tileCollection != null)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorGUIUtility.PingObject(tileCollection);
                
                TileCollectionProperty.objectReferenceValue = tileCollection;
            }
        }

        private Sprite[] GetSprites()
        {
            Sprite[] sprites =
                Drawers.Cast<LDtkDrawerIntGrid>().Where(p => p != null)
                    .SelectMany(p => p.IntGridValueDrawers.Where(drawer => drawer != null).Select(intGridValue => intGridValue.Asset)).Where(p => p != null).Distinct().ToArray();
            return sprites;
        }

        private static Tile ContructIntGridTile(Sprite sprite)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.colliderType = sprite.GetPhysicsShapeCount() == 0 ? Tile.ColliderType.None : Tile.ColliderType.Sprite;
            tile.sprite = sprite;
            tile.name = sprite.name;
            return tile;
        }
    }
}