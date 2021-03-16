using LDtkUnity;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleChest : MonoBehaviour
    {
        [LDtkField("content")] public ItemType[] _content;
        [LDtkField("requireKey")] public bool _requireKey;
    }
}
