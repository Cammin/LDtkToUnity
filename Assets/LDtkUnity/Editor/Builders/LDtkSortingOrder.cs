namespace LDtkUnity.Editor
{
    internal sealed class LDtkSortingOrder
    {
        public int SortingOrderValue { get; private set; } = 0;

        public void Next()
        {
            SortingOrderValue--;
        }
    }
}