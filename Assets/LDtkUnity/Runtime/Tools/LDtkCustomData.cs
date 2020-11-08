using System;

namespace LDtkUnity.Runtime.Tools
{
    [Serializable]
    public struct Bool2 : IEquatable<Bool2>, IFormattable
    {
        public bool x;
        public bool y;

        public Bool2(bool x, bool y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Bool2 other) => x == other.x && y == other.y;
    
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"({x.ToString(formatProvider)}, {y.ToString(formatProvider)})";
        }
    }
    
}