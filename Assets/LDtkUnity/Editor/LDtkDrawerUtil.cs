using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkDrawerUtil
    {
        public static void ScrollView(ref Vector2 scroll, Action draw)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            draw.Invoke();
            EditorGUILayout.EndScrollView();
        }
    }
}