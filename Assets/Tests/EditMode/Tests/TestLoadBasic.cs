using NUnit.Framework;

namespace LDtkUnity.Tests
{
    public class TestLoadBasic
    {
        [Test]
        public static void DeserializeBasicUtf8()
        {
            string jsonText = @"{""foo"":""json"",""bar"":100,""nest"":{""foobar"":true},""fooEnum"":""OtherThing""}";

            TestThing project = Utf8Json.JsonSerializer.Deserialize<TestThing>(jsonText);
            Assert.NotNull(project, "Failure to deserialize LDtk project");
        }
        public class TestThing
        {
            public string foo;
            public int bar;
            public Nest nest;
            public TestEnum fooEnum;
        }

        public class Nest
        {
            public bool foobar;
        }

        public enum TestEnum
        {
            Thing,
            OtherThing
        }
    }
}