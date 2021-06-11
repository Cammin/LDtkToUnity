using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_SCENE_DRAWER)]
    [ExcludeFromDocs]
    public class LDtkEntityDrawer : MonoBehaviour
    {
        [SerializeField] private List<LDtkSceneDrawerBase> _data = new List<LDtkSceneDrawerBase>();

        public void AddReference(LDtkSceneDrawerBase data)
        {
            _data.Add(data);
        }
        
        private void OnDrawGizmos()
        {
            foreach (LDtkSceneDrawerBase drawer in _data)
            {
                drawer.Draw();
            }
        }
    }
}