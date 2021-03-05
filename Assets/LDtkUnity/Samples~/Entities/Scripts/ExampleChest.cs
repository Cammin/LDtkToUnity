using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField("loot")] public ItemType[] _loot;
        [LDtkField("requireKey")] public bool _requireKey;
    }
}
