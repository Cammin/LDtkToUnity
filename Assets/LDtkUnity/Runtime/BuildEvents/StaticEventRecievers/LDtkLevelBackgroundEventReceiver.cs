using LDtkUnity.Builders;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    //todo add helpurl
    public class LDtkLevelBackgroundEventReceiver : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Level Background Color Receiver";
        
        [SerializeField] private UnityEvent<Color> _onLevelBackgroundColorSet = null;

        public UnityEvent<Color> OnLevelBackgroundColorSet => _onLevelBackgroundColorSet;

        private void OnEnable()
        {
            LDtkLevelBuilder.OnLevelBackgroundColorSet += UpdateBackgroundColor;
        }

        private void OnDisable()
        {
            LDtkLevelBuilder.OnLevelBackgroundColorSet -= UpdateBackgroundColor;
        }

        private void UpdateBackgroundColor(Color bgColor)
        {
            _onLevelBackgroundColorSet.Invoke(bgColor);
        }
    }
}