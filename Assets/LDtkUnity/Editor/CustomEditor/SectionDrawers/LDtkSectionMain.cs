using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
        private static readonly GUIContent LevelFields = new GUIContent
        {
            text = "Custom Level Prefab",
            tooltip = "Optional.\n" +
                      "If assigned, will be in place of every GameObject for levels.\n" +
                      "Use for custom scripting via the interface events to store certain values, etc."
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
        private static readonly GUIContent GeometryType = new GUIContent
        {
            text = "Geometry Type",
            tooltip = "Specifies the type of geometry the Composite Collider generates. Outlines is like an EdgeCollider2D, Polygons is like a PolygonCollider2D"
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
        private static readonly GUIContent UseParallax = new GUIContent
        {
            text = "Use Parallax",
            tooltip = "Adds components to layer GameObjects that will try to mimic the parallax motion seen in LDtk."
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

            DrawCustomLevelField();
            
            if (_data.Defs.Layers.Any(p => p.IsIntGridLayer))
            {
                DrawField(IntGridVisible, LDtkProjectImporter.INTGRID_VISIBLE);
            }
            if (_data.Defs.Layers.Any(p => p.IsIntGridLayer || p.IsAutoLayer || p.IsTilesLayer))
            {
                SerializedProperty compositeProp = DrawField(UseCompositeCollider, LDtkProjectImporter.USE_COMPOSITE_COLLIDER);
                if (compositeProp.boolValue)
                {
                    DrawField(GeometryType, LDtkProjectImporter.GEOMETRY_TYPE);
                }
            }
            
            DrawField(CreateBackgroundColor, LDtkProjectImporter.CREATE_BACKGROUND_COLOR);
            DrawField(CreateLevelBoundsTrigger, LDtkProjectImporter.CREATE_LEVEL_BOUNDS_TRIGGER);
            DrawField(UseParallax, LDtkProjectImporter.USE_PARALLAX);

            Editor.DrawDependenciesProperty();
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