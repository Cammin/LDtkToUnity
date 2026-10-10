using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkTilemapRenderer))]
    [CanEditMultipleObjects]
    internal sealed class LDtkTilemapRendererEditor : UnityEditor.Editor
    {
        private static readonly GUIContent OrderInLayerContent = new GUIContent("Order in Layer", "The sortingOrder of the renderer");
        private static readonly GUIContent OrderingsContent = new GUIContent("Orderings", "The order of tiles to internally render in. This enforces tile order to match LDtk exactly.");
        
        private SerializedProperty _propTilemap;
        private SerializedProperty _propFilter;
        private SerializedProperty _propRenderer;
        private SerializedProperty _propOrderings;
        
        private SerializedObject _rendererObj;
        private SerializedProperty _rendererPropSortingLayerId;
        private SerializedProperty _rendererPropSortingOrder;
        
        private LDtkTilemapRenderer _renderer;
        private LDtkSortingLayerAttributeDrawer _sortingLayerDrawer;
        
        private void OnEnable()
        {
            _renderer = target as LDtkTilemapRenderer;
            _sortingLayerDrawer = new LDtkSortingLayerAttributeDrawer();
            
            _propTilemap = serializedObject.FindProperty(LDtkTilemapRenderer.TILEMAP);
            _propFilter = serializedObject.FindProperty(LDtkTilemapRenderer.FILTER);
            _propRenderer = serializedObject.FindProperty(LDtkTilemapRenderer.RENDERER);
            _propOrderings = serializedObject.FindProperty(LDtkTilemapRenderer.ORDERINGS);
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.HelpBox($"Multi-object editing not supported for LDtkTilemapRenderer", MessageType.Warning);
                return;
            }
            
            if (!_renderer.EnsureRenderer())
            {
                EditorGUILayout.HelpBox($"This GameObject has no renderer", MessageType.Error);
                return;
            }
            
            if (_rendererObj == null)
            {
                _rendererObj = new SerializedObject(_renderer._renderer);
                _rendererPropSortingLayerId = _rendererObj.FindProperty("m_SortingLayerID");
                _rendererPropSortingOrder = _rendererObj.FindProperty("m_SortingOrder");
            }
            
            _rendererObj.Update();
            _sortingLayerDrawer.DrawProperty(_rendererPropSortingLayerId);
            EditorGUILayout.PropertyField(_rendererPropSortingOrder, OrderInLayerContent);
            _rendererObj.ApplyModifiedProperties();
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(_propOrderings, OrderingsContent);
            serializedObject.ApplyModifiedProperties(); 
        }
    }
}