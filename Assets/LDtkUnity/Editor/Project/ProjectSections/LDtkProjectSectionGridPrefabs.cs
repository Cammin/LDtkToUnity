using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionGridPrefabs : LDtkProjectSectionDrawer<LayerDefinition>
    {
        private Grid _defaultPrefab;
        
        protected override string PropertyName => LDtkProject.TILEMAP_PREFABS;
        protected override string GuiText => "Grid Prefabs";

        protected override string GuiTooltip => 
            "The Grid prefabs used when instantiating the level layers. " +
            "This section is completely optional, but you can create custom grid prefabs if you wish to customize them. " +
            "If no fields are assigned, then the stock default grid prefab will be used. " +
            "If the default override field is assigned, then it will be the new default instead of the stock one. " +
            "For more customization, assign grid prefabs for specific layers. " +
            "Check the rules for how a custom tilemap prefab is created in the documentation.";

        protected override Texture GuiImage => LDtkIconLoader.GetUnityIcon("Grid");
        
        public LDtkProjectSectionGridPrefabs(SerializedObject serializedObject) : base(serializedObject)
        {
            _defaultPrefab = LDtkProject.LoadDefaultGridPrefab();
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
            OverrideGridField();
                
            base.DrawDropdownContent(datas);
        }

        private void DefaultAssetField()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Default Grid", _defaultPrefab, typeof(Grid), false);
            GUI.enabled = true;
        }

        private void OverrideGridField()
        {
            SerializedProperty gridPrefabProp = SerializedObject.FindProperty(LDtkProject.TILEMAP_PREFAB_DEFAULT);

            GUIContent content = new GUIContent()
            {
                text = "Default Override",
                tooltip = "Optional. Assign a prefab here if you wish to override the default Tilemap prefab.",
            };
            
            EditorGUILayout.PropertyField(gridPrefabProp, content);
        }
    }
}