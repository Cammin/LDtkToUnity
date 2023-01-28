using UnityEngine;
using Utf8Json;

namespace LDtkUnity.Editor
{
    internal static class LDtkJsonDiggerExtensions
    {
        public static bool Read(this ref JsonReader reader)
        {
            reader.ReadNext();

            //if the end was reached
            if (reader.GetCurrentJsonToken() == JsonToken.None)
            {
                return false;
            }
            return true;
        }
        public static bool ReadIsPropertyName(this ref JsonReader reader, string propertyName)
        {
            return 
                reader.GetCurrentJsonToken() == JsonToken.String && 
                reader.ReadString() == propertyName && 
                reader.ReadIsNameSeparator();
        }
        public static bool IsInArray(this ref JsonReader reader, ref int depth)
        {
            if (reader.ReadIsBeginArray())
            {
                Debug.Log("depth++");
                depth++;
            }

            if (reader.ReadIsEndArray())
            {
                Debug.Log("depth--");
                depth--;
            }

            if (depth <= 0)
            {
                return false;
            }

            return true;
        }
    }
}