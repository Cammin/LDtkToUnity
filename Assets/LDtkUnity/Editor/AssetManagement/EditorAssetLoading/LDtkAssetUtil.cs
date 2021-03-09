using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkAssetUtil
    {
        public static void WriteText(string path, string content)
        {
            using StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.Write(content);
        }
    }
}