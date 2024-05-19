using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity
{
    internal static class LDtkProfiler
    {
        //don't need to worry about statically resetting this
        private static bool _prevProfilingState;
        
        internal sealed class Scope : IDisposable
        {
            public Scope(string path) => BeginWriting(path);
            public void Dispose() => EndWriting();
        }
        
        [Conditional("LDTK_ENABLE_PROFILER")]
        public static void BeginSample(string name)
        {
            Profiler.BeginSample(name);
        }
    
        [Conditional("LDTK_ENABLE_PROFILER")]
        public static void EndSample()
        {
            Profiler.EndSample();
        }
        
        //[Conditional("LDTK_ENABLE_PROFILER")]
        public static void BeginWriting(string path)
        {
            string directory = $"{Path.GetDirectoryName(Application.dataPath)}/Profiler";
            string fullPath = $"{directory}/{path}";
            string directoryName = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryName);
            File.WriteAllText($"{directory}/.gitignore", "*");
            Profiler.logFile = fullPath;
            Profiler.enableBinaryLog = true;
            _prevProfilingState = Profiler.enabled;
            Profiler.enabled = true;
            Profiler.BeginSample(path);
        }
        
        //[Conditional("LDTK_ENABLE_PROFILER")]
        public static void EndWriting()
        {
            try
            {
                Profiler.EndSample();
                LDtkDebug.Log($"Wrote profiler import details to \"{Profiler.logFile}\"");
                Profiler.enabled = _prevProfilingState;
                Profiler.logFile = "";
            }
            catch (Exception e)
            {
                LDtkDebug.LogError($"Failed to end a profiler sample for \"{Profiler.logFile}\". Reason: {e}");
            }
        }
    }
}
