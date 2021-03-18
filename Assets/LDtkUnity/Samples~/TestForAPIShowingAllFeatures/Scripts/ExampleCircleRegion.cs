using LDtkUnity;
using UnityEngine;

namespace Samples.TestForAPIShowingAllFeatures
{
    public class ExampleCircleRegion : MonoBehaviour
    {
        [LDtkField("SomeEnum")] public SomeEnum _someEnum;
        [LDtkField("text")] public string _text;
    }
}