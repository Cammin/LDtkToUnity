using System.Collections.Generic;
using LDtkUnity.Tests;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class FieldsValuesAsString : FieldsTestBase
    {
        
        
        
        #region fails

        [Test, TestCaseSource(nameof(Singles))]
        public void GetValueAsString_Fail(string s)
        {
            var value = Fields.GetValueAsString(s);
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }
        
        [Test, TestCaseSource(nameof(Arrays))]
        public void GetValuesAsStrings_Fail(string s)
        {
            var value = Fields.GetValuesAsStrings(s);
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }
        
        [Test, TestCaseSource(nameof(Singles))]
        public void TryGetValueAsString_Fail(string s)
        {
            Assert.True(Fields.TryGetValueAsString(s, out var value));
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }
        
        [Test, TestCaseSource(nameof(Arrays))]
        public void TryGetValuesAsStrings_Fail(string s)
        {
            Assert.True(Fields.TryGetValuesAsStrings(s, out var value));
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }
        

        #endregion
        
        [Test, TestCaseSource(nameof(ExpectedValuesAsString))]
        public void TryGetFloat_GetExpectedValues(string s)
        {
            if (!ExpectedValuesAsString.ContainsKey(s))
            {
                Assert.Fail("no key");
            }
            
            string goal = ExpectedValuesAsString[s];
            Assert.True(Fields.TryGetValueAsString(s, out var value));
            Assert.AreEqual(value, goal, $"{value} != {goal}, should be equal");
        }

    }
}