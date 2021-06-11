using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkAssetMovedDialog : UnityEditor.AssetModificationProcessor
    {
        private const string DIALOGUE_KEY = "LDtkMoveDialogue";
        private const string DIALOGUE_OK = "Okay";
        private const string DIALOGUE_CANCEL = "Cancel";
        
        private static bool ProjectDialog(string title, string description)
        {
            return EditorUtility.DisplayDialog(
                $"Move {title}", 
                description, 
                DIALOGUE_OK, 
                DIALOGUE_CANCEL, 
                DialogOptOutDecisionType.ForThisSession, 
                DIALOGUE_KEY);
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            string fileName = Path.GetFileName(sourcePath);
            string srcPath = Path.GetExtension(sourcePath).Substring(1);

            switch (srcPath)
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
            
            File.Move(sourcePath, destinationPath);
            return AssetMoveResult.DidMove;
        }
    }
}