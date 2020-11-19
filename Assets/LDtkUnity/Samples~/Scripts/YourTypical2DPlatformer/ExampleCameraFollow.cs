using LDtkUnity.Runtime.Builders;
using LDtkUnity.Runtime.Data.Level;
using UnityEngine;

namespace Samples.Scripts.YourTypical2DPlatformer
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
            LDtkLevelBuilder.OnLevelBuilt += OnOnLevelBuilt;
        }

        private void OnDisable()
        {
            LDtkLevelBuilder.OnLevelBuilt -= OnOnLevelBuilt;
        }

        private void OnOnLevelBuilt(LDtkDataLevel lvl)
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
