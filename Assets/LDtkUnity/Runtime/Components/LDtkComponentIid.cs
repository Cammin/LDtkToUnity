using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// This component contains a unique identifier, usable for referencing in runtime.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_IID)]
    [AddComponentMenu("")]
    public class LDtkComponentIid : MonoBehaviour
    {
        internal const string PROPERTY_IID = nameof(_iid);

        [SerializeField] private string _iid;

        /// <value>
        /// The unique identifier of this GameObject.
        /// </value>
        public string Iid => _iid;
        
        private void OnEnable()
        {
            LDtkIidUnityBank.Add(this);
        }

        private void OnDisable()
        {
            LDtkIidUnityBank.Remove(this);
        }
        
        internal void SetIid(ILDtkIid iid)
        {
            _iid = iid.Iid;
            LDtkIidUnityBank.Add(this);
        }
    }
}
