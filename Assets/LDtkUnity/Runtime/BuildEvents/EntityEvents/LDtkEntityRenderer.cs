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
            
            LDtkEditorUtil.Dirty(_renderer);
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
            
            LDtkEditorUtil.Dirty(_renderer);
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
            
            LDtkEditorUtil.Dirty(_renderer);
        }

        private bool CheckRendererIsAssigned()
        {
            if (_renderer != null) return true;
            
            Debug.LogError($"{name}'s Renderer was null. Is it assigned in inspector?", gameObject);
            return false;
        }
    }
}