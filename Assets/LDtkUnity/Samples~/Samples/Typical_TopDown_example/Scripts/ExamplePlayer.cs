using LDtkUnity;
using UnityEngine;

namespace Samples.Typical_TopDown_example
{
    public class ExamplePlayer : MonoBehaviour
    {
        [LDtkField("life")] public int _life;
        [LDtkField("ammo")] public int _ammo;
    }
}