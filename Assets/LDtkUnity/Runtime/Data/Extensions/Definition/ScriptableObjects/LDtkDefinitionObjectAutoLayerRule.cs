using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_AUTO_RULE_DEF)]
    public class LDtkDefinitionObjectAutoLayerRule : LDtkDefinitionObject<AutoLayerRuleDefinition>, ILDtkUid
    {
        [field: Header("Internal")]
        [field: Tooltip("If FALSE, the rule effect isn't applied, and no tiles are generated.")]
        [field: SerializeField] public bool Active { get; private set; }

        [field: SerializeField] public float Alpha { get; private set; }
        
        [field: Tooltip("When TRUE, the rule will prevent other rules to be applied in the same cell if it matches (TRUE by default).")]
        [field: SerializeField] public bool BreakOnMatch { get; private set; }
        
        [field: Tooltip("Chances for this rule to be applied (0 to 1)")]
        [field: SerializeField] public float Chance { get; private set; }
        
        [field: Tooltip("Checker mode Possible values: `None`, `Horizontal`, `Vertical`")]
        [field: SerializeField] public Checker Checker { get; private set; }
        
        [field: Tooltip("If TRUE, allow rule to be matched by flipping its pattern horizontally")]
        [field: SerializeField] public bool FlipX { get; private set; }
        
        [field: Tooltip("If TRUE, allow rule to be matched by flipping its pattern vertically")]
        [field: SerializeField] public bool FlipY { get; private set; }
        
        [field: Tooltip("If TRUE, then the rule should be re-evaluated by the editor at one point")]
        [field: SerializeField] public bool Invalidated { get; private set; }
        
        [field: Tooltip("Default IntGrid value when checking cells outside of level bounds")]
        [field: SerializeField] public int OutOfBoundsValue { get; private set; }
        
        [field: Tooltip("Rule pattern (size x size)")]
        [field: SerializeField] public int[] Pattern { get; private set; }
        
        [field: Tooltip("If TRUE, enable Perlin filtering to only apply rule on specific random area")]
        [field: SerializeField] public bool PerlinActive { get; private set; }

        [field: SerializeField] public float PerlinOctaves { get; private set; }

        [field: SerializeField] public float PerlinScale { get; private set; }

        [field: SerializeField] public float PerlinSeed { get; private set; }
        
        [field: Tooltip("Pivot of a tile stamp (0-1 both axis)")]
        [field: SerializeField] public Vector2 Pivot { get; private set; }
        
        [field: Tooltip("Pattern width and height. Should only be 1,3,5 or 7.")]
        [field: SerializeField] public int Size { get; private set; }
        
        [field: Tooltip("Defines how tileIds array is used Possible values: `Single`, `Stamp`")]
        [field: SerializeField] public TileMode TileMode { get; private set; }
        
        [field: Tooltip("Max random offset for tile pos")]
        [field: SerializeField] public Vector2Int TileRandomMax { get; private set; }
        
        [field: Tooltip("Min random offset for tile pos")]
        [field: SerializeField] public Vector2Int TileRandomMin { get; private set; }
        
        [field: Tooltip("Array containing all the possible tile IDs rectangles (picked randomly).")]
        [field: SerializeField] public int[][] TileRectsIds { get; private set; }

        [field: Tooltip("Tile offset")]
        [field: SerializeField] public Vector2Int TileOffset { get; private set; }

        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }

        [field: Tooltip("Cell coord modulo")]
        [field: SerializeField] public Vector2Int Modulo { get; private set; }
        
        [field: Tooltip("Cell start offset")]
        [field: SerializeField] public Vector2Int Offset { get; private set; }
        
        internal override void SetAssetName()
        {
            name = $"Rule_{Uid}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, AutoLayerRuleDefinition def)
        {
            Active = def.Active;
            Alpha = def.Alpha;
            BreakOnMatch = def.BreakOnMatch;
            Chance = def.Chance;
            Checker = def.Checker;
            FlipX = def.FlipX;
            FlipY = def.FlipY;
            Invalidated = def.Invalidated;
            OutOfBoundsValue = def.OutOfBoundsValue != null ? def.OutOfBoundsValue.Value : -1;
            Pattern = def.Pattern;
            PerlinActive = def.PerlinActive;
            PerlinOctaves = def.PerlinOctaves;
            PerlinScale = def.PerlinScale;
            PerlinSeed = def.PerlinSeed;
            Pivot = def.UnityPivot;
            Size = def.Size;
            TileMode = def.TileMode;
            TileRandomMax = def.UnityTileRandomMax;
            TileRandomMin = def.UnityTileRandomMin;
            TileRectsIds = def.TileRectsIds; //todo make serializable. tileset tiles
            TileOffset = def.UnityOffset;
            Uid = def.Uid;
            Modulo = def.UnityModulo;
            Offset = def.UnityOffset;
        }
    }
}