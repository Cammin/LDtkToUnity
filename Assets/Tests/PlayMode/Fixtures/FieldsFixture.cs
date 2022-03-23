using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public static class FieldsFixture
    {
        private const string PATH = "FieldsFixture";
        private const string PATH_NULLABLE = "FieldsFixture_Nullable";

        public static LDtkFields Fields;
        public static LDtkFields FieldsNullable;
        
        public static void LoadComponents()
        {
            if (Fields == null)
            {
                Fields = GetValue(PATH);
            }
            
            if (FieldsNullable == null)
            {
                FieldsNullable = GetValue(PATH_NULLABLE);
            }
        }

        private static LDtkFields GetValue(string pathNullable)
        {
            GameObject obj = Resources.Load<GameObject>(pathNullable);

            if (obj == null)
            {
                Debug.LogError("issue");
                return null;
            }

            LDtkFields fields = obj.GetComponent<LDtkFields>();
            if (!fields)
            {
                Debug.LogError("issue");
                return null;
            }

            return fields;
        }
    }
}
