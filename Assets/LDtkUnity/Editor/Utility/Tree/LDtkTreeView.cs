using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkTreeView : TreeView
    {
        private int _nextId = 0;
        protected int Depth = -1;

        private readonly List<int> _expandedIDs = new List<int>();

        protected LDtkTreeView(TreeViewState state) : base(state)
        {

        }
        
        public void ExpandSpecificIds()
        {
            foreach (int id in _expandedIDs)
            {
                SetExpanded(id, true);
            }
        }

        protected TreeViewItem CreateTreeItem(bool expanded)
        {
            if (expanded)
            {
                _expandedIDs.Add(_nextId);
            }
            
            return new TreeViewItem
            {
                id = _nextId++,
                depth = Depth
            }; 
        }
        
        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = CreateTreeItem(true);
            Depth++;
            
            BuildFirstRoot(root);
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected abstract void BuildFirstRoot(TreeViewItem parent);

        protected void BuildLevel(TreeViewItem parent, Level level)
        {
            TreeViewItem levelItem = CreateTreeItem(false);
            levelItem.displayName = level.Identifier;
            levelItem.icon = LDtkIconUtility.LoadLevelIcon();
            parent.AddChild(levelItem);
            
            BuildLayers(levelItem, level);
        }

        private void BuildLayers(TreeViewItem parent, Level level)
        {
            Depth++;

            if (level?.LayerInstances == null) //like if the layer instances were null due to being split into a separate level file
            {
                return;
            }
            
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

            TreeViewItem layerItem = CreateTreeItem(false);
            layerItem.displayName = layerInstance.Identifier;
            layerItem.icon = LDtkIconUtility.GetIconForLayerInstance(layerInstance);
            parent.AddChild(layerItem);

            BuildLayerExtras(layerItem, layerInstance);
        }

        private void BuildLayerExtras(TreeViewItem parent, LayerInstance layerInstance)
        {
            void BuildLayerContent(string displayName, Texture2D icon)
            {
                TreeViewItem item = CreateTreeItem(false);
                item.displayName = displayName;
                item.icon = icon;
                parent.AddChild(item);
            }

            if (layerInstance.IsIntGridLayer)
            {
                Dictionary<long, int> valueCounts = new Dictionary<long, int>();

                foreach (long l in layerInstance.IntGridCsv)
                {
                    if (l == 0)
                    {
                        continue;
                    }
                    
                    if (!valueCounts.ContainsKey(l))
                    {
                        valueCounts.Add(l, 0);
                    }
                    valueCounts[l]++;
                }

                string totalCount = $"{valueCounts.Values.Sum()} Total Values";
                BuildLayerContent(totalCount, LDtkIconUtility.LoadIntGridIcon());
                
                
                foreach (KeyValuePair<long,int> pair in valueCounts.OrderBy(p => p.Key))
                {
                    string count = $"{pair.Key}: {pair.Value} Values";
                    BuildLayerContent(count, LDtkIconUtility.LoadIntGridIcon());
                }
            }
            
            if (layerInstance.IsEntitiesLayer)
            {
                foreach (EntityInstance entityInstance in layerInstance.EntityInstances)
                {
                    BuildLayerContent(entityInstance.Identifier, LDtkIconUtility.LoadEntityIcon());
                }
            }
            
            if (layerInstance.IsTilesLayer)
            {
                string count = $"{layerInstance.GridTiles.Length} Tiles";
                BuildLayerContent(count, LDtkIconUtility.LoadTilesetIcon());
            }
            
            if (layerInstance.IsAutoLayer)
            {
                string count = $"{layerInstance.AutoLayerTiles.Length} Auto Tiles";
                BuildLayerContent(count, LDtkIconUtility.LoadAutoLayerIcon());
            }
        }
    }
}