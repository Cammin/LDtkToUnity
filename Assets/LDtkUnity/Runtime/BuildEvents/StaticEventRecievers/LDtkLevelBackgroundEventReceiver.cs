using System;
using LDtkUnity.Builders;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    //todo add helpurl
    public class LDtkLevelBackgroundEventReceiver : MonoBehaviour
    {
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