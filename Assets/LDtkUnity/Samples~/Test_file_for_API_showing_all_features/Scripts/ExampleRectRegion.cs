using LDtkUnity;
using Samples.Test_file_for_API_showing_all_features;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleRectRegion : MonoBehaviour
    {
        [LDtkField("SomeEnum")] public SomeEnum _someEnum;
        [LDtkField("Integer")] public int _integer;
    }
}
