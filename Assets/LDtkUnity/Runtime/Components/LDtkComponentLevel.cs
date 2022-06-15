using System.Collections.Generic;
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
        [SerializeField] private string _identifier = string.Empty;
        [SerializeField] private Vector2 _size = Vector2.zero;
        [SerializeField] private Color _bgColor = Color.white;
        [SerializeField] private Color _smartColor = Color.white;
        [SerializeField] private int _worldDepth;

        private static List<LDtkComponentLevel> _levels = new List<LDtkComponentLevel>();
        
        /// <summary>
        /// A static collection of all active level GameObjects in the scene during runtime.<br/>
        /// This list will actively update as level GameObjects are set active/inactive.
        /// </summary>
        public static IReadOnlyCollection<LDtkComponentLevel> Levels => _levels;

        /// <value>
        /// The size of this level in Unity units.
        /// </value>
        public Vector2 Size => _size;
        
        /// <value>
        /// The color of this level's background.
        /// </value>
        public Color BgColor => _bgColor;
        
        /// <value>
        /// The smart color of this level.
        /// </value>
        public Color SmartColor => _smartColor;
        
        /// <value>
        /// The LDtk identifier of this level.
        /// </value>
        public string Identifier => _identifier;

        /// <value>
        /// The world depth of this level.
        /// </value>
        public int WorldDepth => _worldDepth;
        
        /// <value>
        /// The world-space rectangle of this level. <br/>
        /// Useful for getting a level's bounds for a camera, for example.
        /// </value>
        public Rect BorderRect => new Rect(transform.position, _size);

        internal void SetIdentifier(string identifier)
        {
            _identifier = identifier;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void ResetStatically()
        {
            _levels = new List<LDtkComponentLevel>();
        }

        private void OnEnable()
        {
            _levels.Add(this);
        }

        private void OnDisable()
        {
            _levels.Remove(this);
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
    }
}
