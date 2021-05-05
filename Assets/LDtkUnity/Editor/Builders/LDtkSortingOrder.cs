namespace LDtkUnity.Editor
{
    public class LDtkSortingOrder
    {
        public int SortingOrderValue { get; private set; } = 0;

        public void Next()
        {
            SortingOrderValue--;
        }
    }
}