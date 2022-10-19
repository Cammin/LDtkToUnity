using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkPrefsGUI
    {
        private readonly SerializedObject _serializedObject;
        
        private readonly Action _resetAction;
        private readonly Action _saveAction;
        
        private readonly GUIStyle _style;
        
        private readonly GUIContent _resetButtonContent;
        private readonly GUIContent _enableAllButtonContent;
        private readonly GUIContent _disableAllButtonContent;
        
        private readonly SerializedProperty _propWriteProfiledImports;
        private readonly SerializedProperty _propVerboseLogging;
        private readonly SerializedProperty _propDrawDistance;
        private readonly SerializedProperty _propShowLevelIdentifier;
        private readonly SerializedProperty _propShowLevelBorder;
        private readonly SerializedProperty _propLevelBorderThickness;
        private readonly SerializedProperty _propShowEntityIdentifier;
        private readonly SerializedProperty _propShowEntityIcon;
        private readonly SerializedProperty _propShowEntityShape;
        private readonly SerializedProperty _propEntityShapeOnlyHollow;
        private readonly SerializedProperty _propEntityShapeOnlyBorders;
        private readonly SerializedProperty _propEntityShapeThickness;
        private readonly SerializedProperty _propShowFieldRadius;
        private readonly SerializedProperty _propFieldRadiusThickness;
        private readonly SerializedProperty _propShowFieldPoints;
        private readonly SerializedProperty _propFieldPointsThickness;
        private readonly SerializedProperty _propShowEntityRef;
        private readonly SerializedProperty _propEntityRefThickness;
        
        private static readonly GUIContent WriteProfiledImports = new GUIContent
        {
            text = "Write Profiled Imports",
            tooltip = "Upon importing any LDtk level or project, unity will write a .raw file to a \"Profiler\" folder in this root unity project.\n" +
                      "These files can be opened from the profiler window to view the performance of an import.\n" +
                      "\n" +
                      "Only toggle on for analysis purposes. This has a performance overhead for every import and the files can also use a lot of storage, especially if deep profiling is enabled."
        };
        private static readonly GUIContent VerboseLogging = new GUIContent
        {
            text = "Verbose Logging",
            tooltip = "Log various info related to parts of the import process.",
        };
        private static readonly GUIContent DrawDistance = new GUIContent
        {
            text = "Draw Distance",
            tooltip = "Fade Scene Handles.\n" +
                      "Fade out and stop rendering gizmos that are zoomed out on screen."
        };
        private static readonly GUIContent LevelIdentifier = new GUIContent
        {
            text = "Show Identifier",
            tooltip = "Display the level identifiers at the bottom left of a level in the scene view.\n" +
                      "Clicking the text selects the level object"
        };
        private static readonly GUIContent LevelBorder = new GUIContent
        {
            text = "Show Border",
            tooltip = "Display level borders in the scene"
        };
        private static readonly GUIContent EntityIdentifier = new GUIContent
        {
            text = "Show Identifier",
            tooltip = "Display the entity identifiers at the the entity objects that enable this visibility in LDtk.\n" +
                      "Clicking the text selects the entity object"
        };
        private static readonly GUIContent EntityIcon = new GUIContent
        {
            text = "Show Icon",
            tooltip = "Display the entity icons at the the entity objects that enable this visibility in LDtk.\n" +
                      "Clicking the image selects the entity object"
        };
        private static readonly GUIContent EntityShape = new GUIContent
        {
            text = "Show Shape",
            tooltip = "Display entity shapes in the scene view (Rectangle, Ellipse, Cross)"
        };
        private static readonly GUIContent OnlyHollow = new GUIContent
        {
            text = "Only Hollow",
            tooltip = "Only show shapes that are set as hollow in LDtk"
        };
        private static readonly GUIContent OnlyBorders = new GUIContent
        {
            text = "Only Borders",
            tooltip = "Don't show the fill of the entity, only the border"
        };
        private static readonly GUIContent FieldRadius = new GUIContent
        {
            text = "Show Radius",
            tooltip = "Display entity float/int fields (that were set to display a radius) in the scene view"
        };

        private static readonly GUIContent FieldPoints = new GUIContent
        {
            text = "Show Points",
            tooltip = "Display entity point fields in the scene view"
        };
        
        private static readonly GUIContent EntityRef = new GUIContent
        {
            text = "Show Entity References",
            tooltip = "Display entity references in the scene view"
        };
        
        private static readonly GUIContent Thickness = new GUIContent
        {
            text = "Thickness",
            tooltip = "Affects the thickness of the lines drawn in the scene view"
        };

        public LDtkPrefsGUI(SerializedObject obj, Action resetAction, Action saveAction)
        {
            _serializedObject = obj;
            _resetAction = resetAction;
            _saveAction = saveAction;
            
            _style = EditorStyles.miniBoldLabel;
            
            _propWriteProfiledImports = obj.FindProperty(LDtkPrefs.WRITE_PROFILED_IMPORTS);
            _propVerboseLogging = obj.FindProperty(LDtkPrefs.VERBOSE_LOGGING);
            _propDrawDistance = obj.FindProperty(LDtkPrefs.DRAW_DISTANCE);
            _propShowLevelIdentifier = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_LEVEL_IDENTIFIER);
            _propShowLevelBorder = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_LEVEL_BORDER);
            _propLevelBorderThickness = obj.FindProperty(LDtkPrefs.PROPERTY_LEVEL_BORDER_THICKNESS);
            _propShowEntityIdentifier = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_ENTITY_IDENTIFIER);
            _propShowEntityIcon = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_ENTITY_ICON);
            _propShowEntityShape = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_ENTITY_SHAPE);
            _propEntityShapeOnlyHollow = obj.FindProperty(LDtkPrefs.PROPERTY_ENTITY_SHAPE_ONLY_HOLLOW);
            _propEntityShapeOnlyBorders = obj.FindProperty(LDtkPrefs.PROPERTY_ENTITY_SHAPE_ONLY_BORDERS);
            _propEntityShapeThickness = obj.FindProperty(LDtkPrefs.PROPERTY_ENTITY_SHAPE_THICKNESS);
            _propShowFieldRadius = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_FIELD_RADIUS);
            _propFieldRadiusThickness = obj.FindProperty(LDtkPrefs.PROPERTY_FIELD_RADIUS_THICKNESS);
            _propShowFieldPoints = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_FIELD_POINTS);
            _propFieldPointsThickness = obj.FindProperty(LDtkPrefs.PROPERTY_FIELD_POINTS_THICKNESS);
            _propShowEntityRef = obj.FindProperty(LDtkPrefs.PROPERTY_SHOW_ENTITY_REF);
            _propEntityRefThickness = obj.FindProperty(LDtkPrefs.PROPERTY_ENTITY_REF_THICKNESS);
            
            _resetButtonContent = new GUIContent
            {
                tooltip = "Reset to defaults",
                image = LDtkIconUtility.GetUnityIcon("Refresh", "")
            };
            
            _enableAllButtonContent = new GUIContent
            {
                tooltip = "Enable all",
                image = LDtkIconUtility.GetUnityIcon("scenevis_visible_hover", "")
            };
            
            _disableAllButtonContent = new GUIContent
            {
                tooltip = "Disable all",
                image = LDtkIconUtility.GetUnityIcon("scenevis_hidden_hover", "")
            };
        }
        
        public void OnGUI(string searchContext)
        {
            _serializedObject.Update();
            LDtkSettingsSwitchGUI.DrawSwitchSettingsButton();
            DrawButtons();
            
            EditorGUIUtility.labelWidth = 200;

            using (new EditorGUI.IndentLevelScope())
            {
                LDtkEditorGUIUtility.DrawDivider();

                DrawMiscSection();
                
                LDtkEditorGUIUtility.DrawDivider();
                
                DrawLevelSection();

                LDtkEditorGUIUtility.DrawDivider();

                DrawEntitySection();

                LDtkEditorGUIUtility.DrawDivider();
                
                DrawFieldSection();

                LDtkEditorGUIUtility.DrawDivider();
            }

            if (_serializedObject.ApplyModifiedPropertiesWithoutUndo())
            {
                _saveAction?.Invoke();
            }
        }

        private void DrawMiscSection()
        {
            EditorGUILayout.PropertyField(_propWriteProfiledImports, WriteProfiledImports);
            EditorGUILayout.PropertyField(_propVerboseLogging, VerboseLogging);
            EditorGUILayout.PropertyField(_propDrawDistance, DrawDistance);
        }

        private void DrawLevelSection()
        {
            EditorGUILayout.LabelField("Level Handles", _style);
            EditorGUILayout.PropertyField(_propShowLevelIdentifier, LevelIdentifier);
            EditorGUILayout.PropertyField(_propShowLevelBorder, LevelBorder);
            if (_propShowLevelBorder.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(_propLevelBorderThickness, Thickness);
                }
            }
        }

        private void DrawEntitySection()
        {
            EditorGUILayout.LabelField("Entity Handles", _style);
            EditorGUILayout.PropertyField(_propShowEntityIdentifier, EntityIdentifier);
            EditorGUILayout.PropertyField(_propShowEntityIcon, EntityIcon);
            
            EditorGUILayout.PropertyField(_propShowEntityShape, EntityShape);
            if (_propShowEntityShape.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(_propEntityShapeOnlyHollow, OnlyHollow);
                    EditorGUILayout.PropertyField(_propEntityShapeOnlyBorders, OnlyBorders);
                    EditorGUILayout.PropertyField(_propEntityShapeThickness, Thickness);
                }
            }
            
        }

        private void DrawFieldSection()
        {
            EditorGUILayout.LabelField("Entity Field Handles", _style);

            EditorGUILayout.PropertyField(_propShowEntityRef, EntityRef);
            if (_propShowEntityRef.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(_propEntityRefThickness, Thickness);
                }
            }
            
            EditorGUILayout.PropertyField(_propShowFieldRadius, FieldRadius);
            if (_propShowFieldRadius.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(_propFieldRadiusThickness, Thickness);
                }
            }

            EditorGUILayout.PropertyField(_propShowFieldPoints, FieldPoints);
            if (_propShowFieldPoints.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(_propFieldPointsThickness, Thickness);
                }
            }
            
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(_enableAllButtonContent, GUILayout.Width(30)))
            {
                SetEnables(true);
            }
            
            if (GUILayout.Button(_disableAllButtonContent, GUILayout.Width(30)))
            {
                SetEnables(false);
            }
            
            if (GUILayout.Button(_resetButtonContent, GUILayout.Width(30)))
            {
                _resetAction.Invoke();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void SetEnables(bool enables)
        {
            _propShowEntityIcon.boolValue = enables;
            _propShowEntityIdentifier.boolValue = enables;
            _propShowEntityRef.boolValue = enables;
            _propShowEntityShape.boolValue = enables;

            _propEntityShapeOnlyHollow.boolValue = !enables;
            _propEntityShapeOnlyBorders.boolValue = !enables;
            
            _propShowFieldPoints.boolValue = enables;
            _propShowFieldRadius.boolValue = enables;
            
            _propShowLevelBorder.boolValue = enables;
            _propShowLevelIdentifier.boolValue = enables;
        }
    }
}