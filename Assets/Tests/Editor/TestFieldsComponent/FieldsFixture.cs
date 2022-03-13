using LDtkUnity;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor
{
    public static class FieldsFixture
    {
        private const string PATH = "Assets/Tests/Editor/TestFieldsComponent/Fixtures/TestFieldsComponent.prefab";

        public static LDtkFields Fields;
        
        public static void LoadComponent()
        {
            //Debug.Log("load");

            if (Fields != null)
            {
                return;
            }
            
            
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(PATH);

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
