﻿using NUnit.Framework;

namespace LDtkUnity.Tests
{
    public class FixtureAssertions : FieldsTestBase
    {
        [Test, TestCaseSource(nameof(Arrays))]
        public void IsArraySizeExpected(string s)
        {
            Assert.AreEqual(2, Fields.GetArraySize(s));
        }
        
        [Test, TestCaseSource(nameof(All))]
        public void ContainsAllFields(string s)
        {
            Assert.True(Fields.ContainsField(s), $"Fields.ContainsField({s})");
        }
        
        [Test, TestCaseSource(nameof(Arrays))]
        public void IsArraySizeExpectedNullable(string s)
        {
            Assert.AreEqual(FieldsNullable.GetArraySize(s), 2);
        }
        
        [Test, TestCaseSource(nameof(All))]
        public void ContainsAllFieldsNullable(string s)
        {
            Assert.True(FieldsNullable.ContainsField(s), $"Fields.ContainsField({s})");
        }
    }
}