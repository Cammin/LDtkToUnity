using LDtkUnity.Builders;
using LDtkUnity.Data;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    //todo add helpurl
    public class LDtkLevelBuiltReciever : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Built Reciever";
        
        [SerializeField] private UnityEvent _onLevelBuilt = null;

        public UnityEvent OnLevelBuilt => _onLevelBuilt;

        private void OnEnable()
        {
            LDtkLevelBuilder.OnLevelBuilt += UpdateBackgroundColor;
        }

        private void OnDisable()
        {
            LDtkLevelBuilder.OnLevelBuilt -= UpdateBackgroundColor;
        }

        private void UpdateBackgroundColor(LDtkDataLevel lDtkDataLevel)
        {
            _onLevelBuilt.Invoke();
        }
    }
}