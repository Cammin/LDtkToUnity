using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    public class LDtkLevelFile : LDtkJsonFile<Level>
    {
        public override Level FromJson => Level.FromJson(_json);
    }
}