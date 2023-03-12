using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //This class could be useful to set at the main asset if things go wrong.
    internal class FailedImportObject : ScriptableObject
    {
        public List<ImportInfo> Messages = new List<ImportInfo>();
    }

    [Serializable]
    public class ImportInfo
    {
        public string Message;
        public MessageType Type;
    }
}