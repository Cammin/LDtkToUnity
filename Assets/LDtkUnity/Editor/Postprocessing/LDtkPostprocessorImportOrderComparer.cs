using System.Collections.Generic;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkPostprocessorImportOrderComparer : IComparer<LDtkPostprocessor>
    {
        public int Compare(LDtkPostprocessor xo, LDtkPostprocessor yo)
        {
            int x = xo.GetPostprocessOrder();
            int y = yo.GetPostprocessOrder();
            return x.CompareTo(y);
        }
    }
}