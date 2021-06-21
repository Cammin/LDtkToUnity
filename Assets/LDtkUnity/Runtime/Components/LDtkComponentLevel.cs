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
