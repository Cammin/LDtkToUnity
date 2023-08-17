using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LDtkProjectImporter))]
    internal sealed class LDtkProjectImporterEditor : LDtkImporterEditor
    {
        private LDtkJsonEditorCache _cache;
        private LDtkProjectImporter _importer;
        private LDtkEditorCommandUpdater _commandUpdater;

        private ILDtkSectionDrawer[] _sectionDrawers;
        private LDtkSectionMain _sectionMain;
        private LDtkSectionIntGrids _sectionIntGrids;
        private LDtkSectionEntities _sectionEntities;
        private LDtkSectionEnums _sectionEnums;
        private bool _shouldApply = true;



        private static readonly GUIContent ExportButtonContent = new GUIContent()
        {
            text = "Export",
            tooltip = "Export Native Prefab"
        };

        public override void OnEnable()
        {
            base.OnEnable();

            _importer = (LDtkProjectImporter)target;
            ConstructCache();
            LDtkUidBank.CacheUidData(_cache.Json);

            _commandUpdater = new LDtkEditorCommandUpdater(_importer.assetPath);
            
            _sectionMain = new LDtkSectionMain(this, serializedObject);
            _sectionIntGrids = new LDtkSectionIntGrids(this, serializedObject);
            _sectionEntities = new LDtkSectionEntities(this, serializedObject);
            _sectionEnums = new LDtkSectionEnums(this, serializedObject);

            _sectionDrawers = new[]
            {
                (ILDtkSectionDrawer)_sectionMain,
                _sectionIntGrids,
                _sectionEntities,
                _sectionEnums,
                SectionDependencies
            };

            foreach (ILDtkSectionDrawer drawer in _sectionDrawers)
            {
                drawer.Init();
            }
            LDtkUidBank.ReleaseDefinitions();
        }

        public override void OnDisable()
        {
            if (_sectionDrawers != null)
            {
                foreach (ILDtkSectionDrawer drawer in _sectionDrawers)
                {
                    drawer?.Dispose();
                }
            }

            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            try
            {
                GUIUpdate();
            }
            finally
            {
                ApplyRevertGUI();
                LDtkUidBank.ReleaseDefinitions();
            }
        }

        private void GUIUpdate()
        {
            serializedObject.Update();

            if (TryDrawBackupGui(_importer))
            {
                return;
            }

            DrawLogEntries();
            
            TryReconstructCache();

            LDtkUidBank.CacheUidData(_cache.Json);

            Profiler.BeginSample("ShowGUI");
            ShowGUI();
            Profiler.EndSample();

            serializedObject.ApplyModifiedProperties();

            if (_shouldApply)
            {
                ApplyIfArraySizesChanged();
                _shouldApply = false;
            }

            DrawPotentialProblem();
        }

        private void TryReconstructCache()
        {
            if (_cache == null)
            {
                LDtkDebug.LogError("Bug, cache is null, but its expected to never be null");
                return;
            }
            
            if (_cache.ShouldForceReconstruct())
            {
                ConstructCache();
                _shouldApply = true;
            }
        }
        
        private void ConstructCache()
        {
            _cache = new LDtkJsonEditorCache((LDtkProjectImporter)target);
        }

        private void ShowGUI()
        {
            Profiler.BeginSample("JsonSetup");

            EditorGUIUtility.SetIconSize(Vector2.one * 16);
            LdtkJson data = GetJson();
            if (data == null)
            {
                DrawTextBox();
                Profiler.EndSample();
                return;
            }
            
            //todo disabled for now. Currently doesn't work perfectly as expected
            //DrawExportButton();

            
            
            _sectionMain.SetJson(data);
            _commandUpdater.TryDrawFixButton(data);
            EditorGUIUtility.SetIconSize(Vector2.one * 16);

            Definitions defs = data.Defs;
            Profiler.EndSample();

            Profiler.BeginSample("MainSection");
            _sectionMain.Draw();
            Profiler.EndSample();
            
            Profiler.BeginSample("IntGridSection");
            _sectionIntGrids.Draw(defs.IntGridLayers);
            Profiler.EndSample();
            
            Profiler.BeginSample("EntitiesSection");
            _sectionEntities.Draw(defs.Entities);
            Profiler.EndSample();
            
            Profiler.BeginSample("EnumsSection");
            _sectionEnums.Draw(defs.Enums);
            Profiler.EndSample();            
            
            Profiler.BeginSample("DependenciesSection");
            SectionDependencies.Draw();
            Profiler.EndSample();
            
            LDtkEditorGUIUtility.DrawDivider();
        }

        private void DrawExportButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool button = GUILayout.Button(ExportButtonContent, GUILayout.Width(45));
            GUILayout.EndHorizontal();

            if (button)
            {
                GameObject gameObject = (GameObject)assetTarget;
                LDtkNativeExportWindow.CreateWindowWithContext(gameObject);
            }
            
            LDtkEditorGUIUtility.DrawDivider();
        }

        private LdtkJson GetJson()
        {
            if (_cache == null)
            {
                LDtkDebug.LogError("Cache was null");
                return null;
            }
            
            LdtkJson cachedJson = _cache.Json;
            if (cachedJson != null)
            {
                return cachedJson;
            }

            return null;
        }
        
        private void ApplyIfArraySizesChanged()
        {
            //IMPORTANT: if there are any new/removed array elements via this setup of automatically resizing arrays as LDtk definitions change,
            //then Unity's going to notice and make the apply/revert buttons appear active which normally gives us trouble when we try clicking out.
            //So, try applying right now when this specific case happens; whenever there is an array resize.
            
            if (_sectionDrawers.Any(drawer => drawer.HasResizedArrayPropThisUpdate))
            {
                Apply();
                //Debug.Log("Applied an array resize and reimported as a result");
            }
        }
        
        private void DrawPotentialProblem()
        {
            bool problem = _sectionDrawers.Any(drawer => drawer.HasProblem);

            if (problem)
            {
                EditorGUIUtility.SetIconSize(Vector2.one * 32);
                EditorGUILayout.HelpBox(
                    "LDtk Project asset configuration has unresolved issues, mouse over them to see the problem",
                    MessageType.Warning);
            }
        }

        
    }
}