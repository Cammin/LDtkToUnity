using System.IO;
using System.Linq;
using LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow
{
    public class LDtkAssetWindow : EditorWindow
    {
        private TextAsset _ldtkProject;
        private LDtkDataProject? _projectData;

        private Vector2 _currentScroll;
        private bool _foldout;

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

            LDtkDefinitions defs = _projectData.Value.defs;
            
            if (GUILayout.Button("Generate Enums"))
            {
                string assetPath = AssetDatabase.GetAssetPath(_ldtkProject);
                assetPath = Path.GetDirectoryName(assetPath);
                LDtkEnumGenerator.GenerateEnumScripts(defs.enums, assetPath, _ldtkProject.name);
            }

            DrawDefinitions(defs);
        }

        private void DrawWelcomeMessage()
        {
            string welcomeMessage = _ldtkProject == null ? "Assign an LDtk project" : "LDtk Project";
            EditorGUILayout.LabelField(welcomeMessage);
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


        private void DrawDefinitions(LDtkDefinitions definitions)
        {
            _foldout = EditorGUILayout.Foldout(_foldout, "Definitions");
            if (!_foldout) return;

            _currentScroll = EditorGUILayout.BeginScrollView(_currentScroll);

            EditorGUI.indentLevel++;
            foreach (LDtkDefinitionLayer layer in definitions.layers)
            {
                DrawEntry(LDtkIconLoader.LoadFileIcon(), layer.identifier);
            }
            EditorGUILayout.Space();
            foreach (LDtkDefinitionEntity entity in definitions.entities)
            {
                DrawEntry(LDtkIconLoader.LoadEntityIcon(), entity.identifier);
            }
            EditorGUILayout.Space();
            foreach (LDtkDefinitionEnum enumDefinition in definitions.enums)
            {
                DrawEntry(LDtkIconLoader.LoadEnumIcon(), enumDefinition.identifier);
            }
            EditorGUILayout.Space();
            foreach (LDtkDefinitionTileset tileset in definitions.tilesets)
            {
                DrawEntry(LDtkIconLoader.LoadTilesetIcon(), tileset.identifier);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndScrollView();
        }
        
        public void DrawEntry(Texture2D icon, string entryName)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            Rect indentedRect = EditorGUI.IndentedRect(controlRect);

            Rect textureRect = new Rect(indentedRect)
            {
                width = controlRect.height
            };
            GUI.DrawTexture(textureRect, icon);

            Rect labelPos = new Rect(controlRect)
            {
                x = controlRect.x + controlRect.height
            };
            EditorGUI.LabelField(labelPos, entryName);
        }
        
        private void DrawLayers(LDtkDataLayer[] layers)
        {
            Rect verticalRect = EditorGUILayout.BeginVertical();
            
            
        }
        
        private Texture2D GetTextureForLayerType(LDtkDataLayer layer)
        {
            if (layer.IsIntGridLayer)
            {
                LDtkIconLoader.LoadIntGridIcon();
            }

            if (layer.IsAutoTilesLayer)
            {
                return LDtkIconLoader.LoadAutoLayerIcon();
            }
            
            if (layer.IsEntityInstancesLayer)
            {
                return LDtkIconLoader.LoadEntityIcon();
            }
            
            if (layer.IsGridTilesLayer)
            {
                return LDtkIconLoader.LoadTilesetIcon();
            }

            Debug.LogError("LDtk: Could not load an icon image");
            return null;
        }
    }
}
