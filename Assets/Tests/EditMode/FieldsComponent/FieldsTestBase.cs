using System.Collections.Generic;
using LDtkUnity;
using LDtkUnity.Tests;
using NUnit.Framework;

namespace Tests.EditMode
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
        
        protected LDtkFields Fields;
        protected LDtkFields FieldsNullable;

        [SetUp]
        public void Setup()
        {
            FieldsFixture.LoadComponents();
            Fields = FieldsFixture.Fields;
            FieldsNullable = FieldsFixture.FieldsNullable;
        }
    }
}