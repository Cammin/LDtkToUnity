using LDtkUnity.FieldInjection;
using LDtkUnity.GridVania.Enums;
using UnityEngine;

namespace Samples.WorldMap_GridVania_layout
{
    public class ExampleItem : MonoBehaviour
    {
        [LDtkField("type")] public ItemType _item;
    }
}
