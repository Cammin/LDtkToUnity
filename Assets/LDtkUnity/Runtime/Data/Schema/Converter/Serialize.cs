using Newtonsoft.Json;

namespace LDtkUnity
{
    public static class Serialize
    {
        public static string ToJson(this LdtkJson self) => JsonConvert.SerializeObject(self, LDtkUnity.Converter.Settings);
    }
}