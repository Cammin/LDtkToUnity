using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkEnumGetter
    {
        private static List<Type> _cachedTypes;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _cachedTypes = null;
        }
        
        public static Type GetEnumType(string enumName)
        {
            bool ContainedCachedValue(Type cachedType)
            {
                return cachedType.Name == enumName;
            }
            if (!_cachedTypes.NullOrEmpty() && _cachedTypes.Any(ContainedCachedValue))
            {
                return _cachedTypes.First(ContainedCachedValue);
            }

            //TODO this is currently only optimized using a personal technique/ maybe make this string settable from LEd scriptable object settings?
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("Cam")))
            {
                foreach (TypeInfo type in assembly.DefinedTypes)
                {
                    if (type.IsArray)
                    {
                        Type element = type.GetElementType();

                        if (IsValid(element, enumName))
                        {
                            _cachedTypes.Add(element);
                            return element;
                        }

                        continue;
                    }
                    
                    if (IsValid(type, enumName))
                    {
                        _cachedTypes.Add(type);
                        return type;
                    }
                }
            }
            
            Debug.LogError($"Was unable to get Enum type: {enumName}");
            return null;
        }

        public static void Init()
        {
            _cachedTypes = new List<Type>();
        }
        public static void Dispose()
        {
            _cachedTypes.Clear();
        }
        
        private static bool IsValid(Type t, string enumName)
        {
            return t.IsEnum && t.Name == enumName;
        }
    }
}