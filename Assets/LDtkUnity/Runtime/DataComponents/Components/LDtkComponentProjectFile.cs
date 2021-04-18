using UnityEngine;

namespace LDtkUnity
{
    public class LDtkComponentProjectFile : LDtkDataComponent<LdtkJson>
    {
        protected override LdtkJson DeserializeJson()
        {
            return LdtkJson.FromJson(_json);
        }

        protected override string SerializeJson(LdtkJson data)
        {
            return data.ToJson();
        }
    }
}
