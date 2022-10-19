using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
	[CustomPropertyDrawer(typeof(LDtkLayerMaskAttribute))]
	internal sealed class LDtkLayerMaskAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.Integer)
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}
			
			property.intValue = EditorGUI.LayerField(position, label, property.intValue);
		}
	}
}