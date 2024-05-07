using System;
using LDtkUnity;
using UnityEngine;
using UnityEngine.Assertions;

namespace Tests.Misc.TestEntityImportInterface
{
    public class TestEntityAssert : MonoBehaviour, ILDtkImportedFields
    {
        public string _identifier = "";
        public void OnLDtkImportFields(LDtkFields fields)
        {
            if (string.IsNullOrEmpty(_identifier))
            {
                Debug.LogError($"no identifier check for {gameObject.name}");
                return;
            }

            if (fields == null)
            {
                Debug.LogError($"fields NULL for {gameObject.name}?");
                return;
            }
            
            Debug.Assert(fields.gameObject.name.Contains(_identifier), $"{fields.gameObject.name} does not have {_identifier}");
        }
    }
}
