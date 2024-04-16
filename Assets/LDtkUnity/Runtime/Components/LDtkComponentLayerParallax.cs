using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Mimics the parallax implementation from LDtk.
    /// If you want something more customized than this, make a custom implementation of parallax. But this should be able to suit most needs.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_LAYER)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLayerParallax : MonoBehaviour
    {
        [Tooltip("Enable this on to mimic the parallax seen in LDtk.\n" +
                 "It will try grabbing the MainCamera, but it can also be set in code.")]
        public bool useParallax = true;
        
        [Tooltip("Whether to use FixedUpdate; otherwise LateUpdate")]
        public bool useFixedUpdate;
        public Vector2 parallaxFactor;
        public Vector2 halfLevelSize;
        public bool scaled;
        
        private Transform _cameraTransform;
        private Vector3 _prevPos;
        private bool _prevUseParallax;
        private Vector3 _delta;
        //private Vector3 _speed;

        internal void OnImport(Vector2 parallax, Vector2 halfLvlSize, bool isScaled)
        {
	        parallaxFactor = parallax;
	        halfLevelSize = halfLvlSize;
	        scaled = isScaled;

	        if (scaled)
	        {
				transform.position += (Vector3)(halfLevelSize * parallaxFactor);
				transform.localScale *= parallax;
	        }
        }
        
        private void Start()
        {
	        //If already cached from external code, don't fight with it
	        if (_cameraTransform)
	        {
		        return;
	        }
	        
	        Camera cam = Camera.main;
	        if (cam)
	        {
		        SetCamera(cam.transform);
		        Center();
	        }
        }
        
        private void Center()
        {
	        //setup the prev position as the center of the layer so its original position matches like LDtk
	        Vector2 scaledOffset = halfLevelSize;
	        if (scaled)
	        {
		        scaledOffset *= parallaxFactor;
	        }
	        _prevPos = (Vector2)transform.position + scaledOffset;
			
	        //set true to make the camera delta snap to where it should be like how it looks in ldtk
	        _prevUseParallax = true;
        }

        /// <summary>
        /// Custom-controlled set.
        /// </summary>
        [PublicAPI]
        public void SetCamera(Transform cam)
        {
	        _cameraTransform = cam;
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                UpdateParallax();
            }
        }
        private void LateUpdate()
        {
            if (!useFixedUpdate)
            {
                UpdateParallax();
            }
        }

        private void UpdateParallax()
        {
	        if (!_cameraTransform)
	        {
		        return;
	        }

	        if (parallaxFactor == Vector2.zero)
	        {
		        return;
	        }
	        
	        if (!Application.isPlaying && !useParallax)
	        {
		        return;
	        }

	        Vector3 cameraPos = _cameraTransform.position;
	        
		    //only realize a difference if the parallax is enabled
	        if (useParallax && !_prevUseParallax)
	        {
		        _prevPos = cameraPos;
	        }
	        _prevUseParallax = useParallax;

	        //Delta of the camera from last update. If there was movement, move this object with the camera but of a certain multiplier. 
	        _delta = cameraPos - _prevPos;
	        transform.position += Vector3.Scale(_delta, parallaxFactor);

	        _prevPos = cameraPos;
        }
    }
}