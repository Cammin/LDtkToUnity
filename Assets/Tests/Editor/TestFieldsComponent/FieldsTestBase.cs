using LDtkUnity;
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
    }
}