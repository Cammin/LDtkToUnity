using System.Text.RegularExpressions;
using LDtkUnity.Editor;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LDtkUnity.Tests
{
    public class TestVersioning
    {
        private static string[] _oldVersions = new[]
        {
            "1.0.0",
            "1.0.0-beta",
        };
        private static string[] _versions = new[]
        {
            "1.1.0",
            "1.1.1",
            "1.1.1-beta",
            "1.1.2",
            "1.1.3",
        };
        
        
        [Test, TestCaseSource(nameof(_oldVersions))]
        public void TryOldVersions(string ver)
        {
            LogAssert.Expect(LogType.Error, new Regex("<color=yellow>LDtk:</color> The version of the project*"));
            LDtkProjectImporter.CheckOutdatedJsonVersion(ver, "test");
        }
        [Test, TestCaseSource(nameof(_versions))]
        public void TryVersions(string ver)
        {
            LDtkProjectImporter.CheckOutdatedJsonVersion(ver, "test");
        }
    }
}