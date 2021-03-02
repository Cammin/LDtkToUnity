using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.WorldMap_GridVania_layout.Scripts
{
    public class ExampleItem : MonoBehaviour
    {
        [LDtkField("type")] public ItemType _item;
        [LDtkField("price")] public int _price;
    }
}
