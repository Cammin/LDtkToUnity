#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity
{
    internal sealed class LDtkDebugInstance
    {
        private readonly HashSet<string> _importMessages = new HashSet<string>();
        private readonly AssetImportContext _ctx;

        public LDtkDebugInstance(AssetImportContext ctx)
        {
            _ctx = ctx;
        }

        public void LogError(string msg, Object obj = null)
        {
            if (!ShouldBlockImport(msg))
            {
                _ctx.LogImportError(LDtkDebug.Format(msg) + '\n' + StackTraceUtility.ExtractStackTrace(), obj);
            }
        }
        
        public void LogWarning(string msg, Object obj = null)
        {
            if (!ShouldBlockImport(msg))
            {
                _ctx.LogImportWarning(LDtkDebug.Format(msg) + '\n' + StackTraceUtility.ExtractStackTrace(), obj);
            }
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
}
#endif
