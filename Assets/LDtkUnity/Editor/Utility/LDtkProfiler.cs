using System;
using System.IO;
using UnityEngine;
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
            string directory = $"{Path.GetDirectoryName(Application.dataPath)}/Profiler";
            Directory.CreateDirectory(directory);
            Profiler.logFile = $"{directory}/{path}";
            Profiler.enableBinaryLog = true;
            Profiler.enabled = true;
            Profiler.BeginSample(path);
        }
        public static void EndSample()
        {
            try
            {
                Profiler.EndSample();
                LDtkDebug.Log($"Wrote profiler import details to \"{Profiler.logFile}\"");
                Profiler.enabled = false;
                Profiler.logFile = "";
            }
            catch (Exception e)
            {
                LDtkDebug.LogError($"Failed to end a profiler sample for \"{Profiler.logFile}\". Reason: {e}");
            }
        }
    }
}