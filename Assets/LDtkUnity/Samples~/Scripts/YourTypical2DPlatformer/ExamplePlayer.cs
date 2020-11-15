using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples.Scripts.YourTypical2DPlatformer
{
    public class ExamplePlayer : MonoBehaviour
    {
        [LDtkField] public Item[] items;

        [SerializeField] private float _moveSpeed = 5;
        
        private Rigidbody2D _rb;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
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
    }
}