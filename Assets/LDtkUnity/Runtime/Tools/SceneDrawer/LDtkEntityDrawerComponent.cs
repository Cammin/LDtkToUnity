using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_SCENE_DRAWER)]
    [ExcludeFromDocs]
    public class LDtkEntityDrawerComponent : MonoBehaviour
    {
        [SerializeField] private LDtkEntityDrawerData _entityDrawer;
        [SerializeField] private List<LDtkFieldDrawerData> _fieldDrawers = new List<LDtkFieldDrawerData>();

        public LDtkEntityDrawerData EntityDrawer => _entityDrawer;
        public List<LDtkFieldDrawerData> FieldDrawers => _fieldDrawers;

        public void AddReference(LDtkFieldDrawerData data)
        {
            _fieldDrawers.Add(data);
        }

        public void AddEntityDrawer(LDtkEntityDrawerData data)
        {
            _entityDrawer = data;
        }
    }
}