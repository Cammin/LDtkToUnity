
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Typical_2D_platformer_example
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}