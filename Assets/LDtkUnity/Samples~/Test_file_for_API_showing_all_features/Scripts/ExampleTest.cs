using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleTest : MonoBehaviour
    {
        [LDtkField("Integer")] public int _integer;
        [LDtkField("Float")] public float _float;
        [LDtkField("Boolean")] public bool _boolean;
        [LDtkField("String_singleLine")] public string _singleLine;
        [LDtkField("String_multiLines")] public string _multiLine;
        [LDtkField("Enum")] public SomeEnum _enum;
        [LDtkField("Color")] public Color _color;
        [LDtkField("Point")] public Vector2 _point;
        [LDtkField("Array_Integer")] public int[] _arrayInteger;
        [LDtkField("Array_Enum")] public SomeEnum[] _arrayEnum;
        [LDtkField("Array_points")] public Vector2[] _arrayPoints;
    }
}
