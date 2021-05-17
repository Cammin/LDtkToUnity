using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_SETTABLE_RENDERER)]
    public class LDtkEntityRenderer : MonoBehaviour, ILDtkSettableSortingOrder, ILDtkSettableOpacity, ILDtkSettableColor
    {
        private const string COMPONENT_NAME = "Entity Renderer";
        
        [SerializeField] private Renderer _renderer = null;
        [SerializeField] private bool _setSortingOrder = true;
        [SerializeField] private bool _setOpacity = true;
        [SerializeField] private bool _setEntityColor = true;
        
        public void OnLDtkSetSortingOrder(int sortingOrder)
        {
            if (!_setSortingOrder || !CheckRendererIsAssigned())
            {
                return;
            }
            
            _renderer.sortingOrder = sortingOrder;
        }

        public void OnLDtkSetOpacity(float alpha)
        {
            if (!_setOpacity || !(_renderer is SpriteRenderer spriteRenderer) || !CheckRendererIsAssigned())
            {
                return;
            }
            
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }

        public void OnLDtkSetEntityColor(Color newColor)
        {
            if (!_setEntityColor || !(_renderer is SpriteRenderer spriteRenderer) || !CheckRendererIsAssigned())
            {
                return;
            }
            
            //maintain alpha
            newColor.a = spriteRenderer.color.a;
            
            
            //however, if there exists a field with a color, then use it's color instead
            if (TryGetComponent(out LDtkFields fields))
            {
                LDtkField field = fields._fields.FirstOrDefault(p => p._data.Any(pp => pp._type == LDtkFieldType.Color));
                if (field != null)
                {
                    LDtkFieldElement element = field._data.FirstOrDefault(p => p._type == LDtkFieldType.Color);
                    if (element != null)
                    {
                        newColor = element.GetColorValue();
                    }
                }
            }
            

            spriteRenderer.color = newColor;
        }

        private bool CheckRendererIsAssigned()
        {
            if (_renderer != null) return true;
            
            Debug.LogError($"{name}'s Renderer was null. Is it assigned in inspector?", gameObject);
            return false;
        }
    }
}