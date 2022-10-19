using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkPostProcessorCache
    {
        private readonly List<LDtkPostProcessorAction> _postprocessActions = new List<LDtkPostProcessorAction>();
        
        public void AddPostProcessAction(int order, Action action, string debugInfo)
        {
            _postprocessActions.Add(new LDtkPostProcessorAction()
            {
                Action = action,
                Order = order,
                DebugInfo = debugInfo,
            });
        }
        
        public void TryAddInterfaceEvent<T>(MonoBehaviour[] behaviors, Action<T> action) where T : ILDtkImported
        {
            foreach (MonoBehaviour component in behaviors)
            {
                if (component is T imported)
                {
                    AddPostProcessAction(
                        imported.GetPostprocessOrder(), 
                        () => { action.Invoke(imported); },
                        $"Interface\t<{typeof(T).Name}>\t({component.gameObject.name})");
                }
            }
        }
        
        public void PostProcess()
        {
            if (_postprocessActions == null)
            {
                LDtkDebug.LogError("LDtkPostProcessorCache not initialized first");
                return;
            }
            
            //sort everything to that execution is based on the user's custom inputs
            _postprocessActions.Sort();

            if (LDtkPrefs.VerboseLogging)
            {
                foreach (LDtkPostProcessorAction action in _postprocessActions)
                {
                    LDtkDebug.Log($"Postprocess: {action}");
                }
            }
            
            foreach (LDtkPostProcessorAction action in _postprocessActions)
            {
                try
                {
                    action.Action?.Invoke();
                }
                catch (Exception e)
                {
                    LDtkDebug.LogError(e.ToString());
                }
            }
        }
    }
}