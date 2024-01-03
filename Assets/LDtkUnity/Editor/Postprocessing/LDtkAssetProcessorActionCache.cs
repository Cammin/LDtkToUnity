using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// An object that would be stored somewhere to accumulate actions, and then run them all later.
    /// The stored actions are sorted by order before  </summary>
    internal sealed class LDtkAssetProcessorActionCache
    {
        private readonly List<LDtkAssetProcessorAction> _assetProcessActions = new List<LDtkAssetProcessorAction>();
        
        public void AddProcessAction(int order, Action action, string debugInfo)
        {
            _assetProcessActions.Add(new LDtkAssetProcessorAction()
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
                    AddProcessAction(
                        imported.GetPostprocessOrder(), 
                        () => { action.Invoke(imported); },
                        $"Interface\t<{typeof(T).Name}>\t({component.gameObject.name})");
                }
            }
        }
        
        public void Process()
        {
            if (_assetProcessActions == null)
            {
                LDtkDebug.LogError("LDtkPostProcessorCache not initialized first");
                return;
            }
            
            //sort everything to that execution is based on the user's custom inputs
            _assetProcessActions.Sort();

            if (LDtkPrefs.VerboseLogging)
            {
                foreach (LDtkAssetProcessorAction action in _assetProcessActions)
                {
                    LDtkDebug.Log($"Process: {action}");
                }
            }
            
            foreach (LDtkAssetProcessorAction action in _assetProcessActions)
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
    
    internal sealed class LDtkAssetProcessorAction : IComparable<LDtkAssetProcessorAction>
    {
        public int Order = 0;
        public Action Action;
        public string DebugInfo;

        public int CompareTo(LDtkAssetProcessorAction other)
        {
            return Order.CompareTo(other.Order);
        }

        public override string ToString()
        {
            return $"{Order}\t{DebugInfo}";
        }
    }
}