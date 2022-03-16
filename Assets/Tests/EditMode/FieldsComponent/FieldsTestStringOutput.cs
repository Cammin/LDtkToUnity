using LDtkUnity;
using LDtkUnity.Tests;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class FieldsTestStringOutput : FieldsTestBase
    {
        [Test]
        public void LogEachValueAsString()
        {
            LDtkFields fields = Fields;
            
            foreach (string identifier in FixtureConsts.Singles)
            {
                Debug.Log($"GetValueAsString ({identifier}:{fields.GetValueAsString(identifier)})");
            }
            
            foreach (string identifier in FixtureConsts.Singles)
            {
                if (fields.TryGetValueAsString(identifier, out string value))
                {
                    Debug.Log($"TryGetValueAsString ({identifier}:{value})");
                }
                else
                {
                    Debug.LogError($"issue getting values as string for single value {identifier}");
                }
            }
            
            foreach (string identifier in FixtureConsts.Arrays)
            {
                string joined = string.Join(", ", fields.GetValuesAsStrings(identifier));
                Debug.Log($"GetValuesAsStrings ({identifier}:{joined})");
            }
            
            foreach (string identifier in FixtureConsts.Arrays)
            {
                Assert.True(fields.ContainsField(identifier));
                
                if (fields.TryGetValuesAsStrings(identifier, out string[] strings))
                {
                    Debug.Log($"TryGetValuesAsStrings ({identifier}:{string.Join(", ", strings)})");
                }
                else
                {
                    Debug.LogError($"issue getting values as strings for array value {identifier}");
                }
            }
        }
    }
}