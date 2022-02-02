using LDtkUnity;
using UnityEngine;

namespace Samples.TestAllFields
{
    public class ExampleTestAllFields : MonoBehaviour, ILDtkImportedFields
    {
        [Header("This component demonstrates how the LDtkFields component can be used during import")]
        [SerializeField] private int _int;
        [SerializeField] private float _float;
        [SerializeField] private bool _bool;
        [SerializeField] private string _string;
        [SerializeField] private string _multiline;
        [SerializeField] private SomeEnum _enum;
        [SerializeField] private Color _color;
        [SerializeField] private Vector2 _point;
        [SerializeField] private string _filePath;
        
        [SerializeField] private int[] _ints;
        [SerializeField] private float[] _floats;
        [SerializeField] private bool[] _bools;
        [SerializeField] private string[] _strings;
        [SerializeField] private string[] _multilines;
        [SerializeField] private SomeEnum[] _enums;
        [SerializeField] private Color[] _colors;
        [SerializeField] private Vector2[] _points;
        [SerializeField] private string[] _filePaths;

        public void OnLDtkImportFields(LDtkFields fields)
        {
            _int = fields.GetInt("integer");
            _float = fields.GetFloat("float");
            _bool = fields.GetBool("boolean");
            _string = fields.GetString("string");
            _multiline = fields.GetMultiline("multilines");
            _enum = fields.GetEnum<SomeEnum>("someenum");
            _color = fields.GetColor("color");
            _point = fields.GetPoint("point");
            _filePath = fields.GetFilePath("file_path");
            
            _ints = fields.GetIntArray("integer_array");
            _floats = fields.GetFloatArray("float_array");
            _bools = fields.GetBoolArray("boolean_array");
            _strings = fields.GetStringArray("string_array");
            _multilines = fields.GetMultilineArray("multilines_array");
            _enums = fields.GetEnumArray<SomeEnum>("someenum_array");
            _colors = fields.GetColorArray("color_array");
            _points = fields.GetPointArray("point_array");
            _filePaths = fields.GetFilePathArray("file_path_array");
        }
    }
}
