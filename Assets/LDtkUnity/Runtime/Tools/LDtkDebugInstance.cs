
using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

#endif

namespace LDtkUnity
{
    internal sealed class LDtkDebugInstance
    {
        private readonly HashSet<string> _importMessages = new HashSet<string>();
#if UNITY_EDITOR
        private readonly AssetImportContext _ctx;
        public readonly ImportLogEntries _entries;
        public LDtkDebugInstance(AssetImportContext ctx)
        {
            _ctx = ctx;
            _entries = new ImportLogEntries(_ctx.assetPath);
        }
#endif

        public void LogError(string msg, Object obj = null)
        {
#if UNITY_EDITOR
            if (ShouldBlockImport(msg))
            {
                return;
            }
            
            msg = LDtkDebug.Format(msg) + '\n' + StackTraceUtility.ExtractStackTrace();
            _ctx.LogImportError(msg, obj);
            
    #if !UNITY_2022_2_OR_NEWER
            _entries.Log(msg, ImportLogFlags.Error);
    #endif
#endif
        }
        
        public void LogWarning(string msg, Object obj = null)
        {
#if UNITY_EDITOR
            if (ShouldBlockImport(msg))
            {
                return;
            }
            
            msg = LDtkDebug.Format(msg) + '\n' + StackTraceUtility.ExtractStackTrace();

            _ctx.LogImportWarning(msg, obj);
            
    #if !UNITY_2022_2_OR_NEWER
            _entries.Log(msg, ImportLogFlags.Warning);
    #endif
#endif
        }
        
        private bool ShouldBlockImport(string msg)
        {
            if (_importMessages.Contains(msg))
            {
                return true;
            }
            
            _importMessages.Add(msg);
            return false;
        }
    }
    
    #if UNITY_EDITOR
    
    [Serializable]
    internal sealed class ImportLogEntries
    {
        public List<ImportLogEntry> _entries = new List<ImportLogEntry>();
        [NonSerialized] public string AssetPath;

        public ImportLogEntries(string assetPath)
        {
            AssetPath = assetPath;
        }

        public void Log(string msg, ImportLogFlags flag)
        {
            _entries.Add(new ImportLogEntry()
            {
                _message = msg,
                _flag = flag
            });
        }
        
        [UsedImplicitly]
        public void WriteTheEntries()
        {
            string dir = Dir();
            string path = FilePath();
            
            Directory.CreateDirectory(dir);
            if (_entries.IsNullOrEmpty())
            {
                File.WriteAllText(path, string.Empty);
                return;
            }
            
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }
        
        [UsedImplicitly]
        public void ReadTheEntries()
        {
            string dir = Dir();
            string path = FilePath();

            if (!Directory.Exists(dir))
            {
                return;
            }
            if (!File.Exists(path))
            {
                return;
            }

            string json = File.ReadAllText(path);
            if (json.IsNullOrEmpty())
            {
                return;
            }
            _entries = JsonUtility.FromJson<ImportLogEntries>(json)._entries;
        }

        private string Dir()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "LDtkImportLogs"));
        }
        private string FilePath()
        {
            string guid = AssetDatabase.AssetPathToGUID(AssetPath);
            return Path.Combine(Dir(), $"{Path.GetFileNameWithoutExtension(AssetPath)}_{guid}.txt");
        }
    }
    [Serializable]
    public struct ImportLogEntry
    {
        public string _message;
        public ImportLogFlags _flag;

        public void PrintToConsole(Object ctx = null)
        {
            switch (_flag)
            {
                case ImportLogFlags.Warning:
                    Debug.LogWarning(_message, ctx);
                    break;
                case ImportLogFlags.Error:
                    Debug.LogError(_message, ctx);
                    break;
                default:
                    break;
            }
        }
    }
    
    public enum ImportLogFlags
    {
        Warning = 0,
        Error = 1,
    }
    #endif
}
