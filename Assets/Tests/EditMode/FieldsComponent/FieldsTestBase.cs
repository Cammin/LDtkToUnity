using LDtkUnity;
using LDtkUnity.Tests;
using NUnit.Framework;

namespace Tests.EditMode
{
    [TestFixture]
    public abstract class FieldsTestBase
    {
        protected static string[] Attempts = FixtureConsts.All;
        protected static string[] Singles = FixtureConsts.Singles;
        protected static string[] Arrays = FixtureConsts.Arrays;
        
        protected LDtkFields Fields;

        [SetUp]
        public void Setup()
        {
            FieldsFixture.LoadComponent();
            Fields = FieldsFixture.Fields;
        }

        protected void InitialAssert()
        {
            Assert.NotNull(Fields, "Fields != null");
        }
    }
}