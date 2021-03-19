using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    //todo add helpurl
    public class LDtkSettableRenderer : MonoBehaviour, ILDtkSettableSortingOrder, ILDtkSettableOpacity, ILDtkSettableColor
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