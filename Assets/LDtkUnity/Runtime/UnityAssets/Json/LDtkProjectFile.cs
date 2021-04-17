using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.JSON_PROJECT)]
    public class LDtkProjectFile : LDtkJsonComponent<LdtkJson>
    {
        public override LdtkJson FromJson => LdtkJson.FromJson(_json);
    }
}