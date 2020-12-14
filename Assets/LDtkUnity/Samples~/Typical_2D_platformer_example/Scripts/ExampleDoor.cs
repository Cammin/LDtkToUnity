using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Typical_2D_platformer_example
{
    public class ExampleDoor : MonoBehaviour
    {
        [LDtkField] public bool locked;
    }
}