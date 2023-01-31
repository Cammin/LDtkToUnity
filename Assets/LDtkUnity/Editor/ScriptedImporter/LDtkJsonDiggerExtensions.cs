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
        public static bool CanRead(this ref JsonReader reader)
        {
            return reader.GetCurrentJsonToken() != JsonToken.None;
        }
        public static bool ReadIsPropertyName(this ref JsonReader reader, string propertyName)
        {
            return reader.GetCurrentJsonToken() == JsonToken.String && 
                   reader.ReadString() == propertyName && 
                   reader.ReadIsNameSeparator();
        }
        public static bool IsInArray(this ref JsonReader reader, ref int depth)
        {
            /*if (reader.GetCurrentJsonToken() == Utf8Json.JsonToken.BeginArray)
            {
                depth++;
                reader.ReadNext();
                return true;
            }
            if (reader.GetCurrentJsonToken() == Utf8Json.JsonToken.EndArray)
            {
                depth--;
                reader.ReadNext();
                return true;
            }
            if (depth <= 0)
            {
                return false; //there's only one instance of the tilesets array in the definitions; we can return after we leave the depth of the tilesets 
            }*/
            
            if (reader.ReadIsBeginArray())
            {
                depth++;
                //Debug.Log($"array depth++ into {depth}");
            }

            if (reader.ReadIsEndArray())
            {
                depth--;
                //Debug.Log($"array depth-- into {depth}");
            }

            return depth > 0;
        }
        public static bool IsInObject(this ref JsonReader reader, ref int depth)
        {
            if (reader.ReadIsBeginObject())
            {
                depth++;
                Debug.Log($"object depth++ into {depth}");
                return true;
            }

            if (reader.ReadIsEndObject())
            {
                depth--;
                Debug.Log($"object depth++ into {depth}");
            }

            if (depth <= 0)
            {
                return false;
            }

            return true;
        }
    }
}