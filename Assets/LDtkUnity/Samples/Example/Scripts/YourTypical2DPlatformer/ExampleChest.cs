using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples.Example.Scripts.YourTypical2DPlatformer
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}