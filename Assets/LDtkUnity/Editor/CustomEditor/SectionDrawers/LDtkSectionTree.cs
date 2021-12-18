using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkSectionTree : LDtkSectionDrawer
    {
        private LdtkJson _data;
        
        private readonly LDtkSectionTreeView _tree;
        private TreeViewState _state;
        
        protected override string GuiText => "Hierarchy";
        protected override string GuiTooltip => "This is the hierarchy of the LDtk json data.";
        protected override Texture GuiImage => LDtkIconUtility.GetUnityIcon("UnityEditor.SceneHierarchyWindow", "");
        protected override string ReferenceLink => null;

        public LDtkSectionTree(SerializedObject serializedObject) : base(serializedObject)
        {
            _state = new TreeViewState();
            _tree = new LDtkSectionTreeView(_state);
        }
        
        public void SetJson(LdtkJson data)
        {
            _data = data;
        }
        

        protected override void DrawDropdownContent()
        {
            
            
            Rect controlRect = EditorGUILayout.GetControlRect(false, 50);
            _tree.Reload();
            _tree.OnGUI(controlRect);
        }
    }
}