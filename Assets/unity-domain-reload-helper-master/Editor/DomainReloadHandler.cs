using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class DomainReloadHandler
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void OnRuntimeLoad()
    {
        int clearedValues = 0;
        int executedMethods = 0;

        /* Clear on reload */
        foreach(MemberInfo member in GetMembers<ClearOnReloadAttribute>(true))
        {
            // Fields
            FieldInfo field = member as FieldInfo;

            if (field != null && !field.FieldType.IsGenericParameter && field.IsStatic)
            {
                Type fieldType = field.FieldType;
                
                // Extract attribute and access its parameters
                var reloadAttribute = field.GetCustomAttribute<ClearOnReloadAttribute>();
                object valueToAssign = reloadAttribute?.valueToAssign;
                bool assignNewTypeInstance = reloadAttribute != null && reloadAttribute.assignNewTypeInstance;

                // Use valueToAssign only if it's convertible to the field value type
                dynamic value = valueToAssign != null 
                                ? Convert.ChangeType(valueToAssign, fieldType) 
                                : null;
                
                // If assignNewTypeInstance is set, create a new instance of this type and assign it to the field
                if (assignNewTypeInstance) value = Activator.CreateInstance(fieldType);
                
                try { field.SetValue(null, value); }
                catch { }

                clearedValues++;
            }

            // Properties
            PropertyInfo property = member as PropertyInfo;

            if (property != null && !property.PropertyType.IsGenericParameter && property.GetAccessors(true).Any(x => x.IsStatic))
            {
                Type fieldType = property.PropertyType;
                
                // Extract attribute and access its parameters
                var reloadAttribute = property.GetCustomAttribute<ClearOnReloadAttribute>();
                object valueToAssign = reloadAttribute?.valueToAssign;
                bool assignNewTypeInstance = reloadAttribute != null && reloadAttribute.assignNewTypeInstance;

                // Use valueToAssign only if it's convertible to the property value type
                dynamic value = valueToAssign != null 
                    ? Convert.ChangeType(valueToAssign, fieldType) 
                    : null;
                
                // If assignNewTypeInstance is set, create a new instance of this type and assign it to the property
                if (assignNewTypeInstance) value = Activator.CreateInstance(fieldType);

                try { property.SetValue(null, value); }
                catch {  }

                clearedValues++;
            }
        }

        /* Execute on reload */
        foreach(MemberInfo member in GetMethodMembers<ExecuteOnReloadAttribute>(true))
        {
            MethodInfo method = member as MethodInfo;

            if (method != null && !method.IsGenericMethod && method.IsStatic)
            {
                method.Invoke(null, new object[] { });
                executedMethods++;
            }
        }

        Debug.Log($"Cleared {clearedValues} members, executed {executedMethods} methods");
    }

    private static IEnumerable<MemberInfo> GetMethodMembers<TAttribute>(bool inherit)
                                 where TAttribute : System.Attribute
    {
        List<MemberInfo> members = new List<MemberInfo>();

        BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                //Methods
                members.AddRange(from t in a.GetTypes()
                                 where t.IsClass
                                 where !t.IsGenericParameter
                                 from m in t.GetMethods(flags)
                                 where !m.ContainsGenericParameters
                                 where m.IsDefined(typeof(TAttribute), inherit)
                                 select m);
            }
            catch (System.Reflection.ReflectionTypeLoadException)
            {
                continue;
            }
        }

        return members;

    }
    
    private static IEnumerable<MemberInfo> GetMembers<TAttribute>(bool inherit)
                                where TAttribute : System.Attribute
    {
        List<MemberInfo> members = new List<MemberInfo>();

        BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                foreach(Type t in a.GetTypes())
                {
                    if(!t.IsClass)
                        continue;

                    //Fields
                    foreach (MemberInfo member in t.GetFields(flags))
                    {
                        if (member.IsDefined(typeof(TAttribute), inherit))
                            members.Add(member);
                    }

                    //Properties
                    foreach (MemberInfo member in t.GetProperties(flags))
                    {
                        if (member.IsDefined(typeof(TAttribute), inherit))
                            members.Add(member);
                    }

                    //Events
                    foreach (EventInfo eventInfo in t.GetEvents(flags))
                    {
                        if (eventInfo.IsDefined(typeof(TAttribute), inherit))
                            members.Add(GetEventField(t,eventInfo.Name));
                    }
                }

            }
            catch (System.Reflection.ReflectionTypeLoadException)
            {
                continue;
            }

        }

        return members;
    }

    private static FieldInfo GetEventField(Type type, string eventName)
    {
        FieldInfo field = null;
        while (type != null)
        {
            /* Find events defined as field */
            field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                break;

            /* Find events defined as property { add; remove; } */
            field = type.GetField($"EVENT_{eventName.ToUpper()}", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
                break;
            type = type.BaseType;
        }
        return field;
    }

}
