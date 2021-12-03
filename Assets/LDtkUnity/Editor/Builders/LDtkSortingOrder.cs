using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkSortingOrder
    {
        public int SortingOrderValue { get; private set; } = 0;

        public void Next()
        {
            SortingOrderValue--;
        }
    }
}