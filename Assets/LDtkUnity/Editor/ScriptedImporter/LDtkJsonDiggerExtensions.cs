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
        public static void ReadUntilPropertyName(this ref JsonReader reader, string propertyName)
        {
            while (true)
            {
                if (reader.ReadIsPropertyName(propertyName))
                {
                    return;
                }
                
                if (!reader.Read())
                {
                    Debug.LogError("Reached JsonToken.None territory");
                    return;
                }
            }
        }
        public static bool IsInArray(this ref JsonReader reader, ref int depth)
        {
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
        public static void ReadToArrayEnd(this ref JsonReader reader, int depth)
        {
            while (true)
            {
                if (reader.ReadIsBeginArray())
                {
                    depth++;
                }

                if (reader.ReadIsEndArray())
                {
                    depth--;
                }

                if (depth <= 0)
                {
                    return;
                }

                if (!reader.Read())
                {
                    return;
                }
            }
        }
        public static bool IsInObject(this ref JsonReader reader, ref int depth)
        {
            if (reader.ReadIsBeginObject())
            {
                depth++;
                //Debug.Log($"object depth++ into {depth}");
            }

            if (reader.ReadIsEndObject())
            {
                depth--;
                //Debug.Log($"object depth++ into {depth}");
            }

            return depth > 0;
        }
        public static void ReadToObjectEnd(this ref JsonReader reader, int depth)
        {
            while (true)
            {
                if (reader.ReadIsBeginObject())
                {
                    depth++;
                }

                if (reader.ReadIsEndObject())
                {
                    depth--;
                }

                if (depth <= 0)
                {
                    return;
                }

                if (!reader.Read())
                {
                    return;
                }
            }
        }
    }
}