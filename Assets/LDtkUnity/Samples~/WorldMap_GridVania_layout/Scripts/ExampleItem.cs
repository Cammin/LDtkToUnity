using LDtkUnity;
using UnityEngine;

namespace Samples.WorldMap_GridVania_layout
{
    public class ExampleItem : MonoBehaviour
    {
        [LDtkField("type")] public ItemType _item;
        [LDtkField("price")] public int _price;
    }
}
