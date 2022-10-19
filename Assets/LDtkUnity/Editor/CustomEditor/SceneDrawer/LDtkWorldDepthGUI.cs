using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkWorldDepthGUI
    {
        private const string KEY = "WorldDepthIndex";
        private static readonly GUIContent WindowContent = new GUIContent()
        {
            text = "World Depth",
            tooltip = "In LDtk, You can set world depth.\nUse these buttons to show/hide levels of a certain world depth."
        };
        
        private List<LDtkComponentLevel> _levels;
        private int[] _depths;
        
        private int _selectedDepthIndex;
        private int _prevSelectedDepthIndex;

        public LDtkWorldDepthGUI()
        {
            int i = EditorPrefs.GetInt(KEY, -1);
            _selectedDepthIndex = i;
            _prevSelectedDepthIndex = i;
        }

        private void SaveToPrefs()
        {
            EditorPrefs.SetInt(KEY, _selectedDepthIndex);
        }

        public bool CanDraw()
        {
            _levels = LDtkFindInScenes.FindInAllScenes<LDtkComponentLevel>();
            if (_levels.IsNullOrEmpty())
            {
                return false;
            }
            
            _depths = _levels.Select(p => p.WorldDepth).Distinct().OrderBy(p => p).ToArray();
            if (_depths.Length <= 1)
            {
                return false;
            }
            return true;
        }
        
        [UsedImplicitly]
        public void Draw()
        {
            if (!CanDraw())
            {
                return;
            }

            float extraY = 22;
#if UNITY_2021_2_OR_NEWER
            extraY = 0;
#endif
            const int paddingFromEdge = 5;
            
            Rect rect = new Rect(paddingFromEdge, paddingFromEdge + extraY, 10, 10);
            GUILayout.Window(-1000, rect, DrawWindow, WindowContent);
        }

        public void DrawWindow(int id = 0)
        {
            DrawEverythingButtons();
            string[] optionsString = _depths.Select(p => p.ToString()).ToArray();
            _selectedDepthIndex = GUILayout.SelectionGrid(_selectedDepthIndex, optionsString, 1, EditorStyles.miniButton);
            TryChange();
        }

        private void TryChange()
        {
            if (_selectedDepthIndex == _prevSelectedDepthIndex)
            {
                return;
            }

            _prevSelectedDepthIndex = _selectedDepthIndex;
            if (_selectedDepthIndex == -1)
            {
                return;
            }
            
            SetDepthVisibility(p => p.WorldDepth == _depths[_selectedDepthIndex]);
            SaveToPrefs();
        }

        private void DrawEverythingButtons()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("All"))
                {
                    SetDepthVisibility(_ => true);
                    _selectedDepthIndex = -1;
                    SaveToPrefs();
                }

                if (GUILayout.Button("None"))
                {
                    SetDepthVisibility(_ => false);
                    _selectedDepthIndex = -1;
                    SaveToPrefs();
                }
            }
        }

        private delegate bool ShouldShow(LDtkComponentLevel level);
        private void SetDepthVisibility(ShouldShow decider)
        {
            List<GameObject> hide = new List<GameObject>(); 
            List<GameObject> show = new List<GameObject>();
            foreach (LDtkComponentLevel level in _levels)
            {
                if (decider.Invoke(level))
                {
                    show.Add(level.gameObject);
                    continue;
                }
                hide.Add(level.gameObject);
            }
            
            SceneVisibilityManager visibility = SceneVisibilityManager.instance;
            visibility.Hide(hide.ToArray(), true);
            visibility.DisablePicking(hide.ToArray(), true);
            
            visibility.Show(show.ToArray(), true);
            visibility.EnablePicking(show.ToArray(), true);
        }
    }
}