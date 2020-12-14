using System;
using LDtkUnity.Builders;
using LDtkUnity.Data;
using UnityEngine;
using UnityEngine.Events;

namespace LDtkUnity.BuildEvents
{
    //todo add helpurl
    public class LDtkLevelBuiltReciever : MonoBehaviour
    {
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