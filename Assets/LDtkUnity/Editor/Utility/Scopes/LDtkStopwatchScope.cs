using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    internal class LDtkStopwatchScope : IDisposable
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
            Debug.Log($"{_name}:{_stopwatch.ElapsedMilliseconds}ms");
        }
    }
}