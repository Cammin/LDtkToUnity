using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleLightEmitter : MonoBehaviour
    {
        [LDtkField("radius")] public float _radius;
        [LDtkField("colors")] public Color[] _colors;
        [LDtkField("flickering")] public bool _flickering;
    }
}
