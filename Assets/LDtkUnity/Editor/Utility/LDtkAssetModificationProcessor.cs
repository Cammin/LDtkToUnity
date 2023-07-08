using System;
using System.IO;
using UnityEditor;

namespace LDtkUnity.Editor
{
    // ReSharper disable once RedundantNameQualifier
    internal sealed class LDtkAssetModificationProcessor : UnityEditor.AssetModificationProcessor
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
                string extension = Path.GetExtension(sourcePath);
                if (extension.Length == 0) //moved by empty extension
                {
                    return AssetMoveResult.DidNotMove;
                }
                
                string ext = extension.Substring(1);
                if (ext != LDtkImporterConsts.PROJECT_EXT && ext != LDtkImporterConsts.LEVEL_EXT && ext != LDtkImporterConsts.TILESET_EXT)
                {
                    return AssetMoveResult.DidNotMove;
                }
                
                //if it was just a rename
                string srcDir = Path.GetDirectoryName(sourcePath);
                string destDir = Path.GetDirectoryName(destinationPath);

                if (srcDir == destDir)
                {
                    switch (ext)
                    {
                        case LDtkImporterConsts.PROJECT_EXT:
                            LDtkDebug.LogWarning("Did not rename the LDtk project. You should instead rename the project inside of the LDtk editor's project settings.");
                            break;
                                
                        case LDtkImporterConsts.LEVEL_EXT:
                            LDtkDebug.LogWarning("Did not rename the LDtk level. Renaming the level should be decided by the project inside the LDtk editor.");
                            break;
                                
                        case LDtkImporterConsts.TILESET_EXT:
                            LDtkDebug.LogWarning("Did not rename the LDtk tileset. The tilesets name is determined by the project's export process.");
                            break;
                    }
                    
                    return AssetMoveResult.FailedMove;
                }

                string fileName = Path.GetFileName(sourcePath);
                switch (ext)
                {
                    case LDtkImporterConsts.PROJECT_EXT:
                        if (!ProjectDialog(
                            fileName,
                            "Are you sure about moving the project?\n" +
                            "This will break path connections for tileset files, enum generation paths, and levels.\n" +
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
                    
                    case LDtkImporterConsts.TILESET_EXT:
                        if (!ProjectDialog(
                            fileName,
                            "Are you sure about moving the tileset?\n" +
                            "This will break the path connection from the corresponding LDtk project and texture.\n" +
                            "If moving the tileset, make sure to move the relevant project as well."))
                        {
                            return AssetMoveResult.FailedMove;
                        }
                        break;
                }
            }
            catch(Exception e)
            {
                LDtkDebug.LogError($"Problem while moving an asset: {e}");
            }

            return AssetMoveResult.DidNotMove;

        }
    }
}