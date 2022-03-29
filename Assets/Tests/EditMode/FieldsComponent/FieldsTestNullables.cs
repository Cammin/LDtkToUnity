using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LDtkUnity.Tests
{
    public class FieldsTestNullables : FieldsTestBase
    {
        [Test, TestCaseSource(nameof(Singles))]
        public void IsNull_EnsureNull(string s)
        {
            if (s == FixtureConsts.SINGLE_BOOL || s == FixtureConsts.SINGLE_COLOR)
            {
                return;
            }
            
            Assert.IsTrue(FieldsNullable.IsNull(s), $"field should exist and be null. Exists: {FieldsNullable.ContainsField(s)}");
        }
        
        [Test, TestCaseSource(nameof(Arrays))]
        public void IsNull_EnsureNullArrayValues(string s)
        {
            if (s == FixtureConsts.ARRAY_BOOL || s == FixtureConsts.ARRAY_COLOR)
            {
                return;
            }
            
            Assert.IsTrue(FieldsNullable.IsNullAtArrayIndex(s, 0), "FieldsNullable.IsNullAtArrayIndex(s, 0)");
            Assert.IsFalse(FieldsNullable.IsNullAtArrayIndex(s, 1), "FieldsNullable.IsNullAtArrayIndex(s, 1)");
        }
        
        [Test, TestCaseSource(nameof(Arrays))]
        public void IsNull_AssureImproperSingleUsage(string s)
        { 
            if (s == FixtureConsts.ARRAY_BOOL || s == FixtureConsts.ARRAY_COLOR)
            {
                return;
            }
            
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            FieldsNullable.IsNull(s);
        }
        
        [Test, TestCaseSource(nameof(Singles))]
        public void IsNull_AssureImproperArrayUsage(string s)
        {
            if (s == FixtureConsts.SINGLE_BOOL || s == FixtureConsts.SINGLE_COLOR)
            {
                return;
            }
            
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            FieldsNullable.IsNullAtArrayIndex(s, 0);
        }
    }
}