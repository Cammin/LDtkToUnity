using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    /// <summary>
    /// This component can be used to get certain LDtk information of a level. Accessible from level GameObjects.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLevel : MonoBehaviour
    {
        #region Custom
        
        [field: Tooltip("This level's world")]
        [field: SerializeField] public LDtkComponentWorld Parent { get; private set; }
        
        [field: Tooltip("The separate level file raw json, in case you want to deserialize on your own.\nNOTE: This only exists if this is a separate level file")]
        [field: SerializeField] public LDtkLevelFile Json { get; private set; }
        
        [field: Tooltip("The size of this level in Unity units.")]
        [field: SerializeField] public Vector2 Size { get; private set; }
        
        #endregion
        
        [field: Header("Redundant Fields")]
        [field: Tooltip("An array listing all other levels touching this one on the world map. Since 1.4.0, this includes levels that overlap in the same world layer, or in nearby world layers.\n Only relevant for world layouts where level spatial positioning is manual (ie. GridVania, Free). For Horizontal and Vertical layouts, this array is always empty.")]
        [field: SerializeField] public LDtkNeighbour[] Neighbours { get; private set; }
        
        [field: Tooltip("Background color of the level. If `null`, the project `defaultLevelBgColor` should be used.")]
        [field: SerializeField] public Color LevelBgColor { get; private set; }
        
        [field: Tooltip("An enum defining the way the background image (if any) is positioned on the level. See `__bgPos` for resulting position info. Possible values: &lt;`null`&gt;, `Unscaled`, `Contain`, `Cover`, `CoverDirty`, `Repeat`")]
        [field: SerializeField] public BgPos LevelBgPos { get; private set; }
        
        [field: Header("Fields")]
        [field: Tooltip("The *optional* relative path to the level background image.")]
        [field: SerializeField] public string BgRelPath { get; private set; }
        
        [field: Tooltip("This value is not null if the project option \"*Save levels separately*\" is enabled. In this case, this **relative** path points to the level Json file.")]
        [field: SerializeField] public string ExternalRelPath { get; private set; }
        
        [field: Tooltip("An array containing this level custom field values.")]
        [field: SerializeField] public LDtkFields FieldInstances { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("Unique instance identifier")]
        [field: SerializeField] public LDtkIid Iid { get; private set; }
        
        [field: Tooltip("An array containing all Layer instances. **IMPORTANT**: if the project option \"*Save levels separately*\" is enabled, this field will be `null`.\n This array is **sorted in display order**: the 1st layer is the top-most and the last is behind.")]
        [field: SerializeField] public LDtkComponentLayer[] LayerInstances { get; private set; }
        
        [field: Tooltip("Size of the level in pixels")]
        [field: SerializeField] public Vector2Int PxSize { get; private set; }
        
        [field: Tooltip("")]
        [field: SerializeField] public int Uid { get; private set; }
        
        [field: Tooltip("Index that represents the \"depth\" of the level in the world. Default is 0, greater means \"above\", lower means \"below\".\nThis value is mostly used for display only and is intended to make stacking of levels easier to manage.")]
        [field: SerializeField] public int WorldDepth { get; private set; }
        
        [field: Tooltip("World coordinate in pixels.\nOnly relevant for world layouts where level spatial positioning is manual (ie. GridVania, Free). For Horizontal and Vertical layouts, the value is always -1 here.")]
        [field: SerializeField] public Vector2Int WorldCoord { get; private set; }

        #region Internal
        
        [field: Header("Internal")]
        [field: Tooltip("The \"guessed\" color for this level in the editor, decided using either the background color or an existing custom field.")]
        [field: SerializeField] public Color SmartColor { get; private set; }
        
        [field: Tooltip("Background color of the level (same as `bgColor`, except the default value is automatically used here if its value is `null`)")]
        [field: SerializeField] public Color BgColor { get; private set; }
        
        [field: Tooltip("Background image pivot")]
        [field: SerializeField] public Vector2 BgPivot { get; private set; }
        
        [field: Tooltip("Position informations of the background image, if there is one.")]
        [field: SerializeField] public LevelBackgroundPosition BgPos { get; private set; }
        
        [field: Tooltip("If TRUE, the level identifier will always automatically use the naming pattern as defined in `Project.levelNamePattern`. Becomes FALSE if the identifier is manually modified by user.")]
        [field: SerializeField] public bool UseAutoIdentifier { get; private set; }

        #endregion

        #region Runtime

        private static readonly List<LDtkComponentLevel> Lvls = new List<LDtkComponentLevel>();
        
        /// <summary>
        /// A static collection of all active level GameObjects in the scene during runtime.<br/>
        /// This list will actively update as level GameObjects are set active/inactive.
        /// </summary>
        [PublicAPI] public static IReadOnlyCollection<LDtkComponentLevel> Levels => Lvls;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatically()
        {
            Lvls.Clear();
        }

        private void OnEnable()
        {
            Lvls.Add(this);
        }

        private void OnDisable()
        {
            Lvls.Remove(this);
        }

        #endregion

        internal void OnImport(Level lvl, LDtkLevelFile file, LDtkComponentLayer[] layers, LDtkFields fields, LDtkComponentWorld world, Vector2 unitySize, LDtkIid iid)
        {
            Neighbours = lvl.Neighbours.Select(neighbour => new LDtkNeighbour(neighbour)).ToArray();
            LevelBgColor = lvl.UnityLevelBgColor;
            //LevelBgPos = lvl.LevelBgPos; //todo
            BgRelPath = lvl.BgRelPath;
            ExternalRelPath = lvl.ExternalRelPath;
            FieldInstances = fields;
            Identifier = lvl.Identifier;
            Iid = iid;
            LayerInstances = layers;
            PxSize = lvl.UnityPxSize;
            Uid = lvl.Uid;
            WorldDepth = lvl.WorldDepth;
            WorldCoord = lvl.UnityWorldCoord;
            
            //internal
            SmartColor = lvl.UnitySmartColor;
            BgColor = lvl.UnityBgColor;
            BgPivot = lvl.UnityBgPivot;
            BgPos = lvl.BgPos;
            UseAutoIdentifier = lvl.UseAutoIdentifier;
            
            //custom
            Size = unitySize;
            Json = file;
            Parent = world;
        }
        
        /// <value>
        /// The world-space rectangle of this level. <br/>
        /// Useful for getting a level's bounds for a camera, for example.
        /// </value>
        [PublicAPI] public Rect BorderRect => new Rect(transform.position, Size);
        
        /// <value>
        /// The world-space bounds of this level. <br/>
        /// Useful for getting a level's bounds for a camera, for example.
        /// </value>
        [PublicAPI] public Bounds BorderBounds => new Bounds(transform.position + (Vector3)(Vector2.one * Size * 0.5f), Size);
    }
}
