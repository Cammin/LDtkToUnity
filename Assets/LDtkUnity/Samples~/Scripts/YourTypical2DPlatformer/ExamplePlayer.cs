using LDtkUnity.Runtime.EntityCallbacks;
using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace Samples.Scripts.YourTypical2DPlatformer
{
    public class ExamplePlayer : MonoBehaviour, ILDtkSettableSortingOrder, ILDtkSettableOpacity
    {
        [LDtkField] public ItemType[] items;

        [SerializeField] private float _moveSpeed = 5;
        
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }
        
        private void Update()
        {
            _rb.velocity = GetMove() * _moveSpeed;
        }

        private Vector2 GetMove()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x, y).normalized;
        }

        public void OnLDtkSetSortingOrder(int sortingOrder)
        {
            _renderer.sortingOrder = sortingOrder;
        }

        public void OnLDtkSetOpacity(float alpha)
        {
            Color newColor = _renderer.color;
            newColor.a = alpha;
            _renderer.color = newColor;
        }
    }
}