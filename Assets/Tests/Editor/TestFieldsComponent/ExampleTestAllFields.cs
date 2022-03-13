using System.Linq;
using LDtkUnity;
using Samples.TestAllFields;
using UnityEngine;

namespace Tests.Editor
{
    public class ExampleTestAllFields : MonoBehaviour, ILDtkImportedFields
    {
        [SerializeField] private bool _assignDirectGets;
        [SerializeField] private bool _assignTryGets;
        [SerializeField] private bool _logEachValueAsString;
        [SerializeField] private bool _ensureStringValuesFailure;
        [SerializeField] private bool _assignTryGetFailures;
        
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

        public void OnLDtkImportFields(LDtkFields fields)
        {
            if (_assignDirectGets)
            {
                AssignDirectGets(fields);
            }

            if (_assignTryGets)
            {
                AssignTryGets(fields);
            }

            if (_logEachValueAsString)
            {
                LogEachValueAsString(fields);
            }

            if (_ensureStringValuesFailure)
            {
                EnsureStringValuesFailure(fields);
            }

            if (_assignTryGetFailures)
            {
                AssignTryGetFailures(fields);
            }
        }

        private void AssignTryGets(LDtkFields fields)
        {
            if (fields.TryGetInt(FixtureConsts.SINGLE_INT, out int intValue))
            {
                _int = intValue;
            }
            if (fields.TryGetFloat(FixtureConsts.SINGLE_FLOAT, out float floatValue))
            {
                _float = floatValue;
            }
            if (fields.TryGetBool(FixtureConsts.SINGLE_BOOL, out bool booleanValue))
            {
                _bool = booleanValue;
            }
            if (fields.TryGetString(FixtureConsts.SINGLE_STRING, out string stringValue))
            {
                _string = stringValue;
            }
            if (fields.TryGetMultiline(FixtureConsts.SINGLE_MULTILINES, out string multilinesValue))
            {
                _multiline = multilinesValue;
            }
            if (fields.TryGetEnum(FixtureConsts.SINGLE_ENUM, out SomeEnum enumValue))
            {
                _enum = enumValue;
            }
            if (fields.TryGetColor(FixtureConsts.SINGLE_COLOR, out Color colorValue))
            {
                _color = colorValue;
            }
            if (fields.TryGetPoint(FixtureConsts.SINGLE_POINT, out Vector2 pointValue))
            {
                _point = pointValue;
            }
            if (fields.TryGetFilePath(FixtureConsts.SINGLE_FILE_PATH, out string filePathValue))
            {
                _filePath = filePathValue;
            }
            if (fields.TryGetEntityReference(FixtureConsts.SINGLE_ENTITY_REF, out GameObject entityRefValue))
            {
                _entityRef = entityRefValue;
            }
            if (fields.TryGetTile(FixtureConsts.SINGLE_TILE, out Sprite tileValue))
            {
                _tile = tileValue;
            }
            
            //ARRAYS
            if (fields.TryGetIntArray(FixtureConsts.ARRAY_INT, out int[] intArrayValue))
            {
                _ints = intArrayValue;
            }
            if (fields.TryGetFloatArray(FixtureConsts.ARRAY_FLOAT, out float[] floatArrayValue))
            {
                _floats = floatArrayValue;
            }
            if (fields.TryGetBoolArray(FixtureConsts.ARRAY_BOOL, out bool[] boolArrayValue))
            {
                _bools = boolArrayValue;
            }
            if (fields.TryGetStringArray(FixtureConsts.ARRAY_STRING, out string[] stringArrayValue))
            {
                _strings = stringArrayValue;
            }
            if (fields.TryGetMultilineArray(FixtureConsts.ARRAY_MULTILINES, out string[] multilinesArrayValue))
            {
                _multilines = multilinesArrayValue;
            }
            if (fields.TryGetEnumArray(FixtureConsts.ARRAY_ENUM, out SomeEnum[] enumArrayValue))
            {
                _enums = enumArrayValue;
            }
            if (fields.TryGetColorArray(FixtureConsts.ARRAY_COLOR, out Color[] colorArrayValue))
            {
                _colors = colorArrayValue;
            }
            if (fields.TryGetPointArray(FixtureConsts.ARRAY_POINT, out Vector2[] pointArrayValue))
            {
                _points = pointArrayValue;
            }
            if (fields.TryGetEntityReferenceArray(FixtureConsts.ARRAY_ENTITY_REF, out GameObject[] entityRefArrayValue))
            {
                _entityRefs = entityRefArrayValue;
            }
            if (fields.TryGetTileArray(FixtureConsts.ARRAY_TILE, out Sprite[] tileArrayValue))
            {
                _tiles = tileArrayValue;
            }
        }

        private void TestOutsideRangeIndex()
        {
            
        }
        
        //Used to test the GetValueAsString function
        void LogEachValueAsString(LDtkFields fields)
        {
            foreach (string identifier in FixtureConsts.Singles)
            {
                Debug.Log($"GetValueAsString ({identifier}:{fields.GetValueAsString(identifier)})");
            }
            
            foreach (string identifier in FixtureConsts.Singles)
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
            
            foreach (string identifier in FixtureConsts.Arrays)
            {
                string joined = string.Join(", ", fields.GetValuesAsStrings(identifier));
                Debug.Log($"GetValuesAsStrings ({identifier}:{joined})");
            }
            
            foreach (string identifier in FixtureConsts.Arrays)
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
            _int = fields.GetInt(FixtureConsts.SINGLE_INT);
            _float = fields.GetFloat(FixtureConsts.SINGLE_FLOAT);
            _bool = fields.GetBool(FixtureConsts.SINGLE_BOOL);
            _string = fields.GetString(FixtureConsts.SINGLE_STRING);
            _multiline = fields.GetMultiline(FixtureConsts.SINGLE_MULTILINES);
            _enum = fields.GetEnum<SomeEnum>(FixtureConsts.SINGLE_ENUM);
            _color = fields.GetColor(FixtureConsts.SINGLE_COLOR);
            _point = fields.GetPoint(FixtureConsts.SINGLE_POINT);
            _filePath = fields.GetFilePath(FixtureConsts.SINGLE_FILE_PATH);
            _entityRef = fields.GetEntityReference(FixtureConsts.SINGLE_ENTITY_REF);
            _tile = fields.GetTile(FixtureConsts.SINGLE_TILE);

            _ints = fields.GetIntArray(FixtureConsts.ARRAY_INT);
            _floats = fields.GetFloatArray(FixtureConsts.ARRAY_FLOAT);
            _bools = fields.GetBoolArray(FixtureConsts.ARRAY_BOOL);
            _strings = fields.GetStringArray(FixtureConsts.ARRAY_STRING);
            _multilines = fields.GetMultilineArray(FixtureConsts.ARRAY_MULTILINES);
            _enums = fields.GetEnumArray<SomeEnum>(FixtureConsts.ARRAY_ENUM);
            _colors = fields.GetColorArray(FixtureConsts.ARRAY_COLOR);
            _points = fields.GetPointArray(FixtureConsts.ARRAY_POINT);
            _filePaths = fields.GetFilePathArray(FixtureConsts.ARRAY_FILE_PATH);
            _entityRefs = fields.GetEntityReferenceArray(FixtureConsts.ARRAY_ENTITY_REF);
            _tiles = fields.GetTileArray(FixtureConsts.ARRAY_TILE);
        }

        private bool CheckFieldsIsNull(LDtkFields fields)
        {
            return FixtureConsts.Singles.All(p => CheckSingleFieldNullSuccess(fields, p)) && FixtureConsts.Arrays.All(p => CheckArrayFieldNullSuccess(fields, p));
        }

        /// <summary>
        /// Try checking for a null array element when it's not an array
        /// </summary>
        private bool CheckInvalidNullCheckUsage(LDtkFields fields)
        {
            return false;
        }

        private bool CheckSingleFieldNullSuccess(LDtkFields fields, string field)
        {
            if (!fields.ContainsField(field))
            {
                Debug.LogError("field doesn't exist, bad");
                return false;
            }
            
            return fields.IsNull(field);
        }

        private bool CheckArrayFieldNullSuccess(LDtkFields fields, string field)
        {
            if (!fields.ContainsField(field))
            {
                Debug.LogError("field doesn't exist, bad");
                return false;
            }

            int arraySize = fields.GetArraySize(field);

            for (int i = 0; i < arraySize; i++)
            {
                if (fields.IsNullAtArrayIndex(field, i))
                {
                    return true;
                }
            }

            return false;
        }

        //This function call used for testing in runtime
        private void Start()
        {
            OnLDtkImportFields(GetComponent<LDtkFields>());
        }
    }
}
