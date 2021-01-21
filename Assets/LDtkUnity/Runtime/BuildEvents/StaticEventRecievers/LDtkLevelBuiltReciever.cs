using LDtkUnity.Builders;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    //todo add helpurl
    public class LDtkLevelBuiltReciever : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Built Reciever";
        
        [SerializeField] private UnityEvent<Level> _onLevelBuilt = null;

        public UnityEvent<Level> OnLevelBuilt => _onLevelBuilt;

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