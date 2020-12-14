using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples
{
    public class ExamplePlayer : MonoBehaviour
    {
        [LDtkField] public Item[] items;
    }
}