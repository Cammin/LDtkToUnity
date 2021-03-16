using LDtkUnity;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleCircleRegion : MonoBehaviour
    {
        [LDtkField("SomeEnum")] public SomeEnum _someEnum;
        [LDtkField("text")] public string _text;
    }
}