using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkEditorCommandUpdater
    {
        public string ProjectPath;
        public string RelPath;

        public LDtkEditorCommandUpdater(string projectPath)
        {
            ProjectPath = projectPath;
            RelPath = GetPath();
        }

        private string GetPath()
        {
            string fromPath = LDtkPathUtility.AssetsPathToAbsolutePath(ProjectPath);
            string appPath = LDtkTilesetExporterUtil.PathToExe();

            var relPath = LDtkPathUtility.GetRelativePath(fromPath, appPath);
            //backslashes break deserialization
            relPath = LDtkPathUtility.CleanPathSlashes(relPath);
            
            //Debug.Log($"fromPath {fromPath}");
            //Debug.Log($"appPath {appPath}");
            //Debug.Log($"relPath {relPath}");
            return relPath;
        }
        
        public void TryDrawFixButton(LdtkJson data)
        {
            //if it defined no tileset defs, it's fine
            if (data.Defs.Tilesets.IsNullOrEmpty())
            {
                return;
            }
            
            if (HasCustomCommand(data, out var reason))
            {
                return;
            }

            if (!IsInstalled(out var installReason))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUIUtility.SetIconSize(Vector2.one * 32);
                    EditorGUILayout.HelpBox($"The importer requires an app installed to the Library folder. This is a one time process.\nReason: {installReason}", MessageType.Error);
                    
                    EditorGUIUtility.SetIconSize(new Vector2(16, 16));
                    GUIContent installContent = new GUIContent()
                    {
                        text = "Install",
                        tooltip = "This will extract from a zip file in this package into the Library folder.",
                        image = EditorGUIUtility.IconContent("Add-Available").image
                    };

                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(75)))
                    {
                        if (GUILayout.Button(installContent, GUILayout.ExpandHeight(true)))
                        {
                            LDtkTilesetExporterUtil.UnzipToLibrary();
                            AssetDatabase.Refresh();
                        }
                    }
                }

                EditorGUILayout.Space();
                LDtkEditorGUIUtility.DrawDivider();
                return;
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUIUtility.SetIconSize(Vector2.one * 32);
                EditorGUILayout.HelpBox($"This project needs a command that runs this after saving:\n{RelPath}\nReason: {reason}", MessageType.Error);
                
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(50)))
                {
                    EditorGUIUtility.SetIconSize(new Vector2(16, 14));
                    GUIContent fixContent = new GUIContent()
                    {
                        text = "Fix",
                        image = LDtkIconUtility.GetUnityIcon("SceneViewTools@2x", "")
                    };
                    if (GUILayout.Button(fixContent, GUILayout.ExpandHeight(true)))
                    {
                        int result = EditorUtility.DisplayDialogComplex(
                            "Add Command",
                            "To import the LDtk project into Unity properly, it needs to load tileset files.\n" +
                            "\n" +
                            "To generate tileset files, you configure LDtk to run a custom export app through a custom command.\n" +
                            "(The app is included with this importer)\n" +
                            "\n" +
                            "To add the command:\n" +
                            "- Go to LDtk's project settings\n" +
                            "- Create a new command\n" +
                            "- Set the timing to \"Run after saving\"\n" +
                            "- Paste the following path from your clipboard:\n" +
                            $"\"{RelPath}\"\n" +
                            $"\n" +
                            "After adding the command, save the project. If a warning appears, select \"I understand the risk, allow user commands\".\n" +
                            "\n" +
                            "You only need to configure this once. Now with every project save, tileset definition files will be generated!\n" +
                            "\n" +
                            "If you are wondering what the export app does, you can view it's GitHub page.",
                            "Copy to Clipboard",
                            "Cancel",
                            "Open tileset exporter's GitHub");
                        switch (result)
                        {
                            case 0:
                                ToClipboard();
                                break;
                            case 1:
                                //cancel
                                break;
                            case 2:
                                Application.OpenURL("https://github.com/Cammin/LDtkTilesetExporter");
                                break;
                        }

                        GUIUtility.ExitGUI();
                    }
                    
                    EditorGUIUtility.SetIconSize(new Vector2(11, 13));
                    GUIContent copyContent = new GUIContent()
                    {
                        text = "Copy",
                        image = EditorGUIUtility.IconContent("Clipboard").image
                    };
                    if (GUILayout.Button(copyContent, GUILayout.ExpandHeight(true)))
                    {
                        ToClipboard();
                    }
                }
            }
            
            EditorGUILayout.Space();
            LDtkEditorGUIUtility.DrawDivider();
        }

        private void ToClipboard()
        {
            GUIUtility.systemCopyBuffer = RelPath;
            LDtkDebug.Log($"Copied to clipboard: \"{RelPath}\". Paste this as a new custom command in LDtk then save!");
        }

        public bool IsInstalled(out string reason)
        {
            if (File.Exists(LDtkTilesetExporterUtil.PathToExe()))
            {
                if (!LDtkTilesetExporterUtil.GetAppUpToDate())
                {
                    reason = $"The app's version does not match the required one";
                    return false;
                }
                
                reason = null;
                return true;
            }

            reason = $"The app doesn't exist";
            return false;
        }
        
        public bool HasCustomCommand(LdtkJson data, out string reason)
        {
            LdtkCustomCommand[] commands = data.CustomCommands;
            
            foreach (LdtkCustomCommand command in commands)
            {
                if (command.Command == RelPath)
                {
                    if (command.When != When.AfterSave)
                    {
                        reason = "The command exists, but the timing is not set to \"Run after saving\"";
                        return false;
                    }

                    reason = null;
                    return true;
                }
            }

            reason = $"A command to the above path doesn't exists";
            return false;
        }
    }
}