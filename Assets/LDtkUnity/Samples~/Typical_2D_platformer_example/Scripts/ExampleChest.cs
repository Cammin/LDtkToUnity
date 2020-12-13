using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}