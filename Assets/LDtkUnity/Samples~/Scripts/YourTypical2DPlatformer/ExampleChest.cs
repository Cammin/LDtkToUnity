using LDtkUnity;
using LDtkUnity.Runtime.FieldInjection;
using LDtkUnity.Typical_2D_platformer_example;
using UnityEngine;

namespace Samples.Scripts.YourTypical2DPlatformer
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}