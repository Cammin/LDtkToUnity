using System;
using System.Text.RegularExpressions;
using LDtkUnity.Tests;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Editor
{
    //when we do a tryget:
    //if the field exists
    //if getting an array is an array and not single and vice versa
    //if the types by identifier is proper


    public class FieldsTestTryGet : FieldsTestBase
    {
        private enum Mode
        {
            All,
            Specific,
            Except
        }

        private static string[] _attempts = FixtureConsts.All;
        
        [Test, TestCaseSource(nameof(_attempts))]
        public void TryFailIncompatibleTypes(string s)
        {
            CheckFailedTryGet(Assert.False, s, Mode.Except);
            //LogAssert.NoUnexpectedReceived();
        }

        [Test, TestCaseSource(nameof(_attempts))]
        public void TryGetCompatibleTypes(string s)
        {
            CheckFailedTryGet(Assert.True, s, Mode.Specific);
            //LogAssert.NoUnexpectedReceived();
        }

        private delegate void Assertion(bool condition, string message, params object[] args);
        
        private void CheckFailedTryGet(Assertion assertion, string identifier, Mode mode)
        {
            string msg = "";
            
            bool ShouldCheck(string check)
            {
                msg = $"FieldName:{identifier}, TryGet{check}";
                switch (mode)
                {
                    case Mode.All:
                        return true;
                    case Mode.Specific:
                        return identifier == check;
                    case Mode.Except:
                        return identifier != check;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }

            if (ShouldCheck(FixtureConsts.SINGLE_INT)) { assertion.Invoke(Fields.TryGetInt(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_FLOAT)) { assertion.Invoke(Fields.TryGetFloat(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_BOOL)) { assertion.Invoke(Fields.TryGetBool(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_STRING)) { assertion.Invoke(Fields.TryGetString(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_MULTILINES)) { assertion.Invoke(Fields.TryGetMultiline(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_ENUM)) { assertion.Invoke(Fields.TryGetEnum<SomeEnum>(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_COLOR)) { assertion.Invoke(Fields.TryGetColor(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_POINT)) { assertion.Invoke(Fields.TryGetPoint(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_FILE_PATH)) { assertion.Invoke(Fields.TryGetFilePath(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_ENTITY_REF)) { assertion.Invoke(Fields.TryGetEntityReference(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.SINGLE_TILE)) { assertion.Invoke(Fields.TryGetTile(identifier, out _), msg); }
            
            if (ShouldCheck(FixtureConsts.ARRAY_INT)) { assertion.Invoke(Fields.TryGetIntArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_FLOAT)) { assertion.Invoke(Fields.TryGetFloatArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_BOOL)) { assertion.Invoke(Fields.TryGetBoolArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_STRING)) { assertion.Invoke(Fields.TryGetStringArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_MULTILINES)) { assertion.Invoke(Fields.TryGetMultilineArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_ENUM)) { assertion.Invoke(Fields.TryGetEnumArray<SomeEnum>(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_COLOR)) { assertion.Invoke(Fields.TryGetColorArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_POINT)) { assertion.Invoke(Fields.TryGetPointArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_FILE_PATH)) { assertion.Invoke(Fields.TryGetFilePathArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_ENTITY_REF)) { assertion.Invoke(Fields.TryGetEntityReferenceArray(identifier, out _), msg); }
            if (ShouldCheck(FixtureConsts.ARRAY_TILE)) { assertion.Invoke(Fields.TryGetTileArray(identifier, out _), msg); }
            

        }

        [Test]
        [TestCase(FixtureConsts.SINGLE_INT)]
        [TestCase(FixtureConsts.ARRAY_INT)]
        public void SingleTest(string identifier)
        {
            bool success = Fields.TryGetFloat(identifier, out _);
            LogAssert.NoUnexpectedReceived();
            Assert.False(success);
        }
        
        [Test]
        [TestCase(FixtureConsts.SINGLE_INT)]
        [TestCase(FixtureConsts.ARRAY_INT)]
        public void ArrayTest(string identifier)
        {
            bool success = Fields.TryGetFloatArray(identifier, out _);
            LogAssert.NoUnexpectedReceived();
            Assert.False(success);
        }

        [Test]
        [TestMustExpectAllLogs]
        public void TestTest()
        {
            //LogAssert.Expect(LogType.Error, "test");
            //LogAssert.NoUnexpectedReceived();
            if (false)
            {
                Debug.LogError("thing");
            }
            
            //LogAssert.Expect(LogType.Error, "test");
            //Debug.LogError("test");
            //LogAssert.NoUnexpectedReceived();
        }

        
        
        private void TryAction(string hkj)
        {
            
        }



    }
}