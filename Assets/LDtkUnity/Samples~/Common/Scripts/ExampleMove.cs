using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Samples
{
    public class ExampleMove : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5;
        
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
#if UNITY_6000_0_OR_NEWER
            _rb.linearVelocity = GetMove() * _moveSpeed;
#else
            _rb.velocity = GetMove() * _moveSpeed;
#endif
        }

        private Vector2 GetMove()
        {
            Vector2 move = Vector2.zero;
            
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            Gamepad gamepad = Gamepad.current;
            if (keyboard != null && keyboard.anyKey.isPressed)
            {
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                    move.y += 1;
                
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                    move.x -= 1;
                
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                    move.y -= 1;
                
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                    move.x += 1;
            }
            else if (gamepad != null)
            {
                if (gamepad.leftStick.IsActuated())
                    move = gamepad.leftStick.ReadValue();
                
                else if (gamepad.dpad.IsActuated())
                    move = gamepad.dpad.ReadValue();
            }
#elif ENABLE_LEGACY_INPUT_MANAGER
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");
#endif
            
            return move.normalized;
        }
    }
}