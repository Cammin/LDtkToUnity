using LDtkUnity;
using LDtkUnity.Tests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Editor
{
    [TestFixture]
    public abstract class FieldsTestBase
    {
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