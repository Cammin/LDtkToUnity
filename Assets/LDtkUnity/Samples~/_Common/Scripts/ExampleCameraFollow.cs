using LDtkUnity.Builders;
using LDtkUnity.Data;
using Samples.Typical_2D_platformer_example;
using UnityEngine;
using ExamplePlayer = Samples.Entities.ExamplePlayer;

namespace Samples
{
    public class ExampleCameraFollow : MonoBehaviour
    {
        [SerializeField] private float _smoothAmount = 0.5f;
        [SerializeField] private Vector2 _followOffset = Vector2.zero;
        
        private Transform _follow;
        private Vector3 _dampVelocity;
        
        private Vector2 FollowPos => (Vector2) _follow.position + _followOffset;
        
        private void OnEnable()
        {
            LDtkLevelBuilder.OnLevelBuilt += OnLevelBuilt;
        }

        private void OnDisable()
        {
            LDtkLevelBuilder.OnLevelBuilt -= OnLevelBuilt;
        }

        private void OnLevelBuilt(LDtkDataLevel lvl)
        {
            ExamplePlayer player = FindObjectOfType<ExamplePlayer>();
            if (player == null) return;
            
            _follow = player.transform; 
            transform.position = FollowPos;
        }

        private void FixedUpdate()
        {
            if (_follow == null)
            {
                return;
            }

            Vector2 followPos = FollowPos;
            Vector3 targetPos = new Vector3(followPos.x, followPos.y, -10);
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _dampVelocity, _smoothAmount);
        }

    }
}
