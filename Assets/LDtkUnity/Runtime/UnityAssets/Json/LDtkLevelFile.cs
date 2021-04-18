using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    public class LDtkLevelFile : LDtkJsonComponent<Level>
    {
        public override Level FromJson => Level.FromJson(_json);
    }
}