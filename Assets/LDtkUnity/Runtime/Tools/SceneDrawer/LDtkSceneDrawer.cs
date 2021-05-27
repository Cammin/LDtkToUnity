using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_SCENE_DRAWER)]
    [ExcludeFromDocs]
    public class LDtkSceneDrawer : MonoBehaviour
    {
        [SerializeField] private List<LDtkSceneDrawerData> _data = new List<LDtkSceneDrawerData>();

        public void AddReference(LDtkSceneDrawerData data)
        {
            _data.Add(data);
        }
        
        private void OnDrawGizmos()
        {
            foreach (LDtkSceneDrawerData drawer in _data)
            {
                drawer.OnDrawGizmos();
            }
        }
    }
}