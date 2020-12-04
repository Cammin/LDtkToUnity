using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;
using LDtkUnity.Enums;

namespace Samples.Scripts.YourTypical2DPlatformer
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}