using System;
using System.IO;
using System.Linq;
using LDtkUnity.Editor.AssetManagement.AssetFactories;
using LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow
{
    public class LDtkAssetWindow : EditorWindow
    {
        private TextAsset _ldtkProject;
        private LDtkDataProject? _projectData;

        private Vector2 _currentScroll;
        private bool _dropdown;

        [MenuItem("Window/2D/LDtk Asset Manager")]
        public static void Init()
        {
            LDtkAssetWindow w = GetWindow<LDtkAssetWindow>("Asset Manager");
            w.titleContent.image = LDtkIconLoader.LoadSimpleIcon();
            w.minSize = new Vector2(224, 135);
            w.Show();
        }
        
        private void OnBecameInvisible()
        {
            LDtkIconLoader.Dispose();
        }

        private void OnGUI()
        {
            DrawWelcomeMessage();

            if (!AssignProjectField()) return;

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
            /*void Dropdown()
            {
                DrawDropdown(ref _dropdown, "Assets", DrawContant);
            }*/
            EditorGUI.indentLevel++;
            DrawScrollView(ref _currentScroll, DrawContant);
            EditorGUI.indentLevel--;

        }
        
        private bool AssignProjectField()
        {
            TextAsset prevAsset = _ldtkProject;
            _ldtkProject = (TextAsset) EditorGUILayout.ObjectField(_ldtkProject, typeof(TextAsset), false);

            if (_ldtkProject == null)
            {
                return false;
            }

            if (_ldtkProject != prevAsset)
            {
                _projectData = null;

                if (!LDtkToolProjectLoader.IsValidJson(_ldtkProject.text))
                {
                    Debug.LogError("LDtk: Invalid LDtk format");
                    _ldtkProject = null;
                    return false;
                }
            }

            if (_projectData == null)
            {
                _projectData = LDtkToolProjectLoader.DeserializeProject(_ldtkProject.text);
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

        private void DrawWelcomeMessage()
        {
            string welcomeMessage = _ldtkProject == null ? "Assign an LDtk project" : "LDtk Project";
            EditorGUILayout.LabelField(welcomeMessage);
        }

        #region drawing
        private void DrawLevels(LDtkDataLevel[] lvls)
        {
            foreach (LDtkDataLevel level in lvls)
            {
                DrawEntry(LDtkIconLoader.LoadWorldIcon(), level.identifier);
            }
        }

        private void DrawEnums(LDtkDefinitionEnum[] definitions)
        {
            foreach (LDtkDefinitionEnum enumDefinition in definitions)
            {
                DrawEntry(LDtkIconLoader.LoadEnumIcon(), enumDefinition.identifier);
            }
        }

        private void DrawTilesets(LDtkDefinitionTileset[] definitions)
        {
            foreach (LDtkDefinitionTileset tileset in definitions)
            {
                Rect rect = DrawEntry(LDtkIconLoader.LoadTilesetIcon(), tileset.identifier);
                DrawAssignableAssetEntry<LDtkTilesetAsset>(rect);
            }
        }
        private void DrawEntities(LDtkDefinitionEntity[] definitions)
        {
            foreach (LDtkDefinitionEntity entity in definitions)
            {
                Rect rect = DrawEntry(LDtkIconLoader.LoadEntityIcon(), entity.identifier);
                DrawAssignableAssetEntry<LDtkEntityAsset>(rect);
            }
        }

        private void DrawLayers(LDtkDefinitionLayer[] definitions)
        {
            foreach (LDtkDefinitionLayer layer in definitions)
            {
                DrawEntry(LDtkIconLoader.LoadFileIcon(), layer.identifier);
            }
        }
        #endregion

        private static void DrawScrollView(ref Vector2 scroll, Action draw)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            draw.Invoke();
            EditorGUILayout.EndScrollView();
        }
        
        private Rect DrawEntry(Texture2D icon, string entryName)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            Rect indentedRect = EditorGUI.IndentedRect(controlRect);

            Rect textureRect = new Rect(indentedRect)
            {
                width = controlRect.height
            };
            GUI.DrawTexture(textureRect, icon);

            Rect labelRect = new Rect(controlRect)
            {
                x = textureRect.x + textureRect.width,
                width = Mathf.Max(controlRect.width/2, EditorGUIUtility.labelWidth) - EditorGUIUtility.fieldWidth
            };
            EditorGUI.LabelField(labelRect, entryName);
            
            Rect fieldRect = new Rect(controlRect)
            {
                x = labelRect.xMax - textureRect.width,
                width = Mathf.Max(controlRect.width - labelRect.width, EditorGUIUtility.fieldWidth)
            };
            return fieldRect;
        }
        private void DrawAssignableAssetEntry<T>(Rect fieldRect)  where T : Object, ILDtkAsset
        {
            //present green if okay //TODO
            //present yellow if referenced item is null
            //present red if asset does not exist

            Color color;
            


            T t = null;
            
            t = (T)EditorGUI.ObjectField(fieldRect, t, typeof(T), false);
        }
    }
}
