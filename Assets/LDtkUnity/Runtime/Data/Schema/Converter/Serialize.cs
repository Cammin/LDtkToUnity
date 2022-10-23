using System.Text.Json;

namespace LDtkUnity
{
    public static class Serialize
    {
        public static string ToJson(this LdtkJson self) => JsonSerializer.Serialize(self, LDtkUnity.Converter.Settings);
    }
}