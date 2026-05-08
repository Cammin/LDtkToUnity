using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkDrawerLayerCustomSortingOrder : LDtkContentDrawer<LayerDefinition>
    {
        private SerializedProperty _propMain;
        private SerializedProperty _propLayerName;
        private SerializedProperty _propOrderValue;

        private GUIContent _content;
        
        public LDtkDrawerLayerCustomSortingOrder(LayerDefinition data, SerializedProperty property) : base(data)
        {
            _propMain = property;
            _propLayerName = _propMain.FindPropertyRelative(LDtkLayerCustomSortingOrder.NAME);
            _propOrderValue = _propMain.FindPropertyRelative(LDtkLayerCustomSortingOrder.ORDER);

            _content = new GUIContent(_propLayerName.stringValue, LDtkIconUtility.GetIconForLayerDefinition(data));
        }
        
        public override void Draw()
        {
            _propLayerName.stringValue = _data.Identifier;
            _propOrderValue.intValue = EditorGUILayout.IntField(_content, _propOrderValue.intValue);
        }
    }
}