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
        
        [SerializeField] private UnityEvent<LDtkDataLevel> _onLevelBuilt = null;

        public UnityEvent<LDtkDataLevel> OnLevelBuilt => _onLevelBuilt;

        private void OnEnable()
        {
            LDtkLevelBuilder.OnLevelBuilt += _onLevelBuilt.Invoke;
        }

        private void OnDisable()
        {
            LDtkLevelBuilder.OnLevelBuilt -= _onLevelBuilt.Invoke;
        }
    }
}