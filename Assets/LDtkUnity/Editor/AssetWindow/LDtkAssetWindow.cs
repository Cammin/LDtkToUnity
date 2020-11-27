using System.IO;
using System.Linq;
using LDtkUnity.Editor.AssetLoading;
using LDtkUnity.Editor.AssetWindow.EnumHandler;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetWindow
{
    
    public class LDtkAssetWindow : EditorWindow
    {
        private TextAsset _ldtkProject;
        private LDtkDataProject? _projectData;


        [MenuItem("Window/2D/LDtk Asset Generator")]
        public static void Init()
        {
            LDtkAssetWindow w = GetWindow<LDtkAssetWindow>("LDtk Asset Generator");
            w.titleContent.image = LDtkIconLoader.LoadProjectIcon();
            w.Show();
        }
        
        private void OnGUI()
        {
            string welcomeMessage = _ldtkProject == null ? "Assign an LDtk project to begin" : "LDtk Project";
            EditorGUILayout.LabelField(welcomeMessage);

            TextAsset prevAsset = _ldtkProject;
            _ldtkProject = (TextAsset)EditorGUILayout.ObjectField(_ldtkProject, typeof(TextAsset), false);
            
            if (_ldtkProject == null)
            {
                return;
            }

            if (_ldtkProject != prevAsset)
            {
                _projectData = null;
            }

            if (!LDtkToolProjectLoader.IsValidJson(_ldtkProject.text))
            {
                Debug.LogError("LDtk: Invalid LDtk format");
                _ldtkProject = null;
                return;
            }
            
            if (_projectData == null)
            {
                _projectData = LDtkToolProjectLoader.DeserializeProject(_ldtkProject.text);
            }
            
            LDtkDefinitions defs = _projectData.Value.defs;

            EditorGUILayout.Space();

            
            
            foreach (LDtkDefinitionLayer layer in defs.layers)
            {
                EditorGUILayout.LabelField(layer.identifier);
            }


            string assetPath = AssetDatabase.GetAssetPath(_ldtkProject);
            assetPath = Path.GetDirectoryName(assetPath);
            
            if (GUILayout.Button("Generate Enums"))
            {
                GenerateEnumScripts(defs.enums, assetPath);
            }

        }

        private void GenerateEnumScripts(LDtkDefinitionEnum[] enums, string relativeFolderPath)
        {
            relativeFolderPath += "\\Enums\\";
                
            Debug.Log($"LDtk: Generating enum scripts at path: {relativeFolderPath}");
            
            foreach (LDtkDefinitionEnum enumDefinition in enums)
            {
                GenerateEnumScript(enumDefinition, relativeFolderPath);
            }
        }

        private void GenerateEnumScript(LDtkDefinitionEnum definition, string folderPath)
        {
            string[] values = definition.values.Select(value => value.id).ToArray();
            LDtkEnumFileMaker.CreateEnumFile(folderPath, definition.identifier, values, _ldtkProject.name);
        }
        
        
        
        
        void SaveAsset<T>(T asset) where T : ScriptableObject, ILDtkAsset
        {
            string meshRootDirectory = $"Assets/Resources/LDtkProject";
            if (Directory.Exists(meshRootDirectory) == false)
            {
                Directory.CreateDirectory(meshRootDirectory);
            }

            string meshFilePath = $"{meshRootDirectory}/{asset.Identifier}.asset";
            
            AssetDatabase.CreateAsset(asset, meshFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


    }
}
