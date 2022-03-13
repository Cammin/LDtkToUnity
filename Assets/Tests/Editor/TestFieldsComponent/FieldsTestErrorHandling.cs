using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class TestErrorHandling : FieldsTestBase
    {
        [Test]
        public void NonExistentField()
        {
            Assert.NotNull(Fields, "Fields != null");

            const string identifier = "String";
            Assert.True(Fields.ContainsField(identifier));
        }
        
        
        [Test]
        public void OutOfBounds()
        {
            
        }

        [Test]
        public void MismatchType()
        {
            Assert.NotNull(Fields, "Fields != null");

            const string identifier = "String";
            Assert.True(Fields.ContainsField(identifier));
            
            if (!Fields.TryGetString(identifier, out string value))
            {
                Assert.Fail("Didnt get string");
            }

            
            
            Debug.Log(value);
            


        }
    
        [Test]
        public void MismatchArray()
        {
            Debug.Log("checking fields");
            Assert.NotNull(Fields);
        }
    }
    
    
}
