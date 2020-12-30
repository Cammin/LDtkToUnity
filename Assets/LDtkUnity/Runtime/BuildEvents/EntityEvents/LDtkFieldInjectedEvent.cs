using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents.EntityEvents
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_FIELD_INJECTED_EVENT)]
    public class LDtkFieldInjectedEvent : MonoBehaviour, ILDtkFieldInjectedEvent
    {
        private const string COMPONENT_NAME = "Entity Field Injection Reciever";
        
        [SerializeField] private UnityEvent _onLDtkFieldsInjected = null;
        
        public void OnLDtkFieldsInjected()
        {
            _onLDtkFieldsInjected.Invoke();
        }
    }
}