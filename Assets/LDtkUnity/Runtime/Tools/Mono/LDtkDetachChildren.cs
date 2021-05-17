using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_DETACH_OBJECT)]
    public class LDtkDetachChildren : MonoBehaviour
    {
        private void Awake()
        {
            transform.DetachChildren();
            Destroy(this);
        }
    }
}