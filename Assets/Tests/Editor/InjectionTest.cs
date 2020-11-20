using System;
using LDtkUnity.Runtime.FieldInjection;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class InjectionTest
    {
        [Test]
        public void EnumInjectionParseTest()
        {
            string type = "LocalEnum.ForceMode";
            string value = "ForceMode.Acceleration";
            
            
            Type typeLDtk = LDtkFieldParser.GetParsedFieldType(type);
            Debug.Log(typeLDtk);
            
            object o = LDtkFieldInjector.GetParsedValue(typeLDtk, value);
            Debug.Log(o);
        }
    }
}
