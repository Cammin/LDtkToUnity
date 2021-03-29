using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_DETACH_OBJECT)]
    public class LDtkDetachGameObject : MonoBehaviour
    {
        private enum DetachPoint
        {
            OnAwake, 
            OnStart
        }
        private enum DetachOption 
        { 
            DetachFromParent, 
            DetachChildren
        }
        
        [Header("This script optimizes the performance of the hierarchy in runtime.")]
        [SerializeField] private DetachPoint _detachPoint = DetachPoint.OnAwake;
        [SerializeField] private DetachOption _detachOption = DetachOption.DetachChildren;

        private void Awake()
        {
            if (_detachPoint == DetachPoint.OnAwake)
            {
                DetachAction();
            }
        }

        private void Start()
        {
            if (_detachPoint == DetachPoint.OnStart)
            {
                DetachAction();
            }
        }

        private void DetachAction()
        {
            Detach();
            Destroy(this);
        }
        
        private void Detach()
        {
            if (_detachOption == DetachOption.DetachFromParent)
            {
                transform.parent = null;
                return;
            }

            transform.DetachChildren();
        }
    }
}