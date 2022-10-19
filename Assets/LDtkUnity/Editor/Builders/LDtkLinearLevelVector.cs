namespace LDtkUnity.Editor
{
    internal sealed class LDtkLinearLevelVector
    {
        private const int SPACED_PIXELS = 48;

        public int Scaler { get; private set; } = 0;
        
        public void Next(int lvlPx)
        {
            int newValue = Scaler + lvlPx + SPACED_PIXELS;
            Scaler = newValue;
        }
    }
}