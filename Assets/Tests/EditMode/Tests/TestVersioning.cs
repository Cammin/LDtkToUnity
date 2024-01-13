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
            "1.1.0",
            "1.1.1",
            "1.1.1-beta",
            "1.1.2",
            "1.1.3",
            "1.2.0",
            "1.2.5",
            "1.3.0",
            "1.3.1",
            "1.3.2",
            "1.4.1",
        };
        private static string[] _versions = new[]
        {
            "1.5.0",
            "1.5.1",
            "1.5.2",
        };
        
        
        [Test, TestCaseSource(nameof(_oldVersions))]
        public void TryOldVersions(string ver)
        {
            LogAssert.Expect(LogType.Error, new Regex($"<color={LDtkDebug.GetStringColor()}>LDtk:</color> Unhandled import error: The version of the project*"));

            LDtkProjectImporter.CheckOutdatedJsonVersion(ver, "test");
        }
        [Test, TestCaseSource(nameof(_versions))]
        public void TryVersions(string ver)
        {
            LDtkProjectImporter.CheckOutdatedJsonVersion(ver, "test");
        }
    }
}