using LDtkUnity;
using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace Samples.Scripts.YourTypical2DPlatformer
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}