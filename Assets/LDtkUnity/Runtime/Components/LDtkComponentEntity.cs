﻿using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    /// <summary>
    /// This component can be used to get certain LDtk information of an entity instance. Accessible from entity GameObjects.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_ENTITY)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentEntity : MonoBehaviour
    {
        [field: Tooltip("This entity's layer")]
        [field: SerializeField] public LDtkComponentLayer Parent { get; private set; }
        
        [field: Tooltip("This entity's size in unity units")]
        [field: SerializeField] public Vector2 Size { get; private set; }
        
        [field: Tooltip("The scale determined by how much the entity is resized relative to it's default size. Use for determining the length of an entity prefab, like scaling or length/size.")]
        [field: SerializeField] public Vector2 ScaleFactor { get; private set; }
        
        [field: Tooltip("Local offset from this transform to the center of this entity based on size. Used for drawing handles in the scene from the entity's center")]
        [field: SerializeField] public Vector2 MiddleCenterOffset { get; private set; }
        
        [field: Header("Redundant Fields")]
        [field: Tooltip("Grid-based coordinates")]
        [field: SerializeField] public Vector2Int Grid { get; private set; }
        
        [field: Tooltip("Entity definition identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("Pivot coordinates of the Entity")]
        [field: SerializeField] public Vector2 Pivot { get; private set; }
        
        [field: Tooltip("The entity \"smart\" color, guessed from either Entity definition, or one its field instances.")]
        [field: SerializeField] public Color SmartColor { get; private set; }
        
        [field: Tooltip("Array of tags defined in this Entity definition")]
        [field: SerializeField] public string[] Tags { get; private set; }
        
        [field: Tooltip("Optional TilesetRect used to display this entity (it could either be the default Entity tile, or some tile provided by a field value, like an Enum).")]
        [field: SerializeField] public Sprite Tile { get; private set; }
        
        [field: Tooltip("World coordinate in pixels. Only available in GridVania or Free world layouts.")]
        [field: SerializeField] public Vector2Int WorldCoord { get; private set; }
        
        [field: Header("Fields")]
        [field: Tooltip("Reference of the **Entity definition** UID")]
        [field: SerializeField] public LDtkDefinitionObjectEntity Def { get; private set; }
        
        [field: Tooltip("An array of all custom fields and their values.")]
        [field: SerializeField] public LDtkFields FieldInstances { get; private set; }
        
        [field: Tooltip("Entity size in pixels. For non-resizable entities, it will be the same as Entity definition.")]
        [field: SerializeField] public Vector2Int PxSize { get; private set; }
        
        [field: Tooltip("Unique instance identifier")]
        [field: SerializeField] public LDtkIid Iid { get; private set; }
        
        [field: Tooltip("Pixel coordinates in current level coordinate space. Don't forget optional layer offsets, if they exist!")]
        [field: SerializeField] public Vector2Int Px { get; private set; }
        
        internal void OnImport(LDtkDefinitionObjectsCache cache, EntityInstance entity, LDtkComponentLayer layer, LDtkFields fields, LDtkIid iid, Vector2 size)
        {
            Grid = entity.UnityGrid;
            Identifier = entity.Identifier;
            Pivot = entity.UnityPivot;
            SmartColor = entity.UnitySmartColor;
            Tags = entity.Tags;
            Tile = cache.GetSpriteForTilesetRectangle(entity.Tile);
            WorldCoord = entity.UnityWorld;
            Def = cache.GetObject<LDtkDefinitionObjectEntity>(entity.DefUid);
            FieldInstances = fields;
            PxSize = entity.UnityPxSize;
            Iid = iid;
            Px = entity.UnityPx;
            
            //custom
            Parent = layer;
            Size = size;
            ScaleFactor = entity.UnityScale;
            MiddleCenterOffset = LDtkCoordConverter.EntityPivotOffset(Def.Pivot, size);
        }
        
        /// <summary>
        /// The middle center of what this entity would be, factoring entity size
        /// </summary>
        public Vector3 MiddleCenter => transform.TransformPoint(MiddleCenterOffset);
        
        //todo: a fallback of 16 probably isnt right. should throw an error
        internal int PixelsPerUnit => Parent && Parent.Parent ? Parent.Parent.PixelsPerUnit : 16;
    }
}