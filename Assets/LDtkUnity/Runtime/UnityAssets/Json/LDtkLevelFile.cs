using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// The imported level file. (.ldtkl)<br/>
    /// Available when saving levels as separate files in LDtk. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    public class LDtkLevelFile : LDtkJsonFile<Level>
    {
        public override Level FromJson => Level.FromJson(_json);
    }
}