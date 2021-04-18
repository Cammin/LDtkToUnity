using UnityEngine;

namespace LDtkUnity
{
    public class LDtkComponentLevelFile : LDtkDataComponent<Level>
    {
        protected override Level DeserializeJson()
        {
            return Level.FromJson(_json);
        }

        protected override string SerializeJson(Level data)
        {
            throw new System.NotImplementedException();
        }
    }
}
