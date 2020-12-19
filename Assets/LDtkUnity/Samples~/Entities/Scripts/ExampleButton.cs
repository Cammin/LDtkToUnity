using LDtkUnity.BuildEvents;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleButton : MonoBehaviour
    {
        [SerializeField] private ExamplePointDrawer _drawer;
        
        [LDtkField("activationTargets")] public Vector2Int[] _activationTargets;
        
        public void Start()
        {
            _drawer.SetPoints(_activationTargets);
        }
    }
}
