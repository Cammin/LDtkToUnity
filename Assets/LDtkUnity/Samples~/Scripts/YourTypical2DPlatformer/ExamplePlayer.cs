using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace Samples.Scripts.YourTypical2DPlatformer
{
    public class ExamplePlayer : MonoBehaviour, ILDtkSettableSortingOrder
    {
        [LDtkField] public Item[] items;

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

        public void OnLDtkSortingOrderSet(int sortingOrder)
        {
            _renderer.sortingOrder = sortingOrder;
        }
    }
}