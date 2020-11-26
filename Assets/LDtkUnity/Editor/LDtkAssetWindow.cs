using System;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Tools;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    
    public class LDtkAssetWindow : EditorWindow
    {
        private TextAsset _ldtkProject;
        private LDtkDataProject _projectData;

        private bool _isAssignedPreviously = false;
        
        
        
        [MenuItem("Window/LDtk Asset Generator")]
        public static void Init()
        {
            LDtkAssetWindow w = GetWindow<LDtkAssetWindow>("LDtk Auto-Generator");
            w.Show();
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("LDtk Project");
            _ldtkProject = (TextAsset)EditorGUILayout.ObjectField(_ldtkProject, typeof(TextAsset), false);

            if (!_ldtkProject)
            {
                if (!_isAssignedPreviously) return;
                
                _isAssignedPreviously = false;
                OnProjectUnassigned();
                return;
            }

            if (!_isAssignedPreviously)
            {
                _isAssignedPreviously = true;
                OnProjectAssigned();
            }

            EditorGUILayout.LabelField(_ldtkProject.name);
            
            LDtkDefinitions defs = _projectData.defs;

            EditorGUILayout.Space();
            
            foreach (LDtkDefinitionLayer layer in defs.layers)
            {
                EditorGUILayout.LabelField(layer.identifier);
            }
            
            
        }

        
        private void OnProjectAssigned()
        {
            _projectData = LDtkToolProjectLoader.DeserializeProject(_ldtkProject.text);
        }
        private void OnProjectUnassigned()
        {
            _projectData = default;
        }
    }
}
