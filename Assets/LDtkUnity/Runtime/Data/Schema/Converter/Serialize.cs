using Newtonsoft.Json;

namespace LDtkUnity.Converter
{
    public static class Serialize
    {
        public static string ToJson(this LdtkJson self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}