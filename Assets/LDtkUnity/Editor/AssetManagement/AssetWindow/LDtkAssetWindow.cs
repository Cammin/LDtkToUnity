using System.Collections.Generic;
using System.IO;
using LDtkUnity.Editor.AssetManagement.AssetFactories;
using LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler;
using LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.Settings;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow
{
    public class LDtkAssetWindow : EditorWindow
    {
        private LDtkProject _ldtkProject;
        private LDtkDataProject? _projectData;

        private Vector2 _currentScroll;
        private bool _dropdown;

        [MenuItem("Window/2D/LDtk Asset Manager")]
        private static void Init()
        {
            CustomInit(null);
        }

        public static void CustomInit(LDtkProject init)
        {
            LDtkAssetWindow w = GetWindow<LDtkAssetWindow>("Asset Manager");
            w.titleContent.image = LDtkIconLoader.LoadSimpleIcon();
            w.minSize = new Vector2(224, 135);
            w._ldtkProject = init;
            w.Show();
        }
        
        
        private void OnBecameInvisible()
        {
            LDtkIconLoader.Dispose();
            ClearCachedReferences();
        }

        private void ClearCachedReferences()
        {
            
        }

        private void RefreshAllReferences()
        {
            if (_ldtkProject == null)
            {
                ClearCachedReferences();
                return;
            }
            
            
        }

        private void OnGUI()
        {
            DrawWelcomeMessage();

            if (!AssignProjectField()) return;
            if (!AssignJsonField()) return;
            
            

            LDtkDataProject project = _projectData.Value;


            
            
            
            if (GUILayout.Button("Generate Enums"))
            {
                string assetPath = AssetDatabase.GetAssetPath(_ldtkProject);
                assetPath = Path.GetDirectoryName(assetPath);
                LDtkEnumGenerator.GenerateEnumScripts(project.defs.enums, assetPath, _ldtkProject.name);
            }

            void DrawContant()
            {
                DrawProjectContent(project);
            }
            LDtkDrawerUtil.ScrollView(ref _currentScroll, DrawContant);

        }




        private bool AssignProjectField()
        {
            _ldtkProject = (LDtkProject)EditorGUILayout.ObjectField(_ldtkProject, typeof(LDtkProject), false);
            return _ldtkProject != null;
        }
        private bool AssignJsonField()
        {
            TextAsset currentAsset = _ldtkProject._jsonProject;
            TextAsset prevAsset = currentAsset;
            _ldtkProject._jsonProject = (TextAsset)EditorGUILayout.ObjectField(currentAsset, typeof(TextAsset), false);

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
                    _ldtkProject = null;
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
            DrawLevels(project.levels);
            EditorGUILayout.Space();
            DrawLayers(project.defs.layers);
            EditorGUILayout.Space();
            DrawEntities(project.defs.entities);
            EditorGUILayout.Space();
            DrawEnums(project.defs.enums);
            EditorGUILayout.Space();
            DrawTilesets(project.defs.tilesets);
        }

        private string GetWelcomeMessage()
        {
            if (_ldtkProject == null)
            {
                return "Assign an LDtk Project asset or create one";
            }
            if (_ldtkProject._jsonProject == null)
            {
                return "Assign a LDtk json text asset";
            }

            return "LDtk Project";

        }
        
        private void DrawWelcomeMessage()
        {
            Rect rect = EditorGUILayout.GetControlRect();
            string welcomeMessage = GetWelcomeMessage();
            EditorGUI.LabelField(rect, welcomeMessage);

            const int buttonWidth = 50;
            Rect buttonRect = new Rect(rect)
            {
                xMin = rect.xMax - buttonWidth
            };
            if (GUI.Button(buttonRect, "Create"))
            {
                LDtkProject project = CreateInstance<LDtkProject>();
                project.name = "LDtkProject";

                if (LDtkAssetManager.SaveAsset(project, "Assets"))
                {
                    if (_ldtkProject == null)
                    {
                        _ldtkProject = project;
                    }
                }
            }
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

        private string ProjectPath => Path.GetDirectoryName(AssetDatabase.GetAssetPath(_ldtkProject._jsonProject));

        private Dictionary<ILDtkUid, LDtkReferenceDrawerTileset> _tilesets = new Dictionary<ILDtkUid, LDtkReferenceDrawerTileset>();
        
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
                _tilesets[tileset].Refresh(tileset);
            }
        }
        private void DrawEntities(LDtkDefinitionEntity[] definitions)
        {
            foreach (LDtkDefinitionEntity entity in definitions)
            {
                new LDtkReferenceDrawerEntity().Draw(entity);
            }
        }

        private void DrawLayers(LDtkDefinitionLayer[] definitions)
        {
            foreach (LDtkDefinitionLayer layer in definitions)
            {
                //new 
                //DrawEntry(LDtkIconLoader.LoadFileIcon(), layer.identifier);
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
