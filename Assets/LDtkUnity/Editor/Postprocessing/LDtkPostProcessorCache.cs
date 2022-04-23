using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkPostProcessorCache
    {
        private readonly List<Action> _postprocessActions = new List<Action>();
        
        public void AddPostProcessAction(Action action)
        {
            _postprocessActions.Add(action);
        }
        
        public void PostProcess()
        {
            if (_postprocessActions == null)
            {
                Debug.LogError("LDtk: LDtkPostProcessorCache not initialized first");
                return;
            }
            
            foreach (Action action in _postprocessActions)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}