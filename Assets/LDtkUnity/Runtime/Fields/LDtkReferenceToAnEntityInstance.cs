using System;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// This object describes the "location" of an Entity instance in the project worlds.
    /// </summary>
    [Serializable]
    public class LDtkReferenceToAnEntityInstance
    {
        internal const string PROPERTY_ENTITY = nameof(_entityIid);
        internal const string PROPERTY_LAYER = nameof(_layerIid);
        internal const string PROPERTY_LEVEL = nameof(_levelIid);
        internal const string PROPERTY_WORLD = nameof(_worldIid);
        
        [SerializeField] private string _entityIid;
        [SerializeField] private string _layerIid;
        [SerializeField] private string _levelIid;
        [SerializeField] private string _worldIid;

        public string EntityIid => _entityIid;
        public string LayerIid => _layerIid;
        public string LevelIid => _levelIid;
        public string WorldIid => _worldIid;

        internal LDtkReferenceToAnEntityInstance(ReferenceToAnEntityInstance reference)
        {
            _entityIid = reference.EntityIid;
            _layerIid = reference.LayerIid;
            _levelIid = reference.LevelIid;
            _worldIid = reference.WorldIid;
        }

        /// <summary>
        /// Finds the iid Entity GameObject.
        /// </summary>
        /// <returns>
        /// The iid component of this entity reference
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame. However if the object is found, it is cached. <br/>
        /// In most cases you can use <see cref="GetEntity"/> instead as long as the object you are looking for is active. If the object you are looking for is inactive, you can try this.
        /// </remarks>
        public LDtkIid FindEntity() => LDtkIidComponentBank.FindObjectOfIid(_entityIid);
        /// <summary>
        /// Finds the iid Layer GameObject.
        /// </summary>
        /// <returns>
        /// The iid component of this entity reference
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame. However if the object is found, it is cached. <br/>
        /// In most cases you can use <see cref="GetLayer"/> instead as long as the object you are looking for is active. If the object you are looking for is inactive, you can try this.
        /// </remarks>
        public LDtkIid FindLayer() => LDtkIidComponentBank.FindObjectOfIid(_layerIid);
        /// <summary>
        /// Finds the iid Level GameObject.
        /// </summary>
        /// <returns>
        /// The iid component of this entity reference
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame. However if the object is found, it is cached. <br/>
        /// In most cases you can use <see cref="GetLevel"/> instead as long as the object you are looking for is active. If the object you are looking for is inactive, you can try this.
        /// </remarks>
        public LDtkIid FindLevel() => LDtkIidComponentBank.FindObjectOfIid(_levelIid);
        /// <summary>
        /// Finds the iid World GameObject.
        /// </summary>
        /// <returns>
        /// The iid component of this entity reference
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame. However if the object is found, it is cached. <br/>
        /// In most cases you can use <see cref="GetWorld"/> instead as long as the object you are looking for is active. If the object you are looking for is inactive, you can try this.
        /// </remarks>
        public LDtkIid FindWorld() => LDtkIidComponentBank.FindObjectOfIid(_worldIid);
        
        /// <summary>
        /// Gets a iid GameObject.
        /// </summary>
        /// <returns>
        /// The iid component that matches the iid.
        /// </returns>
        /// <remarks>
        /// The objects are only available after their OnEnable. Otherwise, try using <see cref="FindEntity"/>. <br/>
        /// If the iid component exists but this returned null, then make sure the referenced component is active and accessed after it's OnEnable.
        /// </remarks>
        public LDtkIid GetEntity() => LDtkIidComponentBank.GetByIid(_entityIid);
        /// <summary>
        /// Gets a iid GameObject.
        /// </summary>
        /// <returns>
        /// The iid component that matches the iid.
        /// </returns>
        /// <remarks>
        /// The objects are only available after their OnEnable. Otherwise, try using <see cref="FindLayer"/>. <br/>
        /// If the iid component exists but this returned null, then make sure the referenced component is active and accessed after it's OnEnable.
        /// </remarks>
        public LDtkIid GetLayer() => LDtkIidComponentBank.GetByIid(_layerIid);
        /// <summary>
        /// Gets a iid GameObject.
        /// </summary>
        /// <returns>
        /// The iid component that matches the iid.
        /// </returns>
        /// <remarks>
        /// The objects are only available after their OnEnable. Otherwise, try using <see cref="FindLevel"/>. <br/>
        /// If the iid component exists but this returned null, then make sure the referenced component is active and accessed after it's OnEnable.
        /// </remarks>
        public LDtkIid GetLevel() => LDtkIidComponentBank.GetByIid(_levelIid);
        /// <summary>
        /// Gets a iid GameObject.
        /// </summary>
        /// <returns>
        /// The iid component that matches the iid.
        /// </returns>
        /// <remarks>
        /// The objects are only available after their OnEnable. Otherwise, try using <see cref="FindWorld"/>. <br/>
        /// If the iid component exists but this returned null, then make sure the referenced component is active and accessed after it's OnEnable.
        /// </remarks>
        public LDtkIid GetWorld() => LDtkIidComponentBank.GetByIid(_worldIid);
    }
}