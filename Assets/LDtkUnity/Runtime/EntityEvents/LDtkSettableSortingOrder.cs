using UnityEngine;

namespace LDtkUnity.EntityEvents
{
    public class LDtkSettableSortingOrder : MonoBehaviour, ILDtkSettableSortingOrder
    {
        [SerializeField] private Renderer _renderer;
        public void OnLDtkSetSortingOrder(int sortingOrder)
        {
            if (_renderer == null)
            {
                Debug.LogError($"{name}'s Renderer was null. Is it assigned in inspector?", gameObject);
                return;
            }
            
            _renderer.sortingOrder = sortingOrder;
        }
    }
}