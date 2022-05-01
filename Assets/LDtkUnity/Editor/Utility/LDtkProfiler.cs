using System;
using System.IO;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    public static class LDtkProfiler
    {
        public class Scope : IDisposable
        {
            public Scope(string path) => BeginSample(path);
            public void Dispose() => EndSample();
        }
        
        public static void BeginSample(string path)
        {
            Directory.CreateDirectory("Profiler");
            Profiler.logFile = $"Profiler/{path}";
            Profiler.enableBinaryLog = true;
            Profiler.enabled = true;
            Profiler.BeginSample(path);
        }
        public static void EndSample()
        {
            Profiler.EndSample();
            Profiler.enabled = false;
            Profiler.logFile = "";
        }
    }
}