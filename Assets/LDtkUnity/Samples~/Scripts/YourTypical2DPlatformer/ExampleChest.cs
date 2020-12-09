using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}