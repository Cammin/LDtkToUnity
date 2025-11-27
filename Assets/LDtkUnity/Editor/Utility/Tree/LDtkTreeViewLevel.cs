using UnityEditor.IMGUI.Controls;

#if UNITY_6000_2_OR_NEWER
using TreeView = UnityEditor.IMGUI.Controls.TreeView<int>;
using TreeViewItem = UnityEditor.IMGUI.Controls.TreeViewItem<int>;
using TreeViewState = UnityEditor.IMGUI.Controls.TreeViewState<int>;
#endif

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