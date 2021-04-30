using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_DETACH_OBJECT)]
    public class LDtkDetachChildren : MonoBehaviour
    {
        private const string COMPONENT_NAME = "Detach Children";

        private void Awake()
        {
            transform.DetachChildren();
            Destroy(this);
        }
    }
}