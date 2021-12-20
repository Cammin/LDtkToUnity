using UnityEditor.IMGUI.Controls;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkTreeViewProject : LDtkTreeView
    {
        private readonly LdtkJson _json;
        private readonly string _projectName;
        
        public LDtkTreeViewProject(TreeViewState state, LdtkJson json, string projectName) : base(state)
        {
            _json = json;
            _projectName = projectName;
        }
        
        protected override void BuildFirstRoot(TreeViewItem parent)
        {
            BuildProject(parent);
        }

        private void BuildProject(TreeViewItem root)
        {
            TreeViewItem projectItem = CreateTreeItem(true);
            projectItem.displayName = _projectName;
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