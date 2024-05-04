using System.Linq;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LAYER_DEF_JSON)]
    public sealed class LDtkDefinitionObjectLayer : LDtkDefinitionObject<LayerDefinition>, ILDtkUid
    {
        [field: Tooltip("Type of the layer (*IntGrid, Entities, Tiles or AutoLayer*)")]
        [field: SerializeField] public string Type { get; private set; }
        
        [field: Tooltip("The source IntGrid layer that this AutoLayer looks upon.")]
        [field: SerializeField] public LDtkDefinitionObjectLayer AutoSourceLayerDef { get; private set; }
        
        [field: Tooltip("Opacity of the layer (0 to 1.0)")]
        [field: SerializeField] public float DisplayOpacity { get; private set; }
        
        [field: Tooltip("Width and height of the grid in pixels")]
        [field: SerializeField] public int GridSize { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("An array that defines extra optional info for each IntGrid value. WARNING: the array order is not related to actual IntGrid values! As user can re-order IntGrid values freely, you may value \"2\" before value \"1\" in this array.")]
        [field: SerializeField] public LDtkDefinitionObjectIntGridValue[] IntGridValues { get; private set; }
        
        [field: Tooltip("Group informations for IntGrid values")]
        [field: SerializeField] public LDtkDefinitionObjectIntGridValueGroup[] IntGridValuesGroups { get; private set; }
        
        [field: Tooltip("Parallax factor (from -1 to 1 on both axis, defaults to 0) which affects the scrolling speed of this layer, creating a fake 3D (parallax) effect.")]
        [field: SerializeField] public Vector2 ParallaxFactor { get; private set; }
        
        [field: Tooltip("If true (default), a layer with a parallax factor will also be scaled up/down accordingly.")]
        [field: SerializeField] public bool ParallaxScaling { get; private set; }
        
        [field: Tooltip("Offset of the layer, in pixels (IMPORTANT: this should be added to the `LayerInstance` optional offset)")]
        [field: SerializeField] public Vector2Int PxOffset { get; private set; }
        
        [field: Tooltip("Reference to the default Tileset UID being used by this layer definition. **WARNING**: some layer *instances* might use a different tileset. So most of the time, you should probably use the `__tilesetDefUid` value found in layer instances.<br/>  Note: since version 1.0.0, the old `autoTilesetDefUid` was removed and merged into this value.")]
        [field: SerializeField] public LDtkDefinitionObjectTileset TilesetDef { get; private set; }
        
        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }
        
        #region EditorOnly
        
        [field: Header("Internal")]
        [field: Tooltip("Contains all the auto-layer rule definitions.")]
        [field: SerializeField] public LDtkDefinitionObjectAutoLayerRuleGroup[] AutoRuleGroups { get; private set; }
        
        [field: SerializeField] public LDtkDefinitionObjectLayer AutoTilesKilledByOtherLayer { get; private set; }
        
        [field: SerializeField] public LDtkDefinitionObjectField BiomeField { get; private set; }
        
        [field: Tooltip("Allow editor selections when the layer is not currently active.")]
        [field: SerializeField] public bool CanSelectWhenInactive { get; private set; }
        
        [field: Tooltip("User defined documentation for this element to provide help/tips to level designers.")]
        [field: SerializeField] public string Doc { get; private set; }
        
        [field: Tooltip("An array of tags to forbid some Entities in this layer")]
        [field: SerializeField] public string[] ExcludedTags { get; private set; }
        
        [field: Tooltip("Size of the optional \"guide\" grid in pixels")]
        [field: SerializeField] public Vector2Int GuideGridSize { get; private set; }
        
        [field: SerializeField] public bool HideFieldsWhenInactive { get; private set; }
        
        [field: Tooltip("Hide the layer from the list on the side of the editor view.")]
        [field: SerializeField] public bool HideInList { get; private set; }
        
        [field: Tooltip("Alpha of this layer when it is not the active one.")]
        [field: SerializeField] public float InactiveOpacity { get; private set; }
        
        [field: Tooltip("If TRUE, the content of this layer will be used when rendering levels in a simplified way for the world view")]
        [field: SerializeField] public bool RenderInWorldView { get; private set; }
        
        [field: Tooltip("An array of tags to filter Entities that can be added to this layer")]
        [field: SerializeField] public string[] RequiredTags { get; private set; }
        
        [field: Tooltip("If the tiles are smaller or larger than the layer grid, the pivot value will be used to position the tile relatively its grid cell.")]
        [field: SerializeField] public Vector2 TilePivot { get; private set; }
        
        [field: Tooltip("Type of the layer as Haxe Enum Possible values: `IntGrid`, `Entities`, `Tiles`, `AutoLayer`")]
        [field: SerializeField] public TypeEnum LayerDefinitionType { get; private set; }
        
        [field: Tooltip("User defined color for the UI")]
        [field: SerializeField] public Color UiColor { get; private set; }
        
        [field: Tooltip("Display tags")]
        [field: SerializeField] public string[] UiFilterTags { get; private set; }
        
        [field: Tooltip("Asynchronous rendering option for large/complex layers")]
        [field: SerializeField] public bool UseAsyncRender { get; private set; }

        #endregion
        
        internal override void SetAssetName()
        {
            name = $"Layer_{Uid}_{Identifier}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, LayerDefinition def)
        {
            Type = def.Type;
            AutoSourceLayerDef = cache.GetObject<LDtkDefinitionObjectLayer>(def.AutoSourceLayerDefUid);
            DisplayOpacity = def.DisplayOpacity;
            GridSize = def.GridSize;
            Identifier = def.Identifier;
            IntGridValues = def.IntGridValues.Select(p => new LDtkDefinitionObjectIntGridValue()).ToArray();
            IntGridValuesGroups = def.IntGridValuesGroups.Select(p => new LDtkDefinitionObjectIntGridValueGroup()).ToArray();
            ParallaxFactor = def.ParallaxFactor;
            ParallaxScaling = def.ParallaxScaling;
            PxOffset = def.PxOffset;
            TilesetDef = cache.GetObject<LDtkDefinitionObjectTileset>(def.TilesetDefUid);
            Uid = def.Uid;
            
            //internal
            AutoRuleGroups = def.AutoRuleGroups.Select(p => cache.GetObject<LDtkDefinitionObjectAutoLayerRuleGroup>(p.Uid)).ToArray();
            AutoTilesKilledByOtherLayer = cache.GetObject<LDtkDefinitionObjectLayer>(def.AutoTilesKilledByOtherLayerUid);
            BiomeField = cache.GetObject<LDtkDefinitionObjectField>(def.BiomeFieldUid);
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
            
            for (int i = 0; i < IntGridValues.Length; i++)
            {
                IntGridValues[i].Populate(cache, def.IntGridValues[i]);
            }

            for (int i = 0; i < AutoRuleGroups.Length; i++)
            {
                AutoRuleGroups[i].Populate(cache, def.AutoRuleGroups[i]);
            }
        }

        public LDtkDefinitionObjectIntGridValueGroup GetGroupOfValue(int value)
        {
            LDtkDefinitionObjectIntGridValue valueObj = IntGridValues.FirstOrDefault(p => p.Value == value);
            if (valueObj == null)
            {
                LDtkDebug.LogError($"Didn't GetGroupOfValue because the value of \"{value}\" doesn't exist");
                return null;
            }
            
            int uid = valueObj.GroupUid;
            return IntGridValuesGroups.FirstOrDefault(p => p.Uid == uid);
        } 
    }
}