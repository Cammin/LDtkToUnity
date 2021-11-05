using System;
using System.Collections.Generic;
using LDtkUnity.Editor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public class LDtkPostProcessorCache
    {
        private class ProjectContext
        {
            private GameObject _projectObj;
            private LdtkJson _project;

            public void Apply()
            {
                //LDtkPostProcessorInvoker.PostProcessProject(_projectObj, _project);
            }
        }
        private class LevelContext
        {
            private GameObject _levelObj;
            private Level _level;
            
            public void Apply()
            {
                //LDtkPostProcessorInvoker.PostProcessLevel(_levelObj, _level);
            }
            
        }
        private class BackgroundContext
        {
            private GameObject _backgroundObj;
            
            public void Apply()
            {
                //LDtkPostProcessorInvoker.PostProcessBackgroundColor(_backgroundObj);
            }
            
        }
        private class EntityContext
        {
            private GameObject _entityObj;
            private EntityInstance _entity;
            
            public void Apply()
            {
                //LDtkPostProcessorInvoker.PostProcessEntity(_entityObj, _entity);
            }
            
        }
        private class IntGridLayerContext
        {
            private GameObject _layerObj;
            private LayerInstance _layer;
            private Tilemap[] _tilemaps;
            
            public void Apply()
            {
                //LDtkPostProcessorInvoker.PostProcessIntGridLayer(_layerObj, _layer, _tilemaps);
            }
            
        }
        private class AutoLayerContext
        {
            private GameObject _layerObj;
            private LayerInstance _layer;
            private Tilemap[] _tilemaps;
            
            public void Apply()
            {
                //LDtkPostProcessorInvoker.PostProcessAutoLayer(_layerObj, _layer, _tilemaps);
            }
            
        }

        private static List<Action> _postprocessActions;

        public static void Initialize()
        {
            _postprocessActions = new List<Action>();
        }
        
        public static void AddPostProcessAction(Action action)
        {
            if (_postprocessActions == null)
            {
                Debug.LogError("LDtk: LDtkPostProcessorCache not initialized first");
                return;
            }
            
            _postprocessActions.Add(action);
        }
        
        public static void PostProcess()
        {
            if (_postprocessActions == null)
            {
                Debug.LogError("LDtk: LDtkPostProcessorCache not initialized first");
                return;
            }
            
            foreach (Action action in _postprocessActions)
            {
                action?.Invoke();
            }
            
            //once finished, dispose of memory
            _postprocessActions = null;
        }
    }
}