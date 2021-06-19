using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Meaf75.Unity {

    public class TimeRecorderTools : EditorWindow {

        private string backupFileName = "time_recorder_backup.json";
        private TextAsset backupTextAsset;

        private Vector2 scrollPos = Vector2.zero;

        private static readonly Vector2 windowSize = new Vector2(543,270);

        private static TimeRecorderTools instance;

        [MenuItem("Tools/Time recorder/Time recorder tools")]
        static void Init() {
            // Get existing open window or if none, make a new one:
            var window = (TimeRecorderTools) GetWindow(typeof(TimeRecorderTools));
            window.titleContent = new GUIContent("Time recorder tools");

            window.minSize = windowSize;
            window.maxSize = windowSize;
        }

        private void OnEnable() {
            instance = this;
        }

        private void OnGUI() {

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.LabelField("Time recorder tools");

            // Pause section
            GUILayout.Space(15);
            EditorGUILayout.LabelField("Pause Time recorder", EditorStyles.boldLabel);

            // State style
            var runningStateLabel = new GUIStyle(EditorStyles.boldLabel) {
                normal = {
                    textColor = TimeRecorder.isPaused ? Color.red : Color.green
                }
            };

            string stateLabel = TimeRecorder.isPaused ? "Paused" : "Running";

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Time recorder is ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(stateLabel, runningStateLabel);

            // Action btn
            if (GUILayout.Button(TimeRecorderExtras.GetPauseButtonLabelForState(TimeRecorder.isPaused))) {
                ChangeTimeRecorderPauseState(!TimeRecorder.isPaused);
            }
            GUILayout.EndHorizontal();

            // Backup section
            GUILayout.Space(15);
            EditorGUILayout.LabelField("Backup", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Generate a json file with your Time recorder work time data");

            backupFileName = EditorGUILayout.TextField("Backup file name: ", backupFileName);

            if (GUILayout.Button("Generate backup")) {
                GenerateBackup();
            }

            // Restore section
            GUILayout.Space(15);
            EditorGUILayout.LabelField("Restore from file", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Drag here any valid json file with the time recorder format and restore your work time");

            backupTextAsset = EditorGUILayout.ObjectField ("Backup file",backupTextAsset,typeof(TextAsset),false) as TextAsset;

            if (GUILayout.Button("Restore from backup")) {
                RestoreFromBackup();
            }

            EditorGUILayout.EndScrollView();
        }

        void GenerateBackup() {
            string path = Path.Combine(Application.dataPath, backupFileName);

            string timeRecorderJson = PlayerPrefs.GetString(TimeRecorderExtras.TIME_RECORDER_REGISTRY, "");

            if (string.IsNullOrEmpty(timeRecorderJson)) {
                Debug.LogWarning("There is nothing to backup");
                return;
            }

            File.WriteAllText(path,timeRecorderJson);
            Debug.Log("Your time recorder backup was written at "+path);
        }

        void RestoreFromBackup() {

            if (!backupTextAsset) {
                Debug.LogError("First select a backup file");
                return;
            }

            string timeRecorderJson = backupTextAsset.text;

            TimeRecorderInfo timeRecorderInfoData;

            try{
                // Get data
                timeRecorderInfoData = JsonUtility.FromJson<TimeRecorderInfo>(timeRecorderJson);
            } catch(Exception){
                Debug.LogError("Your time recorder registry JSON file data is corrupted, please try another valid file. I'm sorry");
                return;
            }

            PlayerPrefs.SetString(TimeRecorderExtras.TIME_RECORDER_REGISTRY, timeRecorderJson);
            PlayerPrefs.Save();

            // Override loaded data with file data new one
            TimeRecorder.timeRecorderInfo = timeRecorderInfoData;

            Debug.Log("Your time recorder registry was restored from the given file");

            if (TimeRecorderWindow.Instance) {
                TimeRecorderWindow.Instance.RepaintWindow();
            }
        }

        public static void ChangeTimeRecorderPauseState(bool paused) {
            TimeRecorder.isPaused = paused;

            var calendarWindow = TimeRecorderWindow.Instance;

            if (calendarWindow) {    // Update calendar window
                calendarWindow.UpdatePausedState(TimeRecorder.isPaused);
            }

            PlayerPrefs.SetInt(TimeRecorderExtras.TIME_RECORDER_PAUSE_P_PREF, TimeRecorder.isPaused ? 1 : 0);
            PlayerPrefs.Save();

            string state = paused ? "Paused" : "Runnning";

            Debug.Log($"Time recorder is now: {state}");

            if (instance)    // Update this window
                instance.Repaint();
        }
    }
}