using UnityEngine;

namespace LDtkUnity.BuildEvents.EntityEvents
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
            if (!_setSortingOrder || !CheckIsAssigned()) return;
            
            _renderer.sortingOrder = sortingOrder;
        }

        public void OnLDtkSetOpacity(float alpha)
        {
            if (!_setOpacity || !(_renderer is SpriteRenderer spriteRenderer) || !CheckIsAssigned()) return;
            
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }

        public void OnLDtkSetEntityColor(Color newColor)
        {
            if (!_setEntityColor || !(_renderer is SpriteRenderer spriteRenderer) || !CheckIsAssigned()) return;
            
            //maintain alpha
            newColor.a = spriteRenderer.color.a;

            spriteRenderer.color = newColor;
        }

        private bool CheckIsAssigned()
        {
            if (_renderer != null) return true;
            
            Debug.LogError($"{name}'s Renderer was null. Is it assigned in inspector?", gameObject);
            return false;
        }
    }
}