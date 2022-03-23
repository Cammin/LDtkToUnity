using LDtkUnity.Tests;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class FieldsTestMisc : FieldsTestBase
    {
        [Test]
        public void IsArraySizeExpected()
        {
            InitialAssert();

            foreach (string array in FixtureConsts.Arrays)
            {
                Assert.AreEqual(Fields.GetArraySize(array), 2);
            }
        }
        
        [Test]
        public void ContainsAllFields()
        {
            InitialAssert();

            foreach (string single in FixtureConsts.All)
            {
                Assert.True(Fields.ContainsField(single), $"Fields.ContainsField({single})");
            }
        }
    }
}