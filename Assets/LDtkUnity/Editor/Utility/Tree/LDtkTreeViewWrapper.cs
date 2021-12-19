using System;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkTreeViewWrapper : IDisposable
    {
        private const string PREFS_KEY = "LDtkUnityTreeState"; 
        
        private TreeViewState _state;
        private readonly TreeView _tree;
        
        public LDtkTreeViewWrapper(LdtkJson data)
        {
            InitState();
            _tree = new LDtkTreeViewProject(_state, data);
            InitTree();
        }
        public LDtkTreeViewWrapper(Level data)
        {
            InitState();
            _tree = new LDtkTreeViewLevel(_state, data);
            InitTree();
        }

        private void InitState()
        {
            if (EditorPrefs.HasKey(PREFS_KEY))
            {
                string json = EditorPrefs.GetString(PREFS_KEY);
                _state = JsonUtility.FromJson<TreeViewState>(json);
            }
            else
            {
                _state = new TreeViewState();
            }
        }

        private void InitTree()
        {
            _tree.Reload();
            _tree.ExpandAll();
        }

        public void Dispose()
        {
            string json = JsonUtility.ToJson(_state);
            EditorPrefs.SetString(PREFS_KEY, json);
        }
        
        public void OnGUI()
        {
            if (_tree == null)
            {
                Debug.LogError("tree null");
                return;
            }
            
            Rect controlRect = EditorGUILayout.GetControlRect(false, Mathf.Min(_tree.totalHeight));
            _tree?.OnGUI(controlRect);
        }
    }
}