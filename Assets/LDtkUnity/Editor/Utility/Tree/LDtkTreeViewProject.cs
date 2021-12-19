using UnityEditor.IMGUI.Controls;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkTreeViewProject : LDtkTreeView
    {
        private readonly LdtkJson _json;
        
        public LDtkTreeViewProject(TreeViewState state, LdtkJson json) : base(state)
        {
            _json = json;
        }
        
        protected override void BuildFirstRoot(TreeViewItem parent)
        {
            BuildProject(parent);
        }

        private void BuildProject(TreeViewItem root)
        {
            TreeViewItem projectItem = CreateTreeItem();
            projectItem.displayName = "Project";
            projectItem.depth = 0;
            projectItem.icon = LDtkIconUtility.LoadWorldIcon();
            root.AddChild(projectItem);
            
            BuildLevels(projectItem);
        }

        private void BuildLevels(TreeViewItem projectItem)
        {
            Depth++;
            foreach (Level level in _json.Levels)
            {
                BuildLevel(projectItem, level);
            }
        }
    }
}