using LDtkUnity;
using UnityEngine;

namespace Samples.Entities
{
    public class ExampleTrigger : MonoBehaviour
    {
        [LDtkField("target")] public Vector2 _target;
        [LDtkField("condition")] public TriggerCondition _condition;
        [LDtkField("message")] public string _message;
    }
}
