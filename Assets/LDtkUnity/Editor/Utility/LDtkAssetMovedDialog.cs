using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkAssetMovedDialog : UnityEditor.AssetModificationProcessor
    {
        private const string DIALOGUE_KEY = "LDtkMoveDialogue";
        private const string DIALOGUE_OK = "Move";
        private const string DIALOGUE_CANCEL = "Cancel";
        
        private static bool ProjectDialog(string title, string description)
        {
            string titleMsg = $"Move {title}";
            return EditorUtility.DisplayDialog(
                titleMsg, 
                description, 
                DIALOGUE_OK, 
                DIALOGUE_CANCEL, 
                DialogOptOutDecisionType.ForThisSession, 
                DIALOGUE_KEY);
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            try
            {
                //if it was just a rename
                string srcDir = Path.GetDirectoryName(sourcePath);
                string destDir = Path.GetDirectoryName(destinationPath);

                if (srcDir == destDir)
                {
                    return AssetMoveResult.DidNotMove;
                }

                string extension = Path.GetExtension(sourcePath);
                if (extension.Length == 0) //moved by empty extension
                {
                    return AssetMoveResult.DidNotMove;
                }
                
                string ext = extension.Substring(1);
                string fileName = Path.GetFileName(sourcePath);
                switch (ext)
                {
                    case LDtkImporterConsts.PROJECT_EXT:
                        if (!ProjectDialog(
                            fileName,
                            "Are you sure about moving the project?\n" +
                            "This will break path connections for tileset textures, enum generation paths, and levels.\n" +
                            "If moving the project, make sure to move the relevant connected assets as well."))
                        {
                            return AssetMoveResult.FailedMove;
                        }

                        break;
                    case LDtkImporterConsts.LEVEL_EXT:
                        if (!ProjectDialog(
                            fileName,
                            "Are you sure about moving the level?\n" +
                            "This will break the path connection from the corresponding LDtk project.\n" +
                            "If moving the level, make sure to move the relevant project as well."))
                        {
                            return AssetMoveResult.FailedMove;
                        }

                        break;
                }
            }
            catch(Exception e)
            {
                Debug.LogError($"LDtk: Problem while moving an asset: {e}");
            }

            return AssetMoveResult.DidNotMove;

        }
    }
}