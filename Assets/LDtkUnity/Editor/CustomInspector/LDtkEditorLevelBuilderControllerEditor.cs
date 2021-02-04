using System;
using System.Linq;
using LDtkUnity.Builders;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkEditorLevelBuilderController))]
    public class LDtkEditorLevelBuilderControllerEditor : LDtkLevelBuilderControllerEditor
    {
        private bool _toggle;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //DrawMainContent();
        }
    }
}