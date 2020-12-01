using System.IO;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler
{
    public static class LDtkAsmRefFactory
    {
        private const string ASM_REF_TEMPLATE_PATH = "LDtkUnity/Editor/AssetWindow/EnumHandler/AsmRefTemplate.txt";
        private const string ASM_REF_NAME = "LDtkUnity.Runtime";
        
        public static void CreateAssemblyDefinitionReference(string folderPath)
        {
            string asmRefPath = folderPath + ASM_REF_NAME + ".asmref";
            string asmrefContents = LDtkInternalLoader.Load<TextAsset>(ASM_REF_TEMPLATE_PATH).text;
            
            using (StreamWriter streamWriter = new StreamWriter(asmRefPath))
            {
                streamWriter.Write(asmrefContents);
            }
        }
    }
}