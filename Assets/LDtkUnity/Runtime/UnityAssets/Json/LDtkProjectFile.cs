using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// The imported level file, available in the the import result. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.JSON_PROJECT)]
    public class LDtkProjectFile : LDtkJsonFile<LdtkJson>
    {
        public override LdtkJson FromJson => LdtkJson.FromJson(_json);
    }
}