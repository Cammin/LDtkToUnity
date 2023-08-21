using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LDtkUnity.Tests
{
    public static class FieldsFixture
    {
        private const string PATH = "FieldsFixture";
        private const string PATH_NULLABLE = "FieldsFixture_Nullable";
        private const string PATH_SPRITE = "FixtureSprite";

        public static LDtkFields Fields;
        public static LDtkFields FieldsNullable;
        public static Sprite Sprite;
        
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

            LoadSprite();
        }

        public static Sprite LoadSprite()
        {
            if (Sprite == null)
            {
                Sprite = LoadObject<Sprite>(PATH_SPRITE);
            }

            return Sprite;
        }

        private static LDtkFields GetValue(string pathNullable)
        {
            GameObject obj = LoadObject<GameObject>(pathNullable);

            if (obj == null)
            {
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

        private static T LoadObject<T>(string pathNullable) where T : Object
        {
            T obj = Resources.Load<T>(pathNullable);

            Assert.IsNotNull(obj, $"Issue loading fixture from resources: {pathNullable}");
            return obj;
        }
    }
}
