using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity
{
    //because logging the same message hundreds of times is very slow, we'll limit the max of the same log up to a certain amount
    
    internal static class LDtkDebug
    {
        private const int MAX_LOGS = 50;
        
        private static readonly Dictionary<string, int> Messages = new Dictionary<string, int>();
        private static bool _dueToResetThisFrame;
        
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

        public static void Assert(bool condition, string msg = "Assertion failed", Object context = null)
        {
            if (ShouldBlock(msg)) return;
            Debug.Assert(condition, Format(msg), context);
        }

        private static bool ShouldBlock(string msg)
        {
#if UNITY_EDITOR
            if (!_dueToResetThisFrame)
            {
                EditorApplication.delayCall += Flush;
                void Flush()
                {
                    Messages.Clear();
                    _dueToResetThisFrame = false;
                }
                _dueToResetThisFrame = true;
            }
            
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
#else
            //in builds, we always want to log everything
            return false;            
#endif
        }


        public static string Format(string msg)
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

        public static void LogError(string msg, LDtkDebugInstance debug, Object context = null)
        {
            if (debug != null)
            {
                debug.LogError(msg, context);
                return;
            }
            LogError($"Unhandled import error: {msg}", context);
        }
        public static void LogWarning(string msg, LDtkDebugInstance debug, Object context = null)
        {
            if (debug != null)
            {
                debug.LogWarning(msg, context);
                return;
            }
            LogWarning($"Unhandled import warning: {msg}", context);
        }
#endif
    }
}