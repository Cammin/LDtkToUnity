using UnityEngine;

namespace LDtkUnity.EntityEvents
{
    public class LDtkSettableOpacity : MonoBehaviour, ILDtkSettableOpacity
    {
        [SerializeField] private SpriteRenderer _renderer;
        public void OnLDtkSetOpacity(float alpha)
        {
            if (_renderer == null)
            {
                Debug.LogError($"{name}'s Renderer was null. Is it assigned in inspector?", gameObject);
                return;
            }
            
            Color newColor = _renderer.color;
            newColor.a = alpha;
            _renderer.color = newColor;
        }
    }
}