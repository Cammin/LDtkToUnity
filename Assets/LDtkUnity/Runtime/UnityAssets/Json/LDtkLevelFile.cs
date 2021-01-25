using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    public class LDtkLevelFile : LDtkJsonFile<Level>
    {
        public override Level FromJson => Level.FromJson(_json);
    }
}