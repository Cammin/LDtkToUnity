using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_SCENE_DRAWER)]
    [ExcludeFromDocs]
    public class LDtkSceneDrawerComponent : MonoBehaviour
    {
        [SerializeField] private LDtkEntityDrawerData _entityDrawer;
        [SerializeField] private List<LDtkFieldDrawerData> _fieldDrawers = new List<LDtkFieldDrawerData>();

        public void AddReference(LDtkFieldDrawerData data)
        {
            _fieldDrawers.Add(data);
        }

        public void AddEntityDrawer(LDtkEntityDrawerData data)
        {
            _entityDrawer = data;
        }
        
        private void OnDrawGizmos()
        {
            _entityDrawer.OnDrawGizmos();
            
            foreach (LDtkFieldDrawerData drawer in _fieldDrawers)
            {
                drawer.OnDrawGizmos();
            }
        }
    }
}