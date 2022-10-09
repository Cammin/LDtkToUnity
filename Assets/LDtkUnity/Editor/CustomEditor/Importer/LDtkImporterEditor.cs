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
        public override bool showImportedObject => false;
        protected override bool useAssetDrawPreview => false;
        //protected override bool ShouldHideOpenButton() => false;

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
            if (!importer.IsBackupFile())
            {
                return false;
            }
            
            const string msg = "This LDtk file is a backup file and as a result, was not imported.\n" +
                               "To import this file, move it to a folder with a name that doesn't contain \"backups\".";

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
    }
}