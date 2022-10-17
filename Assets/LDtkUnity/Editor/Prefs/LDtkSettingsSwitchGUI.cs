using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkSettingsSwitchGUI
    {
        private static readonly GUIContent SwitchToSettings = new GUIContent
        {
            text = "To Project Settings",
            tooltip = "Switches to the LDtkToUnity section in the Project Settings.",
            image = LDtkIconUtility.GetUnityIcon("MoreOptions", "")
        };
        private static readonly GUIContent SwitchToPrefs = new GUIContent
        {
            text = "To Preferences",
            tooltip = "Switches to the LDtkToUnity section in the Preferences.",
            image = LDtkIconUtility.GetUnityIcon("MoreOptions", "")
        };
        
        public static void DrawSwitchSettingsButton()
        {
            if (DrawButton(SwitchToSettings))
            {
                SettingsService.OpenProjectSettings(LDtkProjectSettingsProvider.PROVIDER_PATH);
            }
        }
        public static void DrawSwitchPrefsButton()
        {
            if (DrawButton(SwitchToPrefs))
            {
                SettingsService.OpenUserPreferences(LDtkPrefsProvider.PROVIDER_PATH);
            }
        }

        private static bool DrawButton(GUIContent switchToSettings)
        {
            bool pressed;
            using (new GUILayout.HorizontalScope())
            using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
            {
                GUILayout.FlexibleSpace();
                pressed = GUILayout.Button(switchToSettings, GUILayout.Width(150));
            }

            return pressed;
        }
    }
}