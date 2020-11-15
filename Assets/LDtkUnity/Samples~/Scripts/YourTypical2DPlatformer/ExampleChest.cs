using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples.Scripts.YourTypical2DPlatformer
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField] public Item[] content;
    }
}