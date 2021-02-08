using LDtkUnity.Builders;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    //todo add helpurl
    public class LDtkLevelBackgroundEventReceiver : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Runtime Level Background Color Receiver";
        
        [SerializeField] private UnityEvent<Color> _onLevelBackgroundColorSet = null;

        public UnityEvent<Color> OnLevelBackgroundColorSet => _onLevelBackgroundColorSet;

        private void OnEnable()
        {
            LDtkLevelBuilder.OnLevelBackgroundColorSet += _onLevelBackgroundColorSet.Invoke;
        }

        private void OnDisable()
        {
            LDtkLevelBuilder.OnLevelBackgroundColorSet -= _onLevelBackgroundColorSet.Invoke;
        }
    }
}