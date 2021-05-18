using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_FIELD_INJECTED_EVENT)]
    public class LDtkImportEventReciever : MonoBehaviour, ILDtkImportEvent
    {
        private const string COMPONENT_NAME = "Field Reciever";
        
        [SerializeField] private UnityEvent _onLDtkFieldsInjected = null;
        
        public void OnLDtkImport()
        {
            _onLDtkFieldsInjected.Invoke();
        }
    }
}