using LDtkUnity.Tests;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class TryGetUnknownIdentifierIsNull : FieldsTestBase
    {
        private static readonly string[] Tests = new[]
        {
            "testRandomFailure",
            "",
            null
        };

        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetInt_Fail(string input) => Assert.False(Fields.TryGetInt(input, out _));

        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetIntArray_Fail(string input) => Assert.False(Fields.TryGetIntArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetFloat_Fail(string input) => Assert.False(Fields.TryGetFloat(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetFloatArray_Fail(string input) => Assert.False(Fields.TryGetFloatArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetBool_Fail(string input) => Assert.False(Fields.TryGetBool(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetBoolArray_Fail(string input) => Assert.False(Fields.TryGetBoolArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetString_Fail(string input) => Assert.False(Fields.TryGetString(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetStringArray_Fail(string input) => Assert.False(Fields.TryGetStringArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetMultiline_Fail(string input) => Assert.False(Fields.TryGetMultiline(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetMultilineArray_Fail(string input) => Assert.False(Fields.TryGetMultilineArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetFilePath_Fail(string input) => Assert.False(Fields.TryGetFilePath(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetFilePathArray_Fail(string input) => Assert.False(Fields.TryGetFilePathArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetColor_Fail(string input) => Assert.False(Fields.TryGetColor(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetColorArray_Fail(string input) => Assert.False(Fields.TryGetColorArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetEnum_Fail(string input) => Assert.False(Fields.TryGetEnum<SomeEnum>(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetEnumArray_Fail(string input) => Assert.False(Fields.TryGetEnumArray<SomeEnum>(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetPoint_Fail(string input) => Assert.False(Fields.TryGetPoint(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetPointArray_Fail(string input) => Assert.False(Fields.TryGetPointArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetEntityReference_Fail(string input) => Assert.False(Fields.TryGetEntityReference(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetEntityReferenceArrayArray_Fail(string input) => Assert.False(Fields.TryGetEntityReferenceArray(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetTile_Fail(string input) => Assert.False(Fields.TryGetTile(input, out _));
        
        [Test, TestCaseSource(nameof(Tests))]
        public void TryGetTileArray_Fail(string input) => Assert.False(Fields.TryGetTileArray(input, out _));
    }
}