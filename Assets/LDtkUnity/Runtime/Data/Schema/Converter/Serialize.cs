using Utf8Json;

namespace LDtkUnity
{
    public static class Serialize
    {
        public static byte[] ToJson(this LdtkJson self) => JsonSerializer.Serialize(self);
    }
}