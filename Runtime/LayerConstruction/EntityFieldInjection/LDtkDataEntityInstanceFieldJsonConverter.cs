using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection
{
    public class LDtkDataEntityInstanceFieldJsonConverter : JsonConverter<string[]>
    {
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, string[] value, JsonSerializer serializer) { }
        public override string[] ReadJson(JsonReader reader, Type objectType, string[] existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            
            //Debug.Log(token.Type);
            //Debug.Log( $"[{string.Join("], [", tokens)}]");

            return GetTokens(token);
        }

        private static string[] GetTokens(JToken token)
        {
            List<string> tokens = new List<string>();
            
            switch (token)
            {
                case JValue value:
                    //Debug.Log($"Is JValue");
                    string objString = value.ToObject<string>();
                    if (string.IsNullOrEmpty(objString)) objString = string.Empty;
                    tokens.Add(objString);
                    break;
                    
                case JObject obj:
                    //Debug.Log($"Is JObject");
                    List<string> items = new List<string>();
                    foreach (JToken propertyValue in obj.PropertyValues())
                    {
                        string newEntry = string.Join(", ", GetTokens(propertyValue));
                        items.Add(newEntry);
                    }
                    tokens.Add(string.Join(", ", items));
                    break;

                case JArray array:
                
                    //Debug.Log($"Is JArray");
                    foreach (JToken child in array.Children())
                    {
                        string newEntry = string.Join(", ", GetTokens(child));
                        tokens.Add(newEntry);
                    }
                    break;
                
                default:
                    return default;
                
                
            }

            return tokens.ToArray();
        }
    }
}