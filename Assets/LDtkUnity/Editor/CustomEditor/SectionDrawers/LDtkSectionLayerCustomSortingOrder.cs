using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSectionLayerCustomSortingOrder : LDtkSectionDataDrawer<LayerDefinition>
    {
        private SerializedProperty _useProp;
        private static GUIContent _usePropLabel = new GUIContent("Use Custom Sorting Orders");
        
        protected override string PropertyName => LDtkProjectImporter.LAYER_SORTING_ORDERS;
        protected override string GuiText => "Custom Sorting Orders";

        protected override string GuiTooltip =>
            "Normally in the import process, the sorting order is decremented for all occurrences of LDtk layers, starting from 0.\n" +
            "However, this option can be toggled on to customize sorting orders for specific layers.";
        
        protected override Texture GuiImage => LDtkIconUtility.LoadLayerIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_CUSTOM_SORTING_ORDER;

        public LDtkSectionLayerCustomSortingOrder(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
        {
        }

        public override void Init()
        {
            base.Init();
            _useProp = SerializedObject.FindProperty(LDtkProjectImporter.USE_LAYER_SORTING_ORDERS);
        }
        
        protected override void GetDrawers(LayerDefinition[] defs, List<LDtkContentDrawer<LayerDefinition>> drawers)
        {
            for (var i = 0; i < defs.Length; i++)
            {
                LDtkDrawerLayerCustomSortingOrder drawer = 
                    new LDtkDrawerLayerCustomSortingOrder(defs[i], ArrayProp.GetArrayElementAtIndex(i));
                
                drawers.Add(drawer);
            }
        }
        
        protected override void DrawDropdownContent()
        {
            EditorGUILayout.PropertyField(_useProp, _usePropLabel);
            using (new EditorGUI.DisabledScope(!_useProp.boolValue))
            {
                base.DrawDropdownContent();
            }
        }

        protected override SerializedProperty GetKeyPropForElement(SerializedProperty element)
        {
            return element.FindPropertyRelative(LDtkLayerCustomSortingOrder.NAME);
        }

        protected override SerializedProperty GetValuePropForElement(SerializedProperty element)
        {
            return element.FindPropertyRelative(LDtkLayerCustomSortingOrder.ORDER);
        }

        protected override void SetDefaultElementValue(SerializedProperty insertedValueProp, int i)
        {
            insertedValueProp.intValue = -i;
        }
    }
}