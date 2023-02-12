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

            float alpha = layerInstance.Opacity;
            
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

            Color newColor = entityInstance.UnitySmartColor;
            
            //maintain alpha
            newColor.a = spriteRenderer.color.a;
            

            spriteRenderer.color = newColor;
        }
        
        private bool CheckRendererIsAssigned()
        {
            if (_renderer != null) return true;
            
            Debug.LogWarning($"LDtk Sample: An entity's referenced component was null. This can happen when importing the examples for the first time. Try reimporting again to fix the samples.\n{name}", gameObject);
            return false;
        }

        public int GetPostprocessOrder()
        {
            return -1;
        }
    }
}