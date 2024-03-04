using System.Linq;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    public sealed class LDtkDefinitionObjectLayer : ScriptableObject
    {
        /// <summary>
        /// Type of the layer (*IntGrid, Entities, Tiles or AutoLayer*)
        /// </summary>
        [field: SerializeField] public string Type { get; private set; }
        
        /// <summary>
        /// The source IntGrid layer that this AutoLayer looks upon.
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectLayer AutoSourceLayerDef { get; private set; }
        
        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        [field: SerializeField] public float DisplayOpacity { get; private set; }
        
        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        [field: SerializeField] public int GridSize { get; private set; }
        
        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [field: SerializeField] public string Identifier { get; private set; }
        
        /// <summary>
        /// An array that defines extra optional info for each IntGrid value.<br/>  WARNING: the
        /// array order is not related to actual IntGrid values! As user can re-order IntGrid values
        /// freely, you may value "2" before value "1" in this array.
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectIntGridValue[] IntGridValues { get; private set; }

        /// <summary>
        /// Group informations for IntGrid values
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectIntGridValueGroup[] IntGridValuesGroups { get; private set; }

        /// <summary>
        /// Parallax factor (from -1 to 1 on both axis, defaults to 0) which affects the scrolling
        /// speed of this layer, creating a fake 3D (parallax) effect.
        /// </summary>
        [field: SerializeField] public Vector2 ParallaxFactor { get; private set; }

        /// <summary>
        /// If true (default), a layer with a parallax factor will also be scaled up/down accordingly.
        /// </summary>
        [field: SerializeField] public bool ParallaxScaling { get; private set; }

        /// <summary>
        /// Offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance`
        /// optional offset)
        /// </summary>
        [field: SerializeField] public Vector2Int PxOffset { get; private set; }

        /// <summary>
        /// Reference to the default Tileset UID being used by this layer definition.<br/>
        /// **WARNING**: some layer *instances* might use a different tileset. So most of the time,
        /// you should probably use the `__tilesetDefUid` value found in layer instances.<br/>  Note:
        /// since version 1.0.0, the old `autoTilesetDefUid` was removed and merged into this value.
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectTileset TilesetDef { get; private set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [field: SerializeField] public int Uid { get; private set; }
        
        #region EditorOnly

        /// <summary>
        /// Contains all the auto-layer rule definitions.
        /// </summary>
        [field: Header("Internal")]
        [field: SerializeField] public LDtkDefinitionObjectAutoLayerRuleGroup[] AutoRuleGroups { get; private set; }
        
        [field: SerializeField] public LDtkDefinitionObjectLayer AutoTilesKilledByOtherLayer { get; private set; }
        
        [field: SerializeField] public LDtkDefinitionObjectField BiomeField { get; private set; }
        
        /// <summary>
        /// Allow editor selections when the layer is not currently active.
        /// </summary>
        [field: SerializeField] public bool CanSelectWhenInactive { get; private set; }
        
        /// <summary>
        /// User defined documentation for this element to provide help/tips to level designers.
        /// </summary>
        [field: SerializeField] public string Doc { get; private set; }
        
        /// <summary>
        /// An array of tags to forbid some Entities in this layer
        /// </summary>
        [field: SerializeField] public string[] ExcludedTags { get; private set; }
        
        /// <summary>
        /// Size of the optional "guide" grid in pixels
        /// </summary>
        [field: SerializeField] public Vector2Int GuideGridSize { get; private set; }
        
        [field: SerializeField] public bool HideFieldsWhenInactive { get; private set; }
        
        /// <summary>
        /// Hide the layer from the list on the side of the editor view.
        /// </summary>
        [field: SerializeField] public bool HideInList { get; private set; }
        
        /// <summary>
        /// Alpha of this layer when it is not the active one.
        /// </summary>
        [field: SerializeField] public float InactiveOpacity { get; private set; }
        
        /// <summary>
        /// If TRUE, the content of this layer will be used when rendering levels in a simplified way
        /// for the world view
        /// </summary>
        [field: SerializeField] public bool RenderInWorldView { get; private set; }
        
        /// <summary>
        /// An array of tags to filter Entities that can be added to this layer
        /// </summary>
        [field: SerializeField] public string[] RequiredTags { get; private set; }
        
        /// <summary>
        /// If the tiles are smaller or larger than the layer grid, the pivot value will be used to
        /// position the tile relatively its grid cell.
        /// </summary>
        [field: SerializeField] public Vector2 TilePivot { get; private set; }
        
        /// <summary>
        /// Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`,
        /// `AutoLayer`
        /// </summary>
        [field: SerializeField] public TypeEnum LayerDefinitionType { get; private set; }
        
        /// <summary>
        /// User defined color for the UI
        /// </summary>
        [field: SerializeField] public Color UiColor { get; private set; }
        
        /// <summary>
        /// Display tags
        /// </summary>
        [field: SerializeField] public string[] UiFilterTags { get; private set; }

        /// <summary>
        /// Asynchronous rendering option for large/complex layers
        /// </summary>
        [field: SerializeField] public bool UseAsyncRender { get; private set; }

        #endregion
        
        internal void Populate(LDtkDefinitionObjectsCache cache, LayerDefinition def)
        {
            name = $"Layer_{def.Identifier}";
            
            Type = def.Type;
            AutoSourceLayerDef = cache.GetObject(cache.Layers, def.AutoSourceLayerDefUid);
            DisplayOpacity = def.DisplayOpacity;
            GridSize = def.GridSize;
            Identifier = def.Identifier;
            IntGridValues = def.IntGridValues.Select(p => new LDtkDefinitionObjectIntGridValue(cache, p)).ToArray();
            IntGridValuesGroups = def.IntGridValuesGroups.Select(p => cache.GetObject(cache.IntGridValueGroups, p.Uid)).ToArray();
            ParallaxFactor = def.ParallaxFactor;
            ParallaxScaling = def.ParallaxScaling;
            PxOffset = def.PxOffset;
            TilesetDef = cache.GetObject(cache.Tilesets, def.TilesetDefUid);
            Uid = def.Uid;
            
            //internal
            AutoRuleGroups = def.AutoRuleGroups.Select(p => cache.GetObject(cache.RuleGroups, p.Uid)).ToArray();
            AutoTilesKilledByOtherLayer = cache.GetObject(cache.Layers, def.AutoTilesKilledByOtherLayerUid);
            BiomeField = cache.GetObject(cache.LevelFields, def.BiomeFieldUid);
            CanSelectWhenInactive = def.CanSelectWhenInactive;
            Doc = def.Doc;
            ExcludedTags = def.ExcludedTags;
            GuideGridSize = def.GuideGridSize;
            HideFieldsWhenInactive = def.HideFieldsWhenInactive;
            HideInList = def.HideInList;
            InactiveOpacity = def.InactiveOpacity;
            RenderInWorldView = def.RenderInWorldView;
            RequiredTags = def.RequiredTags;
            TilePivot = def.TilePivot;
            LayerDefinitionType = def.LayerDefinitionType;
            UiColor = def.UnityUiColor;
            UiFilterTags = def.UiFilterTags;
            UseAsyncRender = def.UseAsyncRender;
            
            //populate ScriptableObjects that are arrays
            for (int i = 0; i < IntGridValuesGroups.Length; i++)
            {
                IntGridValuesGroups[i].Populate(cache, def.IntGridValuesGroups[i]);
            }

            for (int i = 0; i < AutoRuleGroups.Length; i++)
            {
                AutoRuleGroups[i].Populate(cache, def.AutoRuleGroups[i]);
            }
        }
    }
}