using LDtkUnity;
using UnityEngine;

namespace Samples.TestForAPIShowingAllFeatures
{
    public class ExampleRectRegion : MonoBehaviour
    {
        [LDtkField("SomeEnum")] public SomeEnum _someEnum;
        [LDtkField("Integer")] public int _integer;
    }
}
