using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkPostProcessorCache
    {
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