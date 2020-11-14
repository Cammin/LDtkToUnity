using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples.Example.Scripts.YourTypical2DPlatformer
{
    public class ExamplePlayer : MonoBehaviour
    {
        [LDtkField] public Item[] items;
    }
}