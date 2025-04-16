using NUnit.Framework;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public static class FieldsFixture
    {
        private const string PATH = "FieldsFixture";
        private const string PATH_NULLABLE = "FieldsFixture_Nullable";
        private const string PATH_SPRITE = "FixtureSprite";
        private const string PATH_TOC = "TestAllFields_Toc";

        public static LDtkFields Fields;
        public static LDtkFields FieldsNullable;
        public static LDtkTableOfContents Toc;
        public static Sprite Sprite;
        
        public static void LoadComponents()
        {
            if (Fields == null)
            {
                Fields = LoadFieldsComponent(PATH);
            }
            
            if (FieldsNullable == null)
            {
                FieldsNullable = LoadFieldsComponent(PATH_NULLABLE);
            }

            if (Toc == null)
            {
                Toc = LoadToc(PATH_TOC);
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

        private static LDtkFields LoadFieldsComponent(string pathNullable)
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
        
        private static LDtkTableOfContents LoadToc(string pathNullable)
        {
            return LoadObject<LDtkTableOfContents>(pathNullable);
        }

        private static T LoadObject<T>(string pathNullable) where T : Object
        {
            T obj = Resources.Load<T>(pathNullable);

            Assert.IsNotNull(obj, $"Issue loading fixture from resources: {pathNullable}");
            return obj;
        }
    }
}
