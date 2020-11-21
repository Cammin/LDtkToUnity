using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LDtkUnity.Runtime.FieldInjection;
using LDtkUnity.Runtime.Providers;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

namespace Tests.Editor
{
    public class EnumGetPerformanceTest
    {
        [Test]
        public void GetEnumsInLinq()
        {
            Stopwatch clock = Stopwatch.StartNew();
            
            IEnumerable<Type> ldtkEnumTypes = GetTypesWithAttribute<LDtkEnumAttribute>();
            
            List<string> enums = ldtkEnumTypes
                .SelectMany(p => p.GetCustomAttributes<LDtkEnumAttribute>()
                    .Select(pp => pp.IsCustomDefinedName ? pp.EnumIdentifier : p.Name)).ToList();

            clock.Stop();

            Debug.Log(clock.ElapsedMilliseconds);
            foreach (string s in enums)
            {
                Debug.Log(s);
            }
        }
        
        [Test]
        public void GetEnumsOptimized()
        {
            Stopwatch clock = Stopwatch.StartNew();

            
            Dictionary<string, Type> ldtkEnumTypes = LDtkProviderEnum.GetAllLDtkEnums();
            
            clock.Stop();
            
            Debug.Log(clock.ElapsedMilliseconds);
            foreach (KeyValuePair<string, Type> s in ldtkEnumTypes)
            {
                string logged = $"({s.Key}, {s.Value.Name})";
                Debug.Log(logged);
            }
        }

        IEnumerable<Type> GetTypesWithAttribute<T>(bool inherit = false) where T : Attribute
        { 
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes(), (a, type) => new {a, type})
                .Where(t => t.type.IsDefined(typeof(T), inherit))
                .Select(t => t.type);
        }
        
        private IEnumerable<string> GetTypesAsString()
        {
            return null;
        }
        
        
        [LDtkEnum("Enum1")]
        public enum DatEnum
        {
            v1, v2, v3
        }
        [LDtkEnum()]
        public enum Enum2
        {
            v4, v5, v6
        }
    }
}