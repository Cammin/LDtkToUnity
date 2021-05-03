using LDtkUnity;
using UnityEngine;

namespace Samples.Typical_TopDown_example
{
    public class ExampleInteractive : MonoBehaviour
    {
        [LDtkField("label")] public string _label;
        [LDtkField("description")] public string _description;
        [LDtkField("content")] public Item[] _content;
    }
}