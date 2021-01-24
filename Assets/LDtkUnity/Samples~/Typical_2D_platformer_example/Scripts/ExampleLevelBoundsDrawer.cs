using LDtkUnity;
using UnityEngine;

namespace Samples.Typical_2D_platformer_example
{
    public class ExampleLevelBoundsDrawer : MonoBehaviour
    {
        private Level _lvl;
    
        public void SetLevel(Level lvlInstance)
        {
            _lvl = lvlInstance;
        }

        private void OnDrawGizmos()
        {
            if (_lvl == null) return;

            Rect bounds = _lvl.UnityWorldSpaceBounds(16);

            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
