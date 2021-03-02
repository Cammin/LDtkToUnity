using LDtkUnity.FieldInjection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Samples.WorldMap_GridVania_layout.Scripts
{
    public class ExampleSecretArea : MonoBehaviour
    {
        [LDtkField("playSecretJingle")] public bool _playSecretJingle;
    }
}
