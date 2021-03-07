using LDtkUnity;
using UnityEngine;

namespace Samples.Typical_2D_platformer_example
{
    public class ExampleMob : MonoBehaviour
    {
        [LDtkField] public Item[] loot;
        [LDtkField] public Vector2[] patrol;
    }
}