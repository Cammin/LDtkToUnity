using System;
using System.Collections.Generic;

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
                LDtkDebug.LogError("LDtkPostProcessorCache not initialized first");
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
                    LDtkDebug.LogError(e.ToString());
                }
            }
        }
    }
}