using System.Collections.Generic;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity
{
    //because logging the same message hundreds of times is very slow, we'll limit the max of the same log up to a certain amount
    internal static class LDtkDebug
    {
        private const int MAX_LOGS = 50;
        
        private static readonly Dictionary<string, int> Messages = new Dictionary<string, int>();
        
        public static void Log(string msg, Object context = null)
        {
            if (ShouldBlock(msg)) return;
            Debug.Log(Format(msg), context);
        }
        
        public static void LogWarning(string msg, Object context = null)
        {
            if (ShouldBlock(msg)) return;
            Debug.LogWarning(Format(msg), context);
        }
        public static void LogWarning(string msg, AssetImportContext ctx, Object obj = null)
        {
            if (ShouldBlock(msg)) return;

            if (ctx != null)
            {
                ctx.LogImportWarning(msg + '\n' + StackTraceUtility.ExtractStackTrace(), obj);
                return;
            }
            Debug.LogWarning(Format(msg), obj);
        }

        public static void LogError(string msg, Object context = null)
        {
            if (ShouldBlock(msg)) return;
            Debug.LogError(Format(msg), context);
        }

        public static void LogError(string msg, AssetImportContext ctx, Object obj = null)
        {
            if (ShouldBlock(msg)) return;

            if (ctx != null)
            {
                ctx.LogImportError(msg + '\n' + StackTraceUtility.ExtractStackTrace(), obj);
            }
            else
            {
                Debug.LogError(Format(msg), obj);
            }
        }
        
        public static void Assert(bool condition, string msg = "Assertion failed", Object context = null)
        {
            if (ShouldBlock(msg)) return;
            Debug.Assert(condition, Format(msg), context);
        }

        private static bool ShouldBlock(string msg)
        {
            if (!Messages.ContainsKey(msg))
            {
                Messages.Add(msg, 1);
            }

            if (Messages[msg] > MAX_LOGS)
            {
                return true;
            }

            Messages[msg]++;
            return false;
        }
        
        private static string Format(string msg)
        {
#if UNITY_EDITOR
            return $"<color={GetStringColor()}>LDtk:</color> {msg}";
#else
            return msg;
#endif
        }

#if UNITY_EDITOR
        public static string GetStringColor()
        {
            return UnityEditor.EditorGUIUtility.isProSkin ? "#FFCC00" : "#997A00";
        } 
#endif
    }
}