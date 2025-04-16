using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LDtkUnity.Tests
{
    public class FieldsValuesAsString : FieldsTestBase
    {
        #region ExpectIncorrectValues

        /*
        [Test, TestCaseSource(nameof(Singles))]
        public void GetValueAsString_ExpectIncorrectValue(string s)
        {
            var value = Fields.GetValueAsString(s);
            
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }
        */
        
        /*[Test, TestCaseSource(nameof(Arrays))]
        public void GetValuesAsStrings_ExpectIncorrectValue(string s)
        {
            var value = Fields.GetValuesAsStrings(s);
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }*/
        
        /*[Test, TestCaseSource(nameof(Singles))]
        public void TryGetValueAsString_ExpectIncorrectValue(string s)
        {
            Assert.True(Fields.TryGetValueAsString(s, out var value));
            
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }
        
        [Test, TestCaseSource(nameof(Arrays))]
        public void TryGetValuesAsStrings_ExpectIncorrectValue(string s)
        {
            Assert.True(Fields.TryGetValuesAsStrings(s, out var value));
            Assert.AreNotEqual(value, s, $"{value} != {s}");
        }*/
        #endregion
        
        #region ExpectFailWhenGettingMismatchedSingleOrArray
        [Test, TestCaseSource(nameof(Singles))]
        public void GetValuesAsStrings_ExpectFailWhenGettingArrayForSingle(string s)
        {
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            Fields.GetValuesAsStrings(s);
        }
        [Test, TestCaseSource(nameof(Singles))]
        public void TryGetValuesAsStrings_ExpectFailWhenGettingArrayForSingle(string s)
        {
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            Assert.False(Fields.TryGetValuesAsStrings(s, out _));
        }
        [Test, TestCaseSource(nameof(Arrays))]
        public void GetValuesAsStrings_ExpectFailWhenGettingSingleForArray(string s)
        {
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            Fields.GetValueAsString(s);
        }
        [Test, TestCaseSource(nameof(Arrays))]
        public void TryGetValueAsString_ExpectFailWhenGettingSingleForArray(string s)
        {
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            Assert.False(Fields.TryGetValueAsString(s, out _));
        }
        #endregion
        
        [Test, TestCaseSource(nameof(ExpectedValuesAsString))]
        public void TryGetValueAsString_GetExpectedValues(KeyValuePair<string, string> pair)
        {
            string identifier = pair.Key;
            string expected = pair.Value;

            string actualValue;
            if (Fields.IsArray(identifier))
            {
                Assert.True(Fields.TryGetValuesAsStrings(identifier, out string[] values));
                actualValue = string.Join(", ", values);
            }
            else
            {
                Assert.True(Fields.TryGetValueAsString(identifier, out actualValue));
            }

            Assert.AreEqual(expected, actualValue, $"{actualValue} != {expected}, should be equal");
        }
    }
}