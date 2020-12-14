using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.EntityEvents
{
    [HelpURL(LDtkHelpURL.COMPONENT_FIELD_INJECTED_EVENT)]
    public class LDtkFieldInjectedEvent : MonoBehaviour, ILDtkFieldInjectedEvent
    {
        [SerializeField] private UnityEvent _onLDtkFieldsInjected = null;
        
        public void OnLDtkFieldsInjected()
        {
            _onLDtkFieldsInjected.Invoke();
        }
    }
}