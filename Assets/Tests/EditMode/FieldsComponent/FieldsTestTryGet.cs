using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace LDtkUnity.Tests
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

        
        
        [Test, TestCaseSource(nameof(All))]
        public void TryFailIncompatibleTypes(string s)
        {
            CheckFailedTryGet(Assert.False, s, Mode.Except);
        }

        [Test, TestCaseSource(nameof(All))]
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

                        bool shouldCheck = identifier != check;
                        if (shouldCheck)
                        {
                            LogAssert.Expect(LogType.Error, new Regex(".*"));
                        }
                        return shouldCheck;
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
        
        
        #region EqualGuesses

        private readonly ColorEqualityComparer _comparer = new ColorEqualityComparer(0.01f);
        private delegate bool TryGetSingleAction<T>(string identifier, out T values);
        private void AssertExpectedValue<T>(string identifier, TryGetSingleAction<T> action)
        {
            Assert.True(action.Invoke(identifier, out T value));
            
            object expected = ExpectedSingleValues[identifier];
            object actual = value;
            
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }
        
        private delegate bool TryGetArrayAction<T>(string identifier, out T values);
        private void AssertExpectedValues<T>(string identifier, TryGetArrayAction<T> action)
        {
            Assert.True(action.Invoke(identifier, out T value));
            IEnumerable iEnumerable = (IEnumerable)value;
            object[] actuals = iEnumerable.Cast<object>().ToArray();
            
            object[] expected = ExpectedArrayValues[identifier];

            for (int i = 0; i < actuals.Length; i++)
            {
                object actual = actuals[i];
                object expectedElement = expected[i];
                
                Assert.That(actual, Is.EqualTo(expectedElement).Using(_comparer));
            }
        }

        
        [Test]
        public void TryGetInt_GetExpectedValue() => AssertExpectedValue<int>(FixtureConsts.SINGLE_INT, Fields.TryGetInt);
        
        [Test]
        public void TryGetFloat_GetExpectedValue() => AssertExpectedValue<float>(FixtureConsts.SINGLE_FLOAT, Fields.TryGetFloat);
        
        [Test]
        public void TryGetBool_GetExpectedValue() => AssertExpectedValue<bool>(FixtureConsts.SINGLE_BOOL, Fields.TryGetBool);
        
        [Test]
        public void TryGetString_GetExpectedValue() => AssertExpectedValue<string>(FixtureConsts.SINGLE_STRING, Fields.TryGetString);
        
        [Test]
        public void TryGetMultiline_GetExpectedValue() => AssertExpectedValue<string>(FixtureConsts.SINGLE_MULTILINES, Fields.TryGetMultiline);
        
        [Test]
        public void TryGetColor_GetExpectedValue() => AssertExpectedValue<Color>(FixtureConsts.SINGLE_COLOR, Fields.TryGetColor);
        
        [Test]
        public void TryGetEnum_GetExpectedValue() => AssertExpectedValue<SomeEnum>(FixtureConsts.SINGLE_ENUM, Fields.TryGetEnum);
        
        [Test]
        public void TryGetFilePath_GetExpectedValue() => AssertExpectedValue<string>(FixtureConsts.SINGLE_FILE_PATH, Fields.TryGetFilePath);
        
        [Test]
        public void TryGetTile_GetExpectedValue() => AssertExpectedValue<Sprite>(FixtureConsts.SINGLE_TILE, Fields.TryGetTile);
        
        [Test]
        public void TryGetEntityRef_GetExpectedValue() => AssertExpectedValue<LDtkIid>(FixtureConsts.SINGLE_ENTITY_REF, (string identifier, out LDtkIid values) =>
        {
            bool found = Fields.TryGetEntityReference(identifier, out var outValues);
            values = outValues.FindEntity();
            return found;
        });
        
        [Test]
        public void TryGetPoint_GetExpectedValue() => AssertExpectedValue<Vector2>(FixtureConsts.SINGLE_POINT, Fields.TryGetPoint);
        
        [Test]
        public void TryGetIntArray_GetExpectedValue() => AssertExpectedValues<int[]>(FixtureConsts.ARRAY_INT, Fields.TryGetIntArray);
        
        [Test]
        public void TryGetFloatArray_GetExpectedValue() => AssertExpectedValues<float[]>(FixtureConsts.ARRAY_FLOAT, Fields.TryGetFloatArray);
        
        [Test]
        public void TryGetBoolArray_GetExpectedValue() => AssertExpectedValues<bool[]>(FixtureConsts.ARRAY_BOOL, Fields.TryGetBoolArray);
        
        [Test]
        public void TryGetStringArray_GetExpectedValue() => AssertExpectedValues<string[]>(FixtureConsts.ARRAY_STRING, Fields.TryGetStringArray);
        
        [Test]
        public void TryGetMultilineArray_GetExpectedValue() => AssertExpectedValues<string[]>(FixtureConsts.ARRAY_MULTILINES, Fields.TryGetMultilineArray);
        
        [Test]
        public void TryGetColorArray_GetExpectedValue() => AssertExpectedValues<Color[]>(FixtureConsts.ARRAY_COLOR, Fields.TryGetColorArray);
        
        [Test]
        public void TryGetEnumArray_GetExpectedValue() => AssertExpectedValues<SomeEnum[]>(FixtureConsts.ARRAY_ENUM, Fields.TryGetEnumArray);
        
        [Test]
        public void TryGetFilePathArray_GetExpectedValue() => AssertExpectedValues<string[]>(FixtureConsts.ARRAY_FILE_PATH, Fields.TryGetFilePathArray);
        
        [Test]
        public void TryGetTileArray_GetExpectedValue() => AssertExpectedValues<Sprite[]>(FixtureConsts.ARRAY_TILE, Fields.TryGetTileArray);
        
        [Test]
        public void TryGetEntityRefArray_GetExpectedValue() => AssertExpectedValues<LDtkIid[]>(FixtureConsts.ARRAY_ENTITY_REF, (string identifier, out LDtkIid[] values) =>
        {
            bool found = Fields.TryGetEntityReferenceArray(identifier, out var outValues);
            values = outValues.Select(p => p.FindEntity()).ToArray();
            return found;
        });
        
        [Test]
        public void TryGetPointArray_GetExpectedValue() => AssertExpectedValues<Vector2[]>(FixtureConsts.ARRAY_POINT, Fields.TryGetPointArray);




        #endregion
    }
}