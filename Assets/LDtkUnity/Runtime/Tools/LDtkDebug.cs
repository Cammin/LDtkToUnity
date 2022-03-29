using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity
{
    internal static class LDtkDebug
    {
        private static readonly HashSet<string> Messages = new HashSet<string>();

        public static void Reset()
        {
            Messages.Clear();
        }
        
        public static void LogError(string msg, Object context = null)
        {
            if (Messages.Contains(msg))
            {
                return;
            }

            Messages.Add(msg);
            Debug.LogError($"LDtk: {msg}", context);
        }
    }
}