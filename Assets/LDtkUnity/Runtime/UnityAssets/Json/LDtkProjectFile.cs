using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    public class LDtkProjectFile : LDtkJsonFile<LdtkJson>
    {
        public override LdtkJson FromJson => LdtkJson.FromJson(_json);
    }
}