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

            
            string[] oldAsmDefRefGuids = AssetDatabase.FindAssets("t:AssemblyDefinitionReferenceAsset", new [] { folderPath });

            //delete any amsrefs that are not supposed to be there
            foreach (string oldGuid in oldAsmDefRefGuids)
            {
                string pathToOldAsset = AssetDatabase.GUIDToAssetPath(oldGuid);
                AssemblyDefinitionReferenceAsset asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionReferenceAsset>(pathToOldAsset);
                
                //only delete if the asmdef was not assigned or is not the same name

                if (asmDef != null && asset.name == asmDef.name)
                {
                    continue;
                }
                
                AssetDatabase.DeleteAsset(pathToOldAsset);
            }
            
            //don't make a file if none was inputted
            if (asmDef == null)
            {
                return;
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

            //consider pinging? tried but decided against it
            //AssemblyDefinitionReferenceAsset newAsset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionReferenceAsset>(asmRefPath);
            //EditorGUIUtility.PingObject(newAsset);
        }
        
        
    }
}