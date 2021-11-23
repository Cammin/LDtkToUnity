using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL)]
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + "Level Data")]
    [ExcludeFromDocs]
    public class LDtkComponentLevel : MonoBehaviour
    {
        [SerializeField] private string _identifier = string.Empty;
        [SerializeField] private Vector2 _size = Vector2.zero;
        [SerializeField] private Color _bgColor = Color.white;

        public Vector2 Size => _size;
        public Color BgColor => _bgColor;
        public string Identifier => _identifier;
        
        /// <summary>
        /// The world-space rectangle of this level. Useful for getting a level's bounds for a camera, for example.
        /// </summary>
        public Rect LevelRectangle => new Rect(transform.position, _size);

        public void SetIdentifier(string identifier)
        {
            _identifier = identifier;
        }
        
        public void SetSize(Vector2 size)
        {
            _size = size;
        }

        public void SetBgColor(Color color)
        {
            _bgColor = color;
        }
    }
}
