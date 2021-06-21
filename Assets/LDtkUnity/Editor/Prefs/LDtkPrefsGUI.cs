using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkPrefsGUI
    {
        private readonly SerializedObject _serializedObject;
        
        private readonly Action _resetAction;
        private readonly Action _saveAction;


        private static readonly GUIContent LogBuildTimes = new GUIContent
        {
            text = "Log Build Times",
            tooltip = "Toggle on to log the count of levels built, and how long it took to generate them.\n" +
                      "Triggered upon importing a LDtk Project."
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
        }
        
        public void OnGUI(string searchContext)
        {
            DrawResetButton();
            _serializedObject.Update();
            
            EditorGUIUtility.labelWidth = 200;

            using (new LDtkIndentScope())
            {

                GUIStyle style = EditorStyles.miniBoldLabel;                
                
                _serializedObject.DrawField(LDtkPrefs.PROP_LOG_BUILD_TIMES, LogBuildTimes);
                
                LDtkEditorGUIUtility.DrawDivider();
                
                EditorGUILayout.LabelField("Level Handles", style);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_IDENTIFIER, LevelIdentifier);
                if (_serializedObject.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_BORDER, LevelBorder).boolValue)
                {
                    using (new LDtkIndentScope())
                    {
                        _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_LEVEL_BORDER_THICKNESS, Thickness);
                    }
                    
                }
                
                LDtkEditorGUIUtility.DrawDivider();

                EditorGUILayout.LabelField("Entity Handles", style);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_IDENTIFIER, EntityIdentifier);
                _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_ICON, EntityIcon);
                if (_serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_SHAPE, EntityShape).boolValue)
                {
                    using (new LDtkIndentScope())
                    {
                        _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_SHAPE_ONLY_HOLLOW, OnlyHollow);
                        _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_ENTITY_SHAPE_THICKNESS, Thickness);
                    }
                }
                
                LDtkEditorGUIUtility.DrawDivider();
                
                EditorGUILayout.LabelField("Field Handles", style);
                //_serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_IDENTIFIER);
                if (_serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_RADIUS, FieldRadius).boolValue)
                {
                    using (new LDtkIndentScope())
                    { 
                        _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_RADIUS_THICKNESS, Thickness);
                    }
                }
                if (_serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_POINTS, FieldPoints).boolValue)
                {
                    using (new LDtkIndentScope())
                    {
                        _serializedObject.DrawField(LDtkPrefs.PROP_SHOW_FIELD_POINTS_THICKNESS, Thickness);
                    }
                }
                
                LDtkEditorGUIUtility.DrawDivider();
            }

            if (_serializedObject.ApplyModifiedPropertiesWithoutUndo())
            {
                _saveAction?.Invoke();
            }
            
        }
        
        private void DrawResetButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();


            Texture unityIcon = LDtkIconUtility.GetUnityIcon("Refresh", "");

            GUIContent content = new GUIContent
            {
                //text = "Reset",
                tooltip = "Reset to defaults",
                image = unityIcon
            };
                
            if (GUILayout.Button(content, GUILayout.Width(30)))
            {
                _resetAction.Invoke();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}