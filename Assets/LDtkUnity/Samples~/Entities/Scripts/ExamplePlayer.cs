using LDtkUnity;
using UnityEngine;

namespace Samples.Entities
{
    public class ExamplePlayer : MonoBehaviour
    {
        [LDtkField("life")] public float _life;
        [LDtkField("isAwaken")] public bool _isAwaken;
        [LDtkField("weapon")] public ItemType _weapon;
        [LDtkField("bag")] public ItemType[] _bag;
    }
}
