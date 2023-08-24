using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    //we are not overriding RenderStaticPreview because it didn't work for scripted importers
    internal abstract class LDtkImporterEditor : ScriptedImporterEditor
    {
        public override bool showImportedObject => true;
        protected override bool useAssetDrawPreview => false;
        //protected override bool ShouldHideOpenButton() => false;

        protected LDtkJsonImporter Importer;
        protected ImportLogEntries Entries;
        private bool _isBackupFile;

        protected LDtkSectionDependencies SectionDependencies;
        private SerializedProperty _reimportOnDependencyChangedProp;
        private readonly GUIContent _reimportOnDependencyChanged = new GUIContent
        {
            text = "Depend On Dependencies",
            tooltip = "Controls whether this project/level should be reimported when any of the dependencies are changed. (ex. saved changes to a prefab)\n" +
                      "If turned off, then the import result will not update the assets (such as prefabs) when the dependencies are changed, in which case you will need to manually reimport this project/level.\n" +
                      "Only consider turning off if changes to assets are causing frequent unwanted reimports."
        };
        
        public override void OnEnable()
        {
            base.OnEnable();
            SectionDependencies = new LDtkSectionDependencies(this, serializedObject);
            _reimportOnDependencyChangedProp = serializedObject.FindProperty(LDtkJsonImporter.REIMPORT_ON_DEPENDENCY_CHANGE);
            SectionDependencies.Init();

            Importer = (LDtkJsonImporter)target;
            _isBackupFile = Importer.IsBackupFile();

#if !UNITY_2022_2_OR_NEWER
            Entries = new ImportLogEntries(Importer.assetPath);
            Entries.ReadTheEntries();
#endif
        }

        protected override void Apply()
        {
            base.Apply();
            UpdateDependenciesDrawer();
            
            SectionDependencies.Dispose();
        }

        private void UpdateDependenciesDrawer() //a bit hacky, but gets the job done with little performance issue
        {
            SectionDependencies.UpdateDependencies();
            EditorApplication.delayCall += () =>
            {
                SectionDependencies.UpdateDependencies();
                EditorApplication.delayCall += SectionDependencies.UpdateDependencies;
            };
        }

        protected bool TryDrawBackupGui<T>(LDtkJsonImporter<T> importer) where T : ScriptableObject, ILDtkJsonFile
        {
            if (!_isBackupFile)
            {
                return false;
            }
            
            const string msg = "This LDtk file is a backup file and as a result, was not imported.\n" +
                               "To import this file, move it to a folder outside of the backups folder and reimport it.";

            DrawTextBox(msg, MessageType.Info);
            //AssetDatabase.ForceReserializeAssets();
            return true;

        }
        
        protected static void DrawTextBox(string msg = null, MessageType type = MessageType.Error)
        {
            const string errorContent = "There was a breaking import error; Try reimporting this asset, which might fix it.\n" +
                                        "Check if there are any import errors in the console window, and report to the developer so that it can be addressed.";

            if (msg == null)
            {
                msg = errorContent;
            }

            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 32))
            {
                EditorGUILayout.HelpBox(msg, type);
            }
        }

        public void DrawDependenciesProperty()
        {
            EditorGUILayout.PropertyField(_reimportOnDependencyChangedProp, _reimportOnDependencyChanged);
        }
        
        public void DrawLogEntries()
        {
            if (Entries == null)
            {
                return;
            }
            
            List<ImportLogEntry> entries = Entries._entries;

            if (entries.IsNullOrEmpty())
            {
                return;
            }

            ImportLogEntry[] errors = entries.Where(p => p._flag == ImportLogFlags.Error).ToArray();
            ImportLogEntry[] warnings = entries.Where(p => p._flag == ImportLogFlags.Warning).ToArray();

            MessageType msgType = errors.IsNullOrEmpty() ? MessageType.Warning : MessageType.Error;

            string msg;
            if (!errors.IsNullOrEmpty() && !warnings.IsNullOrEmpty())
            {
                msg = $"Last import generated {errors.Length} errors / {warnings.Length} warnings.";
            }
            else if (!errors.IsNullOrEmpty())
            {
                msg = $"Last import generated {errors.Length} errors";
            }
            else
            {
                msg = $"Last import generated {warnings.Length} warnings";
            }

            Rect rect = EditorGUILayout.GetControlRect(false, 40f);
            rect.xMin -= 15;

            EditorGUI.HelpBox(rect, msg, msgType);

            rect.x += rect.width - 97;
            rect.y += 12;
            rect.size = new Vector2(93, 18);
                      
            if (GUI.Button(rect, "Print to console", EditorStyles.miniButtonRight))
            {
                foreach (ImportLogEntry entry in entries)
                {
                    entry.PrintToConsole(target);
                }
            }
        }
    }
}