using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderEnum
    {
        private static List<Type> _cachedTypes;
        
#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void Dispose()
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
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (TypeInfo type in assembly.DefinedTypes)
                {
                    if (type.IsArray)
                    {
                        Type element = type.GetElementType();

                        if (IsValid(element, enumName))
                        {
                            CacheType(element);
                            return element;
                        }

                        continue;
                    }
                    
                    if (IsValid(type, enumName))
                    {
                        CacheType(type);
                        return type;
                    }
                }
            }
            
            Debug.LogError($"LDtk: Was unable to get Enum type: {enumName}");
            return null;
        }

        public static void Init()
        {
            _cachedTypes = new List<Type>();
        }

        private static void CacheType(Type type)
        {
            if (Application.isPlaying)
            {
                _cachedTypes.Add(type);  
            }
        }
        
        
        private static bool IsValid(Type t, string enumName)
        {
            return t.IsEnum && t.Name == enumName;
        }
    }
}