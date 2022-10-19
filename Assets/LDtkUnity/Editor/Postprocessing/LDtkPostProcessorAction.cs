using System;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkPostProcessorAction : IComparable<LDtkPostProcessorAction>
    {
        public int Order = 0;
        public Action Action;
        public string DebugInfo;

        public int CompareTo(LDtkPostProcessorAction other)
        {
            return Order.CompareTo(other.Order);
        }

        public override string ToString()
        {
            return $"{Order}\t{DebugInfo}";
        }
    }
}