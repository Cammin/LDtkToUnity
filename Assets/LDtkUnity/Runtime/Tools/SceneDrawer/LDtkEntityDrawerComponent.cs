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

        internal LDtkEntityDrawerData EntityDrawer => _entityDrawer;
        internal List<LDtkFieldDrawerData> FieldDrawers => _fieldDrawers;

        internal void AddReference(LDtkFieldDrawerData data)
        {
            _fieldDrawers.Add(data);
        }

        internal void AddEntityDrawer(LDtkEntityDrawerData data)
        {
            _entityDrawer = data;
        }
    }
}