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
        [SerializeField] private GameObject _entityRef;
        [SerializeField] private Sprite _tile;
        
        [SerializeField] private int[] _ints;
        [SerializeField] private float[] _floats;
        [SerializeField] private bool[] _bools;
        [SerializeField] private string[] _strings;
        [SerializeField] private string[] _multilines;
        [SerializeField] private SomeEnum[] _enums;
        [SerializeField] private Color[] _colors;
        [SerializeField] private Vector2[] _points;
        [SerializeField] private string[] _filePaths;
        [SerializeField] private GameObject[] _entityRefs;
        [SerializeField] private Sprite[] _tiles;

        private readonly string[] _singles = new[]
        {
            "integer",
            "float",
            "boolean",
            "string",
            "multilines",
            "someenum",
            "color",
            "point",
            "file_path",
            "entity_ref",
            "tile",
        };
        
        private readonly string[] _arrays = new[]
        {
            "integer_array",
            "float_array",
            "boolean_array",
            "string_array",
            "multilines_array",
            "someenum_array",
            "color_array",
            "point_array",
            "file_path_array",
            "entity_ref_array",
            "tile_array",
        };
        
        public void OnLDtkImportFields(LDtkFields fields)
        {
            //AssignDirectGets(fields);
            AssignTryGets(fields);
            //LogEachValueAsString(fields);

            //EnsureStringValuesFailure(fields);
            //AssignTryGetFailures(fields);
        }

        private void AssignTryGets(LDtkFields fields)
        {
            if (fields.TryGetInt("integer", out int intValue))
            {
                _int = intValue;
            }
            if (fields.TryGetFloat("float", out float floatValue))
            {
                _float = floatValue;
            }
            if (fields.TryGetBool("boolean", out bool booleanValue))
            {
                _bool = booleanValue;
            }
            if (fields.TryGetString("string", out string stringValue))
            {
                _string = stringValue;
            }
            if (fields.TryGetMultiline("multilines", out string multilinesValue))
            {
                _multiline = multilinesValue;
            }
            if (fields.TryGetEnum("someenum", out SomeEnum enumValue))
            {
                _enum = enumValue;
            }
            if (fields.TryGetColor("color", out Color colorValue))
            {
                _color = colorValue;
            }
            if (fields.TryGetPoint("point", out Vector2 pointValue))
            {
                _point = pointValue;
            }
            if (fields.TryGetFilePath("file_path", out string filePathValue))
            {
                _filePath = filePathValue;
            }
            if (fields.TryGetEntityReference("entity_ref", out GameObject entityRefValue))
            {
                _entityRef = entityRefValue;
            }
            if (fields.TryGetTile("tile", out Sprite tileValue))
            {
                _tile = tileValue;
            }
            
            //ARRAYS
            if (fields.TryGetIntArray("integer_array", out int[] intArrayValue))
            {
                _ints = intArrayValue;
            }
            if (fields.TryGetFloatArray("float_array", out float[] floatArrayValue))
            {
                _floats = floatArrayValue;
            }
            if (fields.TryGetBoolArray("boolean_array", out bool[] boolArrayValue))
            {
                _bools = boolArrayValue;
            }
            if (fields.TryGetStringArray("string_array", out string[] stringArrayValue))
            {
                _strings = stringArrayValue;
            }
            if (fields.TryGetMultilineArray("multilines_array", out string[] multilinesArrayValue))
            {
                _multilines = multilinesArrayValue;
            }
            if (fields.TryGetEnumArray("someenum_array", out SomeEnum[] enumArrayValue))
            {
                _enums = enumArrayValue;
            }
            if (fields.TryGetColorArray("color_array", out Color[] colorArrayValue))
            {
                _colors = colorArrayValue;
            }
            if (fields.TryGetPointArray("point_array", out Vector2[] pointArrayValue))
            {
                _points = pointArrayValue;
            }
            if (fields.TryGetEntityReferenceArray("entity_ref_array", out GameObject[] entityRefArrayValue))
            {
                _entityRefs = entityRefArrayValue;
            }
            if (fields.TryGetTileArray("tile_array", out Sprite[] tileArrayValue))
            {
                _tiles = tileArrayValue;
            }
        }

        //Used to test the GetValueAsString function
        void LogEachValueAsString(LDtkFields fields)
        {
            foreach (string identifier in _singles)
            {
                Debug.Log($"GetValueAsString ({identifier}:{fields.GetValueAsString(identifier)})");
            }
            
            foreach (string identifier in _singles)
            {
                if (fields.TryGetValueAsString(identifier, out string value))
                {
                    Debug.Log($"TryGetValueAsString ({identifier}:{value})");
                }
                else
                {
                    Debug.LogError($"issue getting values as string for single value {identifier}");
                }
            }
            
            foreach (string identifier in _arrays)
            {
                string joined = string.Join(", ", fields.GetValuesAsStrings(identifier));
                Debug.Log($"GetValuesAsStrings ({identifier}:{joined})");
            }
            
            foreach (string identifier in _arrays)
            {
                if (fields.TryGetValuesAsStrings(identifier, out string[] strings))
                {
                    Debug.Log($"TryGetValuesAsStrings ({identifier}:{string.Join(", ", strings)})");
                }
                else
                {
                    Debug.LogError($"issue getting values as strings for array value {identifier}");
                }
            }
        }

        private static void EnsureStringValuesFailure(LDtkFields fields)
        {
            //ensure failures
            string valueAsString = fields.GetValueAsString("color_array");
            Debug.Log(valueAsString);
            string[] valuesAsStrings = fields.GetValuesAsStrings("color");
            Debug.Log(string.Join(", ", valuesAsStrings));
        }

        private void AssignTryGetFailures(LDtkFields fields)
        {
            if (fields.TryGetInt("testFailure", out int intValue))
            {
                Debug.Log("Couldn't find the field as expected");
                return;
            }
            
            if (fields.TryGetInt("float", out int value))
            {
                Debug.Log("Type mismatch as expected between float and int");
                return;
            }

            Debug.LogError("Didn't fail the nonexistent fields test");
        }

        private void AssignDirectGets(LDtkFields fields)
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
            _entityRef = fields.GetEntityReference("entity_ref");
            _tile = fields.GetTile("tile");

            _ints = fields.GetIntArray("integer_array");
            _floats = fields.GetFloatArray("float_array");
            _bools = fields.GetBoolArray("boolean_array");
            _strings = fields.GetStringArray("string_array");
            _multilines = fields.GetMultilineArray("multilines_array");
            _enums = fields.GetEnumArray<SomeEnum>("someenum_array");
            _colors = fields.GetColorArray("color_array");
            _points = fields.GetPointArray("point_array");
            _entityRefs = fields.GetEntityReferenceArray("entity_ref_array");
            _tiles = fields.GetTileArray("tile_array");
        }

        //This function call used for testing in runtime
        private void Start()
        {
            OnLDtkImportFields(GetComponent<LDtkFields>());
        }
    }
}
