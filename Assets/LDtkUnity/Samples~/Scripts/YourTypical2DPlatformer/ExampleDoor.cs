using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples.Scripts.YourTypical2DPlatformer
{
    public class ExampleDoor : MonoBehaviour
    {
        [LDtkField] public bool locked;
    }
}