using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    public class LDtkStopwatchScope : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private string _name;

        public LDtkStopwatchScope(string name)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Debug.Log($"{_name}:{_stopwatch.ElapsedMilliseconds}ms");
        }
    }
}