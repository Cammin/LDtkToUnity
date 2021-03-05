using LDtkUnity;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleButton : MonoBehaviour
    {
        [LDtkField("activationTargets")] public Vector2[] _activationTargets;
    }
}
