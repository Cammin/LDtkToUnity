using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkAsmRefFactory
    {
        private const string ASM_REF_TEMPLATE_PATH = LDtkEnumFactoryPath.PATH + "AsmRefTemplate.txt";
        private const string ASM_REF_KEY = "#ASSEMBLY#";
        
        public static void CreateAssemblyDefinitionReference(string folderPath, AssemblyDefinitionAsset asmDef)
        {
            if (asmDef == null)
            {
                //don't make a file if none was inputted
                return;
            }
            
            //todo find out a way to delete the old asm ref
            //string[] pathToOldAsmDefRef = AssetDatabase.FindAssets(".asmref");
            /Object[] assetsAtPath = AssetDatabase.LoadAllAssetsAtPath("Assets/");
            foreach (Object o in assetsAtPath)
            {
                //if (o is AssemblyDefinitionReferenceAsset)
                {
                    Debug.Log(o.name);
                }
            }


            string asmDefPath = AssetDatabase.GetAssetPath(asmDef);
            GUID guid = AssetDatabase.GUIDFromAssetPath(asmDefPath);
            
            string asmRefPath = folderPath + "/" + asmDef.name + ".asmref";
            string asmrefContents = LDtkInternalLoader.Load<TextAsset>(ASM_REF_TEMPLATE_PATH).text;

            asmrefContents = asmrefContents.Replace(ASM_REF_KEY, guid.ToString());
            
            using (StreamWriter streamWriter = new StreamWriter(asmRefPath))
            {
                streamWriter.Write(asmrefContents);
            }
        }
        
        
    }
}