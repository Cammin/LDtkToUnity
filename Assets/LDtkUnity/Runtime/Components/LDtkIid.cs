using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// This component contains a unique identifier, usable for referencing in runtime.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_IID)]
    [AddComponentMenu("")]
    public sealed class LDtkIid : MonoBehaviour
    {
        internal const string PROPERTY_IID = nameof(_iid);

        [SerializeField] private string _iid;

        /// <value>
        /// The unique identifier of this GameObject.
        /// </value>
        public string Iid => _iid;
        
        private void OnEnable()
        {
            LDtkIidComponentBank.Add(this);
        }

        private void OnDisable()
        {
            LDtkIidComponentBank.Remove(this);
        }
        
        internal void SetIid(ILDtkIid iid)
        {
            _iid = iid.Iid;
            LDtkIidComponentBank.Add(this);
        }

        public static implicit operator string(LDtkIid iid) => iid.Iid;
    }
}
