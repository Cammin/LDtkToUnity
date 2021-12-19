using UnityEditor.IMGUI.Controls;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkTreeViewLevel : LDtkTreeView
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