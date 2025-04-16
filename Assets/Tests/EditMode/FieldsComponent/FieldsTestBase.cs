using System.Collections.Generic;
using NUnit.Framework;

namespace LDtkUnity.Tests
{
    [TestFixture]
    public abstract class FieldsTestBase
    {
        protected static string[] All = FixtureConsts.All;
        protected static string[] Singles = FixtureConsts.Singles;
        protected static string[] Arrays = FixtureConsts.Arrays;
        protected static Dictionary<string,object> ExpectedSingleValues = FixtureConsts.ExpectedSingleValues;
        protected static Dictionary<string,object[]> ExpectedArrayValues = FixtureConsts.ExpectedArrayValues;
        protected static Dictionary<string,string> ExpectedValuesAsString = FixtureConsts.ExpectedValuesAsString;
        protected static Dictionary<string,string> ExpectedValuesAsStringForToc = FixtureConsts.ExpectedValuesAsStringForToc;
        
        protected LDtkFields Fields;
        protected LDtkFields FieldsNullable;
        protected LDtkTableOfContents Toc;

        [SetUp]
        public void Setup()
        {
            FieldsFixture.LoadComponents();
            Fields = FieldsFixture.Fields;
            FieldsNullable = FieldsFixture.FieldsNullable;
            Toc = FieldsFixture.Toc;
        }
    }
}