using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// responsible for holding onto entity references.
    /// When trying to access from cref LDtkFields for the first time, they will automatically be loaded. howeever, you can also cache it yourself when you'd like.
    /// </summary>
    public class LDtkEntityReferenceBank
    {
        private static Dictionary<int, GameObject> _IidObjects;
        
        //todo set up runtime automatic references when needed.
    }
}