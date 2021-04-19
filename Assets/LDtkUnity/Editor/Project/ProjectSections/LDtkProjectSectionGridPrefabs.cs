using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionGridPrefabs : LDtkProjectSectionDrawer<LayerDefinition>
    {
        private readonly GameObject _defaultResourcesPrefab;

        protected override string PropertyName => LDtkProjectImporter.TILEMAP_PREFABS;
        protected override string GuiText => "Grid Prefabs";

        protected override string GuiTooltip => 
            "The Grid prefabs used when instantiating the level layers.\n" +
            "This section is completely optional, but you can create custom grid prefabs if you wish to customize them.\n" +
            "If a field is not assigned, then the default will be used.\n" +
            "Check the rules for how a custom tilemap prefab is created in the documentation, or use the default grid prefab as a guide.";

        protected override Texture GuiImage => LDtkIconLoader.GetUnityIcon("Grid");

        public LDtkProjectSectionGridPrefabs(SerializedObject serializedObject) : base(serializedObject)
        {
            _defaultResourcesPrefab = LDtkResourcesLoader.LoadDefaultGridPrefab();
        }
        
        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                LayerDefinition definition = defs[i];
                SerializedProperty arrayElement = ArrayProp.GetArrayElementAtIndex(i);
                LDtkDrawerGridPrefab drawer = new LDtkDrawerGridPrefab(definition, arrayElement, definition.Identifier);
                drawers.Add(drawer);
            }
        }
        
        protected override void DrawDropdownContent(LayerDefinition[] datas)
        {
            DefaultAssetField();

            base.DrawDropdownContent(datas);
        }

        private void DefaultAssetField()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Default Grid", _defaultResourcesPrefab, typeof(Grid), false);
            GUI.enabled = true;
        }
    }
}