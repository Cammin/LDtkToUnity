using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.WorldMap_GridVania_layout
{
    public class ExamplePlayer : MonoBehaviour
    {
        //[LDtkField("inventory")] public ItemType[] _inventory;
        [LDtkField("HP")] public int _health;
    }
}
