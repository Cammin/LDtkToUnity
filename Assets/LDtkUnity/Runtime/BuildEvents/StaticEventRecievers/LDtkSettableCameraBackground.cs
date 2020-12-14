using System;
using LDtkUnity.Builders;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    //todo add helpurl
    public class LDtkSettableCameraBackground : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
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
            _camera.backgroundColor = bgColor;
        }
    }
}