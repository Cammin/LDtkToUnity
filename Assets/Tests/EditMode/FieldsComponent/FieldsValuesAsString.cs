using LDtkUnity.Tests;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class FieldsValuesAsString : FieldsTestBase
    {
        #region fails
        [Test]
        public void EnsureStringValuesFailure()
        {
            //ensure failures\
            //Fields.GetValueAsString()
            
            string valueAsString = Fields.GetValueAsString(FixtureConsts.SINGLE_COLOR);
            
            string[] valuesAsStrings = Fields.GetValuesAsStrings(FixtureConsts.ARRAY_COLOR);
            
        }
        
        [Test, TestCaseSource(nameof(Attempts))]
        public void Fail_GetStringValueForArray(string s)
        {
            float goal = 1.2345f;
            Assert.True(Fields.TryGetValueAsString(s, out var value));
            Assert.AreEqual(value, goal, $"{value} != {goal}");
        }
        #endregion
        
        [Test, TestCaseSource(nameof(Attempts))]
        public void TryGetFloat_GetExpectedValues(string s)
        {
            float goal = 1.2345f;
            Assert.True(Fields.TryGetValueAsString(s, out var value));
            Assert.AreEqual(value, goal, $"{value} != {goal}");
        }
        
        [Test, TestCaseSource(nameof(Attempts))]
        public void TryGetFloat_GetExpectedValues(string s)
        {
            float goal = 1.2345f;
            Assert.True(Fields.TryGetFloat(s, out var value));
            Assert.AreEqual(value, goal, $"{value} != {goal}");
        }
    }
}