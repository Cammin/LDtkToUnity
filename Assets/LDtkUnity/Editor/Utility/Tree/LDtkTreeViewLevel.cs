using UnityEditor.IMGUI.Controls;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkTreeViewLevel : LDtkTreeView
    {
        private readonly Level _json;
        
        public LDtkTreeViewLevel(TreeViewState state, Level json) : base(state)
        {
            _json = json;
        }

        protected override void BuildFirstRoot(TreeViewItem parent)
        {
            BuildLevel(parent, _json);
        }
    }
}