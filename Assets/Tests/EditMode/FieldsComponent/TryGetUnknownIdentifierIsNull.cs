using NUnit.Framework;

namespace Tests.Editor
{
    public class TryGetUnknownIdentifierIsNull : FieldsTestBase
    {
        [Test]
        public void TryGetInt_UnknownIdentifier_IsNull()
        {
            string identifier = "testRandomFailure";

            bool actual = Fields.TryGetInt(identifier, out _);
            
            Assert.False(actual);
        }
        
        [Test]
        public void TryGetFloat_UnknownIdentifier_IsNull()
        {
            string identifier = "testRandomFailure";

            bool actual = Fields.TryGetFloat(identifier, out _);
            
            Assert.False(actual);
        }
        
    }
}