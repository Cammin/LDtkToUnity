using System;
using System.Diagnostics;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkStopwatchScope : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _name;

        public LDtkStopwatchScope(string name)
        {
            _name = name;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
        }

        public override string ToString()
        {
            return $"{_name}:{_stopwatch.ElapsedMilliseconds}ms";
        }
    }
}