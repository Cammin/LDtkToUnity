using UnityEditor.IMGUI.Controls;

namespace LDtkUnity.Editor
{
    public abstract class LDtkTreeView : TreeView
    {
        private int _nextId = 0;
        protected int Depth = -1;

        protected LDtkTreeView(TreeViewState state) : base(state)
        {

        }

        protected TreeViewItem CreateTreeItem()
        {
            return new TreeViewItem
            {
                id = _nextId++,
                depth = Depth
            }; 
        }
        
        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = CreateTreeItem();
            Depth++;
            
            BuildFirstRoot(root);
            SetupDepthsFromParentsAndChildren(root);
            return root;

        }

        protected abstract void BuildFirstRoot(TreeViewItem parent);

        protected void BuildLevel(TreeViewItem parent, Level level)
        {
            TreeViewItem levelItem = CreateTreeItem();
            levelItem.displayName = level.Identifier;
            levelItem.icon = LDtkIconUtility.LoadLevelIcon();
            parent.AddChild(levelItem);
            
            BuildLayers(levelItem, level);
        }

        private void BuildLayers(TreeViewItem parent, Level level)
        {
            Depth++;
            foreach (LayerInstance layerInstance in level.LayerInstances)
            {
                BuildLayer(parent, layerInstance);
            }
        }

        private void BuildLayer(TreeViewItem parent, LayerInstance layerInstance)
        {
            if (layerInstance.IsDeadWeight)
            {
                return;
            }

            TreeViewItem layerItem = CreateTreeItem();
            layerItem.displayName = layerInstance.Identifier;
            layerItem.depth = 2;
            layerItem.icon = LDtkIconUtility.GetIconForLayerInstance(layerInstance);
            parent.AddChild(layerItem);
        }
    }
}