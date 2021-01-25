using System.Linq;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelFile))]
    public class LDtkLevelFileEditor : LDtkJsonFileEditor<Level>
    {
        protected override void DrawInspectorGUI(Level level)
        {
            EditorGUILayout.TextField("Identifier", level.Identifier);
            
            LayerInstance[] layers = level.LayerInstances;
            EditorGUILayout.LabelField($"{layers?.Length} Layers");

            if (layers != null)
            {
                int tileCount = layers.Where(p => p.IsIntGridLayer).SelectMany(p => p.IntGrid).Count();
                EditorGUILayout.LabelField($"{tileCount} Int Grid Values");
            }
        }
    }
}