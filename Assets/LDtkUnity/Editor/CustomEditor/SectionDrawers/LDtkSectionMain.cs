using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSectionMain : LDtkSectionDrawer
    {
        private LdtkJson _data;
        
        private static readonly GUIContent PixelsPerUnit = new GUIContent
        {
            text = "Pixels Per Unit",
            tooltip = "Tile assets pixels per unit.\n" +
                      "This number dictates what all of the instantiated tilesets/entities will change their scale to, measured in pixels per unity unit."
        };
        private static readonly GUIContent Atlas = new GUIContent
        {
            text = "Sprite Atlas",
            tooltip = "Create your own Sprite Atlas and assign it here if desired.\n" +
                      "This solves the \"tearing\" in the sprites of the tilemaps.\n" +
                      "The sprite atlas is reserved for auto-generated sprites only. Any foreign sprites assigned to the atlas will be removed."
        };
        private static readonly GUIContent LevelFields = new GUIContent
        {
            text = "Custom Level Prefab",
            tooltip = "Optional.\n" +
                      "If assigned, will be in place of every GameObject for levels.\n" +
                      "Use for custom scripting via the interface events to store certain values, etc."
        };
        private static readonly GUIContent DeparentInRuntime = new GUIContent
        {
            text = "De-parent in Runtime",
            tooltip = "When on, adds components to the project, levels, and entity-layer GameObjects that act to de-parent all of their children in runtime.\n" +
                      "This results in increased runtime performance.\n" +
                      "Keep this on if the exact level/layer hierarchy structure is not a concern in runtime."
        };
        private static readonly GUIContent IntGridVisible = new GUIContent()
        {
            text = "Render IntGrid Values",
            tooltip = "Use this if rendering the IntGrid value colors is preferred."
        };
        private static readonly GUIContent UseCompositeCollider = new GUIContent
        {
            text = "Use Composite Collider",
            tooltip = "Use this to add a CompositeCollider2D to all IntGrid tilemaps."
        };
        private static readonly GUIContent CreateBackgroundColor = new GUIContent
        {
            text = "Create Background Color",
            tooltip = "Creates a flat background for each level, based on the level's background color."
        };
        private static readonly GUIContent CreateLevelBoundsTrigger = new GUIContent
        {
            text = "Create Level Trigger",
            tooltip = "Creates a PolygonCollider2D trigger that spans the level's area for each level. Useful in conjunction with Cinemachine for example."
        };
        
        protected override string GuiText => "Main";
        protected override string GuiTooltip => "This is the importer menu.\n" +
                                                "Configure all of your custom settings here.";
        protected override Texture GuiImage => LDtkIconUtility.LoadFavIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_MAIN;

        protected override bool SupportsMultipleSelection => true;
        
        private readonly GUIContent _buttonContent;

        
        public LDtkSectionMain(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
        {
            _buttonContent = new GUIContent()
            {
                text = "+",
                image = LDtkIconUtility.GetUnityIcon("SpriteAtlas"),
                tooltip = "Creates and automatically assigns a new SpriteAtlas asset."
            };
        }

        public void SetJson(LdtkJson data)
        {
            _data = data;
        }
        

        protected override void DrawDropdownContent()
        {
            if (_data == null)
            {
                return;
            }

            PixelsPerUnitField();

            //draw the sprite atlas only if we have tiles to pack essentially
            if (LDtkProjectImporterAtlasPacker.UsesSpriteAtlas(_data))
            {
                SpriteAtlas atlas = DrawAtlasFieldAndButton();
                if (atlas != null)
                {
                    SerializedProperty prop = SerializedObject.FindProperty(LDtkProjectImporter.ATLAS);
                    prop.objectReferenceValue = atlas;
                }
            }

            DrawCustomLevelField();

            DrawField(DeparentInRuntime, LDtkProjectImporter.DEPARENT_IN_RUNTIME);

            if (!_data.Defs.IntGridLayers.IsNullOrEmpty())
            {
                DrawField(IntGridVisible, LDtkProjectImporter.INTGRID_VISIBLE);
                DrawField(UseCompositeCollider, LDtkProjectImporter.USE_COMPOSITE_COLLIDER);
            }
            
            DrawField(CreateBackgroundColor, LDtkProjectImporter.CREATE_BACKGROUND_COLOR);
            DrawField(CreateLevelBoundsTrigger, LDtkProjectImporter.CREATE_LEVEL_BOUNDS_TRIGGER);

            Editor.DrawDependenciesProperty();
        }

        private SpriteAtlas DrawAtlasFieldAndButton()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawField(Atlas, LDtkProjectImporter.ATLAS);
                
                if (GUILayout.Button(_buttonContent, EditorStyles.miniButton, GUILayout.Width(45)))
                {
                    return LDtkAssetCreator.CreateAsset($"{ProjectImporter.AssetName}_Atlas.spriteatlas", () => new SpriteAtlas(), false);
                }
            }

            return null;
        }

        private void PixelsPerUnitField()
        {
            GUIContent content = new GUIContent(PixelsPerUnit)
            {
                tooltip = PixelsPerUnit.tooltip + $"\n\nThe default grid size in this LDtk project is {_data.DefaultGridSize}."
            };
            
            SerializedProperty ppuProp = DrawField(content, LDtkProjectImporter.PIXELS_PER_UNIT);
            
            //if manually reduced, never allow to go below 1
            if (ppuProp.intValue <= 0)
            {
                ppuProp.intValue = 1;
            }
        }

        private void DrawCustomLevelField()
        {
            GUIContent levelContent = new GUIContent(LevelFields);
            
            if (!_data.Defs.LevelFields.IsNullOrEmpty())
            {
                IEnumerable<string> levelFields = _data.Defs.LevelFields.Select(field => field.Identifier);
                levelContent.tooltip = LevelFields.tooltip + $"\n\nFields:\n{string.Join(", ", levelFields)}";
            }

            SerializedProperty levelPrefabProp = DrawField(levelContent, LDtkProjectImporter.CUSTOM_LEVEL_PREFAB);
            LDtkEditorGUIUtility.DenyPotentialResursiveGameObjects(levelPrefabProp);
        }

        private SerializedProperty DrawField(GUIContent content, string propName)
        {
            SerializedProperty prop = SerializedObject.FindProperty(propName);
            EditorGUILayout.PropertyField(prop, content);
            return prop;
        }
    }
}