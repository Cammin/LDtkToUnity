using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public static class FieldsFixture
    {
        private const string PATH = "FieldsFixture";
        private const string PATH_NULLABLE = "FieldsFixture_Nullable";

        public static LDtkFields Fields;
        
        public static void LoadComponent()
        {
            //Debug.Log("load");

            if (Fields != null)
            {
                return;
            }
            
            //if ()
            
            GameObject obj = Resources.Load<GameObject>(PATH);

            if (obj == null)
            {
                Debug.LogError("issue");
                return;
            }

            LDtkFields fields = obj.GetComponent<LDtkFields>();
            if (!fields)
            {
                Debug.LogError("issue");
                return;
            }

            //Debug.Log("loaded fixture");
            Fields = fields;
        }
    }
}
