using LDtkUnity;
using UnityEngine;

namespace Samples
{
    public class ExampleImportedRenderer : MonoBehaviour, ILDtkImportedLayer, ILDtkImportedEntity, ILDtkImportedSortingOrder
    {
        [SerializeField] private Renderer _renderer = null;
        [SerializeField] private bool _setSortingOrder = true;
        [SerializeField] private bool _setOpacity = true;
        [SerializeField] private bool _setEntityColor = true;
        
        public void OnLDtkImportSortingOrder(int sortingOrder)
        {
            if (!_setSortingOrder || !CheckRendererIsAssigned())
            {
                return;
            }
            
            _renderer.sortingOrder = sortingOrder;
        }

        public void OnLDtkImportLayer(LayerInstance layerInstance)
        {
            if (!_setOpacity || !(_renderer is SpriteRenderer spriteRenderer) || !CheckRendererIsAssigned())
            {
                return;
            }

            float alpha = (float)layerInstance.Opacity;
            
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }

        public void OnLDtkImportEntity(EntityInstance entityInstance)
        {
            if (!_setEntityColor || !(_renderer is SpriteRenderer spriteRenderer) || !CheckRendererIsAssigned())
            {
                return;
            }

            Color newColor = entityInstance.Definition.UnityColor;
            
            //maintain alpha
            newColor.a = spriteRenderer.color.a;
            
            
            //however, if there exists a field with a color, then use it's color instead
            if (TryGetComponent(out LDtkFields fields) && fields.GetFirstColor(out Color firstColor))
            {
                newColor = firstColor;
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