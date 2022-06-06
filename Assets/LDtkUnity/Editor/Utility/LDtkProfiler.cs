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
            string fullPath = $"{directory}/{path}";
            string directoryName = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryName);
            File.WriteAllText($"{directory}/.gitignore", "*");
            Profiler.logFile = fullPath;
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