using UnityEngine;

namespace Samples
{
    public class ExampleCameraFollow : MonoBehaviour
    {
        [SerializeField] private float _smoothAmount = 0.5f;
        [SerializeField] private Vector2 _followOffset = Vector2.zero;
        
        private Transform _follow;
        private Vector3 _dampVelocity;
        
        private Vector2 FollowPos => (Vector2) _follow.position + _followOffset;

        public void Start()
        {
            GameObject player = GameObject.FindWithTag("Player");
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
