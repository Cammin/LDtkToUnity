using LDtkUnity.Editor;
using NUnit.Framework;

namespace LDtkUnity.Tests
{
    public class TestVersioning
    {
        private static string[] _versions = new[]
        {
            "1.0.0",
            "1.0.0-beta",
            "1.1.0",
            "1.1.1",
            "1.1.1-beta",
        };
        
        
        [Test, TestCaseSource(nameof(_versions))]
        public void TryVersions(string ver)
        {
            LDtkProjectImporter.CheckOutdatedJsonVersion(ver, "test");
        }
    }
}