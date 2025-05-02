using NUnit.Framework;
using UnityEngine;

namespace LDtkUnity.Tests
{
    internal sealed class TestLoadTocFieldValues : FieldsTestBase
    {
        //public const string FieldsProjectPath = "Assets/Tests/Misc/OtherTechTests/TestAllFieldsLevels.ldtk";
        
        /*[Test]
        public void TestTocJson()
        {
            string jsonText = TestJsonLoader.LoadTextAsset(FieldsProjectPath);
            LdtkJson project = Utf8Json.JsonSerializer.Deserialize<LdtkJson>(jsonText);
            LdtkTableOfContentEntry[] toc = project.Toc;
            LdtkTableOfContentEntry entry = toc[0];
            LdtkTocInstanceData data = entry.InstancesData[0];
            var fields = data.Fields;

            foreach (var key in fields.Keys)
            {
                object value = fields[key];
                
                Debug.Log($"{key}: {value}");
            }
            
            double intValue = (double)fields["integer"];
            double floatValue = (double)fields["float"];
            string stringValue = (string)fields["string"];
            
            
            Debug.Assert(intValue == 12345);
        }*/

        [Test]
        public void TestTocLength()
        {
            LDtkTableOfContentsEntry entry = Toc.GetEntry("TestAllFields");
            LDtkTableOfContentsEntryData firstEntity = entry.Entries[0];
            Assert.IsTrue(All.Length == firstEntity.Fields.Length);
        }

        [Test, TestCaseSource(nameof(All))]
        public void TestTocValues(string fieldIdentifier)
        {
            LDtkTableOfContentsEntry entry = Toc.Entries[0];
            Debug.Assert(entry.Definition.Identifier == "TestAllFields");
            
            LDtkTableOfContentsEntryData entity = entry.Entries[0];
            LDtkField field = entity.GetField(fieldIdentifier);

            string expected = ExpectedValuesAsStringForToc[fieldIdentifier];
            string actual;

            
            if (field.Definition.IsArray)
            {
                string[] values = field.GetValuesAsStrings();
                actual = string.Join(", ", values);
            }
            else
            {
                actual = field.GetValueAsString();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}