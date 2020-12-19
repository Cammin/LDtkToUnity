using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleDoor : MonoBehaviour
    {
        [LDtkField("locked")] public bool _locked;
    }
}
