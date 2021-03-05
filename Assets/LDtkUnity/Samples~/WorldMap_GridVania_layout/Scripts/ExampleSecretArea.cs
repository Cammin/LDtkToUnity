using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.WorldMap_GridVania_layout
{
    public class ExampleSecretArea : MonoBehaviour
    {
        [LDtkField("playSecretJingle")] public bool _playSecretJingle;
    }
}
