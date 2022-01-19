using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    internal class LDtkPostprocessorImportOrderComparer : IComparer<LDtkPostprocessor>
    {
        public int Compare(LDtkPostprocessor xo, LDtkPostprocessor yo)
        {
            int x = xo.GetPostprocessOrder();
            int y = yo.GetPostprocessOrder();
            return x.CompareTo(y);
        }
    }
}