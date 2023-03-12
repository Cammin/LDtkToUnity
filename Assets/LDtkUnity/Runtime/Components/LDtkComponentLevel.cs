using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// This component can be used to get certain LDtk information of a level. Accessible from level GameObjects.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLevel : MonoBehaviour
    {
        internal const string PROPERTY_IDENTIFIER = nameof(_identifier);
        internal const string PROPERTY_SIZE = nameof(_size);
        internal const string PROPERTY_BG_COLOR = nameof(_bgColor);
        internal const string PROPERTY_SMART_COLOR = nameof(_smartColor);
        internal const string PROPERTY_WORLD_DEPTH = nameof(_worldDepth);
        internal const string PROPERTY_NEIGHBOURS = nameof(_neighbours);

        [SerializeField] private string _identifier = string.Empty;
        [SerializeField] private Vector2 _size = Vector2.zero;
        [SerializeField] private Color _bgColor = Color.white;
        [SerializeField] private Color _smartColor = Color.white;
        [SerializeField] private int _worldDepth = 0;
        
        [SerializeField] private LDtkNeighbour[] _neighbours;

        private static readonly List<LDtkComponentLevel> Lvls = new List<LDtkComponentLevel>();
        
        /// <summary>
        /// A static collection of all active level GameObjects in the scene during runtime.<br/>
        /// This list will actively update as level GameObjects are set active/inactive.
        /// </summary>
        [PublicAPI] public static IReadOnlyCollection<LDtkComponentLevel> Levels => Lvls;

        /// <value>
        /// The size of this level in Unity units.
        /// </value>
        [PublicAPI] public Vector2 Size => _size;
        
        /// <value>
        /// The color of this level's background.
        /// </value>
        [PublicAPI] public Color BgColor => _bgColor;
        
        /// <value>
        /// The smart color of this level.
        /// </value>
        [PublicAPI] public Color SmartColor => _smartColor;
        
        /// <value>
        /// The LDtk identifier of this level.
        /// </value>
        [PublicAPI] public string Identifier => _identifier;

        /// <value>
        /// The world depth of this level.
        /// </value>
        [PublicAPI] public int WorldDepth => _worldDepth;
        
        /// <value>
        /// The world-space rectangle of this level. <br/>
        /// Useful for getting a level's bounds for a camera, for example.
        /// </value>
        [PublicAPI] public Rect BorderRect => new Rect(transform.position, _size);
        
        /// <value>
        /// The world-space bounds of this level. <br/>
        /// Useful for getting a level's bounds for a camera, for example.
        /// </value>
        [PublicAPI] public Bounds BorderBounds => new Bounds(transform.position + (Vector3)(Vector2.one * _size * 0.5f), _size);
        
        /// <value>
        /// This level's neighbours.
        /// </value>
        [PublicAPI] public IEnumerable<LDtkNeighbour> Neighbours => _neighbours;

        internal void SetIdentifier(string identifier)
        {
            _identifier = identifier;
        }

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

        internal void SetSize(Vector2 size)
        {
            _size = size;
        }
        
        internal void SetBgColor(Color bgColor, Color smartColor)
        {
            _bgColor = bgColor;
            _smartColor = smartColor;
        }
        
        internal void SetWorldDepth(int depth)
        {
            _worldDepth = depth;
        }

        internal void SetNeighbours(NeighbourLevel[] neighbours)
        {
            _neighbours = neighbours.Select(neighbour => new LDtkNeighbour(neighbour)).ToArray();
        }
    }
}
