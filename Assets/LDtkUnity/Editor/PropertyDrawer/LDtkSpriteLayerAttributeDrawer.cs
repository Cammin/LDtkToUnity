using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
	[ExcludeFromDocs]
	[CustomPropertyDrawer(typeof(LDtkLayerMaskAttribute))]
	public class LDtkLayerMaskAttributeDrawer : PropertyDrawer
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