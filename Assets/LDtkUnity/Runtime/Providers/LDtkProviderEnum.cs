using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderEnum
    {
        private static Dictionary<string, Type> _cachedTypes;
        
#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void Dispose()
        {
            _cachedTypes = null;
        }
        public static void Init()
        {
            _cachedTypes = GetAllLDtkEnums();

            foreach (KeyValuePair<string,Type> kvp in _cachedTypes)
            {
                string logged = $"({kvp.Key}, {kvp.Value.Name})";
                Debug.Log(logged);
            }
            
        }
        
        public static Type GetEnumType(string ldtkEnumName)
        {
            if (_cachedTypes.ContainsKey(ldtkEnumName))
            {
                return _cachedTypes[ldtkEnumName];
            }
            
            /*foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (TypeInfo type in assembly.DefinedTypes)
                {
                    if (type.IsArray)
                    {
                        Type element = type.GetElementType();

                        if (IsValid(element, ldtkEnumName))
                        {
                            CacheType(element);
                            return element;
                        }

                        continue;
                    }
                    
                    if (IsValid(type, ldtkEnumName))
                    {
                        CacheType(type);
                        return type;
                    }
                }
            }*/
            
            Debug.LogError($"LDtk: Was unable to get Enum type from trying to use enum identifier: {ldtkEnumName}");
            return null;
        }

        public static Dictionary<string, Type> GetAllLDtkEnums()
        {
            Dictionary<string, Type> ldtkEnumTypes = new Dictionary<string, Type>();
            string assemblyDefinedIn = typeof(LDtkEnumAttribute).Assembly.GetName().Name;
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if ((assembly.GlobalAssemblyCache || assembly.GetName().Name != assemblyDefinedIn) &&
                    assembly.GetReferencedAssemblies().All(a => a.Name != assemblyDefinedIn))
                {
                    continue;
                }
                
                foreach (Type type in assembly.GetTypes())
                {
                    LDtkEnumAttribute[] customAttributes = type.GetCustomAttributes<LDtkEnumAttribute>(true).ToArray();
                    if (!customAttributes.Any())
                    {
                        continue;
                    }
                    
                    LDtkEnumAttribute customAttribute = customAttributes.ToArray()[0];
                    string typeName = customAttribute.IsCustomDefinedName ? customAttribute.EnumIdentifier : type.Name;

                    if (ldtkEnumTypes.ContainsKey(typeName))
                    {
                        Debug.LogError($"LDtk: Duplicate [LDtkEnum] name for \"{typeName}\", ensure there are no conflicts with enum names");
                        continue;
                    }
                    
                    ldtkEnumTypes.Add(typeName, type);                    
                }
            }

            return ldtkEnumTypes;
        }
    }
}