using NUnit.Framework;

namespace Tests.Editor
{
    public class TryGetNullIdentifierIsNull : FieldsTestBase
    {
        [Test]
        public void TryGetInt_UnknownIdentifier_IsNull()
        {
            string identifier = "testRandomFailure";

            bool actual = Fields.TryGetInt(identifier, out _);
            
            Assert.False(actual);
        }
        
    }
}