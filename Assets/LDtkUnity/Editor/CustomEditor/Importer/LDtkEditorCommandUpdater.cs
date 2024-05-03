using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkEditorCommandUpdater
    {
        public string ProjectPath;
        public string ProjectName;
        
        public string ExePath;
        public string Arg;
        
        public string Command;

        public LDtkEditorCommandUpdater(string projectPath)
        {
            ProjectPath = projectPath;
            ProjectName = Path.GetFileNameWithoutExtension(projectPath);
            ExePath = GetExecutablePath();
            Arg = $"\\\"{ProjectName}\\\"";
            Command += $"{ExePath} \"{ProjectName}\"";
        }

        private string GetExecutablePath()
        {
            string fromPath = LDtkPathUtility.AssetsPathToAbsolutePath(ProjectPath);
            
            
#if UNITY_EDITOR_OSX
            //if mac, we're launching a different file that's meant to launch the exe
            string appPath = LDtkTilesetExporterUtil.PathToMacSh();
#else
            string appPath = LDtkTilesetExporterUtil.PathToExe();
#endif
            
            var commandContent = LDtkPathUtility.GetRelativePath(fromPath, appPath);
            //backslashes break deserialization
            commandContent = LDtkPathUtility.CleanPathSlashes(commandContent);
            
            //Debug.Log($"fromPath {fromPath}");
            //Debug.Log($"appPath {appPath}");
            //Debug.Log($"relPath {relPath}");
            
            return commandContent;
        }
        
        public void TryDrawFixButton(LdtkJson data)
        {
            //if it defined no tileset defs, it's fine
            if (data.Defs.Tilesets.IsNullOrEmpty())
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
                        image = LDtkIconUtility.GetUnityIcon("CreateAddNew", "")
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
            
            if (HasCustomCommand(data, out var reason))
            {
                return;
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUIUtility.SetIconSize(Vector2.one * 32);
                EditorGUILayout.HelpBox($"This project needs a command that should run this after saving:\n{Command}\nReason: {reason}", MessageType.Error);
                
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
                            "- Ensure LDtk is saved and closed first\n" +
                            "- Click the button below to modify the LDtk file which adds the command automatically\n" +
                            $"\n" +
                            "After adding the command, save the project. If a warning appears, select \"I understand the risk, allow user commands\".\n" +
                            "You only need to configure this once. Now with every project save, tileset definition files will be generated!\n" +
                            "If you are wondering what the export app does, you can view it's GitHub page.",
                            "Auto-add command",
                            "Cancel",
                            "Open tileset exporter's GitHub");
                        switch (result)
                        {
                            case 0:
                                TryModifyProjectWithCommand();
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
            GUIUtility.systemCopyBuffer = Command;
            LDtkDebug.Log($"Copied to clipboard: \"{Command}\". Paste this as a new custom command in LDtk then save!");
        }

        public void TryModifyProjectWithCommand()
        {
            if (IsBlockedFromAddingCommand())
            {
                return;
            }
            bool IsBlockedFromAddingCommand()
            {
                while (true)
                {
                    if (Process.GetProcessesByName("LDtk").IsNullOrEmpty())
                    {
                        return false;
                    }
                    
                    int result = EditorUtility.DisplayDialogComplex(ProjectName, 
                        "Didn't add command.\n" + 
                        "Close all LDtk processes, and try again.\n" + 
                        "\n" + "Alternatively, you may add the command manually:\n" + 
                        "- Copy the path to the clipboard\n" + 
                        "- Go to LDtk's project settings\n" + 
                        "- Create a new command\n" + 
                        "- Set the timing to \"Run after saving\"\n" + 
                        "- Paste the following path from your clipboard:\n" + 
                        $"\"{Command}\"\n", 
                        "Try Again", "Close", "Copy to Clipboard");
                    switch (result)
                    {
                        case 0:
                            continue;
                        case 1:
                            return true;
                        case 2:
                            ToClipboard();
                            return true;
                    }
                }
            }
            
            if (ModifyProjectWithCommand(ProjectPath, ExePath, Arg))
            {
                EditorUtility.DisplayDialog("Modified", $"Modified\n\"{ProjectName}\"\nwith the custom command.\nNow open the project and save!", "Ok");
                AssetDatabase.ImportAsset(ProjectPath);
            }
            else
            {
                LDtkDebug.LogWarning($"Failed to modify \"{ProjectName}\" with the custom command, likely due to foreign custom commands already existing. Try taking the manual approach instead via the clipboard.");
            }
        }

        public static bool ModifyProjectWithCommand(string projectPath, string exeRelPath, string arg)
        {
            if (Path.GetExtension(projectPath) != ".ldtk")
            {
                return false;
            }
            
            const string before = @"""customCommands"": [],";
            const string after = @"""customCommands"": [{ ""command"": ""_PATH"", ""when"": ""AfterSave"" }],";
            string insert = after.Replace("_PATH", $"{exeRelPath} {arg}");
            
            string[] lines = File.ReadAllLines(projectPath);
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains(before))
                {
                    found = true;
                    lines[i] = line.Replace(before, insert);
                    break;
                }
            }

            if (found)
            {
                File.WriteAllLines(projectPath, lines);
            }
            return found;
        }

        public bool IsInstalled(out string reason)
        {
            if (File.Exists(LDtkTilesetExporterUtil.PathToExe()))
            {
                if (!LDtkTilesetExporterUtil.GetAppUpToDate(out var old, out var required))
                {
                    reason = $"The app's version ({old}) does not match the required one ({required})";
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
            
            if (commands == null)
            {
                reason = "customCommands was null. Old LDtk version?";
                return false;
            }
            
            foreach (LdtkCustomCommand command in commands)
            {
                if (command.Command == Command)
                {
                    if (command.When != When.AfterSave)
                    {
                        reason = "The command exists, but the timing is not set to \"Run after saving\"";
                        return false;
                    }
                    
                    //ensure that there is a 2nd arg.
                    string[] split = Regex.Matches(command.Command, @"[\""].+?[\""]|[^ ]+")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToArray();
                    
                    if (split.Length != 2 || split[1] != $"\"{ProjectName}\"")
                    {
                        reason = $"The command exists, but doesn't have a single parameter of the project name." +
                                 $"\n" +
                                 $"The command was this:\n{command.Command}";
                        return false;
                    }

                    reason = null;
                    return true;
                }
            }

            reason = $"A command to the above path doesn't exists or needs to be re-pasted";

            if (commands.Length == 1)
            {
                reason +=
                    $"\n" +
                    $"The incorrect command was this:\n{commands[0].Command}";
            }
            return false;
        }
    }
}