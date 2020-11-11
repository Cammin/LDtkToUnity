using System;
using LDtkUnity.Runtime.FieldInjection;
using NUnit.Framework;
using UnityEngine;

namespace LDtkUnity.Tests.Editor
{
    public class InjectionTest
    {
        [Test]
        public void EnumInjectionParseTest()
        {
            string type = "LocalEnum.ForceMode";
            string value = "ForceMode.Acceleration";
            
            
            Type typeLDtk = LDtkFieldParser.ParseFieldType(type);
            Debug.Log(typeLDtk);
            
            object o = LDtkFieldInjector.GetValue(typeLDtk, value);
            Debug.Log(o);
        }
    }
}
