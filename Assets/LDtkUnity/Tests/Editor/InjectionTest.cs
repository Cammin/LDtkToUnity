using System;
using LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection;
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
            
            
            Type typeLDtk = LDtkEntityInstanceFieldParser.ParseFieldType(type);
            Debug.Log(typeLDtk);
            
            object o = LDtkEntityInstanceFieldInjector.GetValue(typeLDtk, value);
            Debug.Log(o);
        }
    }
}
