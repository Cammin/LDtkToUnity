using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Typical_2D_platformer_example
{
    public class ExamplePlayer : MonoBehaviour
    {
        [LDtkField] public Item[] items;
    }
}