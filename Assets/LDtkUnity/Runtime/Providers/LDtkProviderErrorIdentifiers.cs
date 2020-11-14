using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Runtime.Providers
{
    public static class LDtkProviderErrorIdentifiers
    {
        private static HashSet<string> PreviouslyFailedIdentifierGets { get; set; }
        
        public static void Init()
        {
            PreviouslyFailedIdentifierGets = new HashSet<string>();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Dispose()
        {
            PreviouslyFailedIdentifierGets = null;
        }

        public static bool Contains(string identifier)
        {
            return PreviouslyFailedIdentifierGets.Contains(identifier);
        }
        public static bool Add(string identifier)
        {
            if (Contains(identifier))
            {
                Debug.LogError("LDtk: Already contains identifier");
            }
            
            return PreviouslyFailedIdentifierGets.Add(identifier);
        }
    }
}