using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkScriptingDefines
    {
        private const string DEFINE = "LDTK_ENABLE_PROFILER";
        
        private static readonly GUIContent WriteProfiledImports = new GUIContent
        {
            text = "LDtk Profiler Define",
            tooltip = 
                      "Add " + DEFINE + " to the scripting defines to enable import profiling.\n" +
                      "\n" +
                      "Upon importing any LDtk level or project, unity will write a .raw file to a \"Profiler\" folder in this root unity project.\n" +
                      "These files can be opened from the profiler window to view the performance of an import.\n" +
                      "\n" +
                      "Only toggle on for analysis purposes. This has a performance overhead for every import and the files can also use a lot of storage, especially if deep profiling is enabled.",
            image = LDtkIconUtility.GetUnityIcon("UnityEditor.ProfilerWindow", ""),
        };
        
        public static void PreprocessorAddRemoveGui()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(WriteProfiledImports);
                //EditorGUILayout.HelpBox($"To write import profiling data, add the \"LDTK_ENABLE_PROFILING\" to your scripting defines.", MessageType.Info);
                DrawButton();
            }
        }

        private static void DrawButton()
        {
            BuildTargetGroup current = EditorUserBuildSettings.selectedBuildTargetGroup;

#if UNITY_2020_1_OR_NEWER
            NamedBuildTarget group = NamedBuildTarget.FromBuildTargetGroup(current);
            string currentDefines = PlayerSettings.GetScriptingDefineSymbols(group);
#else
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(current);
#endif

            GUILayoutOption width = GUILayout.Width(180);
            if (currentDefines.Contains(DEFINE))
            {
                if (GUILayout.Button($"- {DEFINE}", width))
                {
                    string newDefines = currentDefines
                        .Replace($"{DEFINE};", "")
                        .Replace(DEFINE, "")
                        .TrimEnd(';');
                    SetNewDefines(newDefines);
                }
            }
            else
            {
                if (GUILayout.Button($"+ {DEFINE}", width))
                {
                    string reformat = string.IsNullOrEmpty(currentDefines) || currentDefines.EndsWith(";") ? "" : ";";
                    string newDefines = currentDefines + reformat + DEFINE;
                    SetNewDefines(newDefines);
                }
            }

            void SetNewDefines(string newDefines)
            {
#if UNITY_2020_1_OR_NEWER
                PlayerSettings.SetScriptingDefineSymbols(group, newDefines);
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(current, newDefines);
#endif
            }
        }
    }
}