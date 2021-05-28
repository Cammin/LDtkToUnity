using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// The imported level file, available in the the import result. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.JSON_PROJECT)]
    public class LDtkProjectFile : LDtkJsonFile<LdtkJson>
    {
        /// <value>
        /// Gets the deserialized project root.
        /// </value>
        public override LdtkJson FromJson => LdtkJson.FromJson(_json);
    }
}