using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkSectionTreeView : TreeView
    {
        private LdtkJson _data;

        public void SetJson(LdtkJson data)
        {
            _data = data;
        }
        

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem();
            root.depth = -1;
            
            TreeViewItem child = new TreeViewItem();
            child.displayName = "Project";
            child.depth = 0;
            
            root.AddChild(child);
            
            


            return root;

        }

        public LDtkSectionTreeView(TreeViewState state) : base(state)
        {
        }

        public LDtkSectionTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
        }
    }
}