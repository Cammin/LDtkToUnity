using System.Reflection;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkFieldInjectorData
    {
        public readonly FieldInfo Info;
        public readonly string FieldIdentifier;
        public readonly object ObjectRef;

        //field, fieldname, monobehaviour ref
        public LDtkFieldInjectorData(FieldInfo info, string fieldIdentifier, object objectRef)
        {
            Info = info;
            FieldIdentifier = fieldIdentifier;
            ObjectRef = objectRef;
        }

        public void SetField(object data)
        {
            if (data != null && data.GetType() == Info.FieldType)
            {
                Info.SetValue(ObjectRef, data);
                return;
            }

            if (data != null)
            {
                Debug.LogError($"LDtk: Error when setting a field \"{FieldIdentifier}\" of type \"{Info.FieldType.Name}\" called \"{Info.Name}\" in object \"{((Object)ObjectRef).name}\". Type mismatch?", (Object)ObjectRef);
            }
            // in the situation where the object data is null, it was set null able in ldtk and not defined. So don't log anything about it.

        }
    }
}