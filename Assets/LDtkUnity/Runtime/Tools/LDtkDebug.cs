using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity
{
    internal static class LDtkDebug
    {
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
        
        public static void LogError(string msg, Object context = null)
        {
            if (ShouldBlock(msg)) return;
            Debug.LogError(Format(msg), context);
        }

        private static bool ShouldBlock(string msg)
        {
            if (!Messages.ContainsKey(msg))
            {
                Messages.Add(msg, 1);
            }

            if (Messages[msg] > 50)
            {
                return true;
            }

            Messages[msg]++;
            return false;
        }
        
        private static string Format(string msg)
        {
#if UNITY_EDITOR
            return $"<color={(UnityEditor.EditorGUIUtility.isProSkin ? "#FFCC00" : "#997A00")}>LDtk:</color> {msg}";
#else
            return msg";
#endif
        }
    }
}