using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LDtkUnity.Tests
{
    public class TestErrorHandling : FieldsTestBase
    {
        const string identifier = "String";
        
        [Test]
        public void NonExistentField()
        {
            Assert.NotNull(Fields, "Fields != null");
            Assert.True(Fields.ContainsField(identifier));
            
            
        }
        
        [Test]
        public void OutOfBounds()
        {
            Assert.NotNull(Fields, "Fields != null");
            Assert.True(Fields.ContainsField(FixtureConsts.ARRAY_STRING));

            int arraySize = Fields.GetArraySize(FixtureConsts.ARRAY_STRING);

            LogAssert.ignoreFailingMessages = true;
            
            
            
            LogAssert.Expect(LogType.Error, new Regex($"<color={LDtkDebug.GetStringColor()}>LDtk:</color> Out of range*"));
            Fields.IsNullAtArrayIndex(FixtureConsts.ARRAY_STRING, -1);
            
            LogAssert.Expect(LogType.Error, new Regex($"<color={LDtkDebug.GetStringColor()}>LDtk:</color> Out of range*"));
            Fields.IsNullAtArrayIndex(FixtureConsts.ARRAY_STRING, arraySize);
        }
        
        

        [Test]
        public void MismatchType()
        {
            Assert.NotNull(Fields, "Fields != null");
            Assert.True(Fields.ContainsField(identifier));
            
            if (!Fields.TryGetString(identifier, out string value))
            {
                Assert.Fail("Didnt get string");
            }

            
            
            Debug.Log(value);
            


        }


        
        
        
        /// <summary>
        /// Try checking for a null array element when it's not an array
        /// </summary>
        private bool CheckInvalidNullCheckUsage()
        {
            return false;
        }
    }
    
    
}
