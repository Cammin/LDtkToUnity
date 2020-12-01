using System.Collections.Generic;
using System.IO;
using LDtkUnity.Editor.AssetManagement.AssetFactories;
using LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler;
using LDtkUnity.Editor.AssetManagement.Drawers;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using LDtkUnity.Runtime.UnityAssets.Settings;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement
{
    [CustomEditor(typeof(LDtkProject))]
    public class LDtkProjectEditor : UnityEditor.Editor
    {
        
        private LDtkDataProject? _projectData;

        private Vector2 _currentScroll;
        private bool _dropdown;
        
        private readonly Dictionary<ILDtkUid, LDtkReferenceDrawerTileset> _tilesets = new Dictionary<ILDtkUid, LDtkReferenceDrawerTileset>();


        private LDtkProject LDtkProject => (LDtkProject)target;
        
        private string ProjectPath => Path.GetDirectoryName(AssetDatabase.GetAssetPath(LDtkProject._jsonProject));
        
        public override void OnInspectorGUI()
        {
            ShowGUI();
            
            _dropdown = EditorGUILayout.Foldout(_dropdown, "Internal Data");
            if (_dropdown)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                base.OnInspectorGUI();
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }

        }
        
        private void DisposeGarbage()
        {
            LDtkIconLoader.Dispose();
        }



        private void ShowGUI()
        {
            DrawWelcomeMessage();

            if (!AssignJsonField()) return;

            LDtkDataProject project = _projectData.Value;

            EditorGUILayout.Space();
            DrawLevels(project.levels);
            EditorGUILayout.Space();
            DrawLayers(project.defs.layers);
            EditorGUILayout.Space();
            DrawEntities(project.defs.entities);
            EditorGUILayout.Space();
            GenerateEnumsButton(project);
            DrawEnums(project.defs.enums);
            EditorGUILayout.Space();
            DrawTilesets(project.defs.tilesets);
        }

        private void GenerateEnumsButton(LDtkDataProject project)
        {
            if (GUILayout.Button("Generate Enums"))
            {
                string assetPath = AssetDatabase.GetAssetPath(LDtkProject);
                assetPath = Path.GetDirectoryName(assetPath);
                LDtkEnumGenerator.GenerateEnumScripts(project.defs.enums, assetPath, LDtkProject.name);
            }
        }

        private bool AssignJsonField()
        {
            TextAsset currentAsset = LDtkProject._jsonProject;
            TextAsset prevAsset = currentAsset;
            LDtkProject._jsonProject = (TextAsset)EditorGUILayout.ObjectField(currentAsset, typeof(TextAsset), false);

            if (currentAsset == null)
            {
                return false;
            }
            
            if (currentAsset != prevAsset)
            {
                _projectData = null;

                if (!LDtkToolProjectLoader.IsValidJson(currentAsset.text))
                {
                    Debug.LogError("LDtk: Invalid LDtk format");
                    LDtkProject._jsonProject = null;
                    return false;
                }
            }
            
            if (_projectData == null)
            {
                _projectData = LDtkToolProjectLoader.DeserializeProject(currentAsset.text);
            }

            return true;
        }

        private void DrawProjectContent(LDtkDataProject project)
        {

        }

        private string GetWelcomeMessage()
        {
            if (LDtkProject == null)
            {
                return "Assign an LDtk Project asset or create one";
            }
            if (LDtkProject._jsonProject == null)
            {
                return "Assign a LDtk json text asset";
            }

            string details = "";

            if (_projectData != null)
            {
                LDtkDataProject data = _projectData.Value;
                details = $" v{data.jsonVersion}";
            }
            
            return $"LDtk Project{details}";

        }
        
        private void DrawWelcomeMessage()
        {
            Rect rect = EditorGUILayout.GetControlRect();
            string welcomeMessage = GetWelcomeMessage();
            EditorGUI.LabelField(rect, welcomeMessage);
        }

        #region drawing
        private void DrawLevels(LDtkDataLevel[] lvls)
        {
            foreach (LDtkDataLevel level in lvls)
            {
                new LDtkReferenceDrawerLevelIdentifier().Draw(level);
            }
        }

        private void DrawEnums(LDtkDefinitionEnum[] definitions)
        {
            foreach (LDtkDefinitionEnum enumDefinition in definitions)
            {
                new LDtkReferenceDrawerEnum().Draw(enumDefinition);
            }
        }

        private void DrawTilesets(LDtkDefinitionTileset[] definitions)
        {
            foreach (LDtkDefinitionTileset tileset in definitions)
            {
                if (!_tilesets.ContainsKey(tileset))
                {
                    string path = ProjectPath + "" + tileset.relPath;
                    LDtkTilesetAsset asset = AssetDatabase.LoadAssetAtPath<LDtkTilesetAsset>(ProjectPath + "/" + tileset.relPath);
                    LDtkReferenceDrawerTileset drawer = new LDtkReferenceDrawerTileset(tileset, asset, path);
                    _tilesets.Add(tileset, drawer);
                }
                
                _tilesets[tileset].Draw(tileset);
                _tilesets[tileset].RefreshSpritePathAssignment(tileset);
            }
        }
        
        
        
        private void DrawEntities(LDtkDefinitionEntity[] definitions)
        {
            foreach (LDtkDefinitionEntity entityData in definitions)
            {
                LDtkEntityAsset entity = LDtkProject.GetEntity(entityData.identifier);
                
                

                LDtkReferenceDrawerEntity drawerEntity = new LDtkReferenceDrawerEntity(entityData, entity);
                drawerEntity.Draw(entityData);
            }
        }

        private void DrawLayers(LDtkDefinitionLayer[] definitions)
        {
            foreach (LDtkDefinitionLayer layer in definitions)
            {

                if (layer.IsIntGridLayer)
                {
                    new LDtkReferenceDrawerIntGridLayer().Draw(layer);
                    foreach (LDtkDefinitionIntGridValue value in layer.intGridValues)
                    {
                        LDtkIntGridValueAsset intGridValue = LDtkProject.GetIntGridValue(value.identifier);
                        new LDtkReferenceDrawerIntGridValue(value, intGridValue).Draw(value);
                    }
                }
            }
        }
        #endregion

        
        
        /*private Rect DrawEntry(Texture2D icon, string entryName)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            int indent = 15;
            controlRect.xMin += indent;

            //controlRect = EditorGUI.IndentedRect(controlRect);

            Rect textureRect = new Rect(controlRect)
            {
                width = controlRect.height
            };
            GUI.DrawTexture(textureRect, icon);

            controlRect.xMin += textureRect.width;
            
            //EditorGUI.DrawRect(controlRect, Color.red);
            

            Rect labelRect = new Rect(controlRect)
            {
                width = Mathf.Max(controlRect.width/2, EditorGUIUtility.labelWidth) - EditorGUIUtility.fieldWidth
            };
            EditorGUI.LabelField(labelRect, entryName);
            
            Rect fieldRect = new Rect(controlRect)
            {
                x = labelRect.xMax,
                width = Mathf.Max(controlRect.width - labelRect.width, EditorGUIUtility.fieldWidth)
            };
            return fieldRect;
        }*/

        /*private void DrawAssignableAssetEntry<T>(Rect fieldRect) where T : Object, ILDtkAsset
        {
            //present green if okay //TODO
            //present yellow if referenced item is null
            //present red if asset does not exist
        }*/
    }
}