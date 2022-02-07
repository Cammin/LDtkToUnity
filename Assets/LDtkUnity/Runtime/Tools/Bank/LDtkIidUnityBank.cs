using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Unlike <see cref="LDtkIidBank"/>, this contains components in the scene, and used for referencing in runtime.<br/>
    /// This is also fully accessible during the import process.
    /// Note: If there are duplicated instances of a GameObject with an iid component, it may result in inconsistent references.
    /// </summary>
    public static class LDtkIidUnityBank
    {
        private static readonly Dictionary<string, LDtkComponentIid> IidObjects = new Dictionary<string, LDtkComponentIid>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void Release()
        {
            IidObjects.Clear();
        }
        
        internal static void Add(LDtkComponentIid iid)
        {
            if (!IidObjects.ContainsKey(iid.Iid))
            {
                IidObjects.Add(iid.Iid, iid);
            }
        }
        
        internal static void Remove(LDtkComponentIid iid)
        {
            if (IidObjects.ContainsKey(iid.Iid))
            {
                IidObjects.Remove(iid.Iid);
            }
        }
        
        /// <summary>
        /// Gets a uid GameObject. The objects are only available after their OnEnable. Otherwise, try using <see cref="FindObjectOfIid"/>. <br/>
        /// If the component was not found, then make sure the referenced component is active and accessed after it's OnEnable.
        /// </summary>
        /// <param name="iid">
        /// The iid value.
        /// </param>
        /// <returns>
        /// The iid component that matches the iid.
        /// </returns>
        [PublicAPI]
        public static LDtkComponentIid GetByIid(string iid)
        {
            if (!IidObjects.ContainsKey(iid))
            {
                return null;
            }
            
            LDtkComponentIid iidComponent = IidObjects[iid];
            if (iidComponent != null)
            {
                return iidComponent;
            }
                
            Debug.LogError($"LDtk: Found a key for iid \"{iid}\" but was null");
            return null;
        }
        
        /// <summary>
        /// Finds an iid component.<br/>
        /// This function uses Object.FindObjectsOfType, so it is slow and not recommended to use every frame. <br/>
        /// In most cases you can use <see cref="GetByIid"/> instead.
        /// </summary>
        /// <returns>
        /// The iid component that matches the iid.
        /// </returns>
        [PublicAPI]
        public static LDtkComponentIid FindObjectOfIid(string iid)
        {
            LDtkComponentIid[] iidComponents = Object.FindObjectsOfType<LDtkComponentIid>(true);
            LDtkComponentIid iidComponent = iidComponents.FirstOrDefault(p => p.Iid == iid);
            return iidComponent;
        }
    }
}