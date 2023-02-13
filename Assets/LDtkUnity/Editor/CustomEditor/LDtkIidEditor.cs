using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkIid))]
    [CanEditMultipleObjects]
    internal sealed class LDtkIidEditor : UnityEditor.Editor
    {
        private static readonly GUIContent IidInfo = new GUIContent()
        {
            text = "iid",
            tooltip = "Unique instance identifier"
        };
        
        public override void OnInspectorGUI()
        {
            SerializedProperty property = serializedObject.FindProperty(LDtkIid.PROPERTY_IID);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(property, IidInfo);
            }
        }

        /// <returns>If the iid component was found</returns>
        public static bool DrawIidAndGameObject(Rect position, Rect labelRect, SerializedProperty iidProp, GUIContent label)
        {
            Profiler.BeginSample("LDtkFieldElementDrawer.DrawEntityRef");

            //GUILayout.BeginArea(position);
            
            string iid = iidProp.stringValue;

            if (string.IsNullOrEmpty(iid))
            {
                Profiler.EndSample();
                return false;
            }

            LDtkIid component = LDtkIidComponentBank.FindObjectOfIid(iid);
            if (component == null)
            {
                EditorGUI.PropertyField(position, iidProp, label);
                Profiler.EndSample();
                return false;
            }

            float desiredObjectWidth = 175;
            float objectWidth = Mathf.Min(desiredObjectWidth, position.width - desiredObjectWidth * 0.83f);
            float fieldWidth = Mathf.Max(position.width - objectWidth);
            
            Rect fieldRect = new Rect(
                position.x, 
                position.y, 
                fieldWidth - 2, 
                position.height);
            
            Rect gameObjectRect = new Rect(
                position.x + fieldWidth, 
                position.y, 
                Mathf.Max(desiredObjectWidth, objectWidth), 
                position.height);

            fieldRect.xMin = labelRect.xMin;

            EditorGUI.PropertyField(fieldRect, iidProp, label);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.ObjectField(gameObjectRect, component.gameObject, typeof(GameObject), true);
            }

            Profiler.EndSample();
            return true;
        }
    }
}