using LDtkUnity.BuildEvents;
using LDtkUnity.Enums;
using LDtkUnity.FieldInjection;
using UnityEngine;

namespace Samples.TestAllFields
{
    public class ExampleFields : MonoBehaviour, ILDtkFieldInjectedEvent
    {
        [LDtkField("integer")] public int _theInt = default;
        [LDtkField("float")] public float _theFloat = default;
        [LDtkField("boolean")] public bool _theBool = default;
        [LDtkField("string")] public string _theString = default;
        [LDtkField("multilines"), TextArea(3, 5)] public string _theMultiLine = default;
        [LDtkField("someenum")] public SomeEnum _theEnum = default;
        [LDtkField("color")] public Color _theColor = default;
        [LDtkField("point")] public Vector2Int _thePoint = default;
        [LDtkField("file_path")] public string _theFilePath = default;
        
        [LDtkField("integer_array")] public int[] _theInts = default;
        [LDtkField("float_array")] public float[] _theFloats = default;
        [LDtkField("boolean_array")] public bool[] _theBools = default;
        [LDtkField("string_array")] public string[] _theStrings = default;
        [LDtkField("multilines_array"), TextArea(3, 5)] public string[] _theMultiLines = default;
        [LDtkField("someenum_array")] public SomeEnum[] _theEnums = default;
        [LDtkField("color_array")] public Color[] _theColors = default;
        [LDtkField("point_array")] public Vector2Int[] _thePoints = default;
        [LDtkField("file_path_array")] public string[] _theFilePaths = default;

        //order of execution is these functions:
        private void Awake()
        {
            Debug.Log("LDtkEntityFieldsExample.Awake");
        }

        private void OnEnable()
        {
            Debug.Log("LDtkEntityFieldsExample.OnEnable");
        }
        
        public void OnLDtkFieldsInjected()
        {
            Debug.Log("LDtkEntityFieldsExample.OnLDtkFieldsInjected");
        }

        private void Start()
        {
            Debug.Log("LDtkEntityFieldsExample.Start");
        }
    }
}
