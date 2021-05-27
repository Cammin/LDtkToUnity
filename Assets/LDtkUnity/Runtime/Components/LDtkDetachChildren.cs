using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_DETACH_OBJECT)]
    [ExcludeFromDocs]
    public class LDtkDetachChildren : MonoBehaviour
    {
        private void Awake()
        {
            transform.DetachChildren();
            Destroy(this);
        }
    }
}