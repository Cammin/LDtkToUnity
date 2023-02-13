using UnityEngine;

namespace LDtkUnity.Tests
{
    public class FieldsTestComponent : MonoBehaviour
    {
        [SerializeField] private bool _assignDirectGets;
        [SerializeField] private bool _assignTryGets;

        [Header("This component demonstrates how the LDtkFields component can be used during import")]
        public int _int;
        public float _float;
        public bool _bool;
        public string _string;
        public string _multiline;
        public SomeEnum _enum;
        public Color _color;
        public Vector2 _point;
        public string _filePath;
        public LDtkReferenceToAnEntityInstance _entityRef;
        public Sprite _tile;
        
        public int[] _ints;
        public float[] _floats;
        public bool[] _bools;
        public string[] _strings;
        public string[] _multilines;
        public SomeEnum[] _enums;
        public Color[] _colors;
        public Vector2[] _points;
        public string[] _filePaths;
        public LDtkReferenceToAnEntityInstance[] _entityRefs;
        public Sprite[] _tiles;
        

        public void Init()
        {
            
        }

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
        }


        //This function call used for testing in runtime
        private void Start()
        {
            OnLDtkImportFields(GetComponent<LDtkFields>());
        }
        
        private bool AssignTryGets(LDtkFields fields)
        {
            if (
               fields.TryGetInt(FixtureConsts.SINGLE_INT, out int intValue)
            && fields.TryGetFloat(FixtureConsts.SINGLE_FLOAT, out float floatValue)
            && fields.TryGetBool(FixtureConsts.SINGLE_BOOL, out bool booleanValue)
            && fields.TryGetString(FixtureConsts.SINGLE_STRING, out string stringValue)
            && fields.TryGetMultiline(FixtureConsts.SINGLE_MULTILINES, out string multilinesValue)
            && fields.TryGetEnum(FixtureConsts.SINGLE_ENUM, out SomeEnum enumValue)
            && fields.TryGetColor(FixtureConsts.SINGLE_COLOR, out Color colorValue)
            && fields.TryGetPoint(FixtureConsts.SINGLE_POINT, out Vector2 pointValue)
            && fields.TryGetFilePath(FixtureConsts.SINGLE_FILE_PATH, out string filePathValue)
            && fields.TryGetEntityReference(FixtureConsts.SINGLE_ENTITY_REF, out LDtkReferenceToAnEntityInstance entityRefValue)
            && fields.TryGetTile(FixtureConsts.SINGLE_TILE, out Sprite tileValue)
            
            //ARRAYS
            && fields.TryGetIntArray(FixtureConsts.ARRAY_INT, out int[] intArrayValue)
            && fields.TryGetFloatArray(FixtureConsts.ARRAY_FLOAT, out float[] floatArrayValue)
            && fields.TryGetBoolArray(FixtureConsts.ARRAY_BOOL, out bool[] boolArrayValue)
            && fields.TryGetStringArray(FixtureConsts.ARRAY_STRING, out string[] stringArrayValue)
            && fields.TryGetMultilineArray(FixtureConsts.ARRAY_MULTILINES, out string[] multilinesArrayValue)
            && fields.TryGetEnumArray(FixtureConsts.ARRAY_ENUM, out SomeEnum[] enumArrayValue)
            && fields.TryGetColorArray(FixtureConsts.ARRAY_COLOR, out Color[] colorArrayValue)
            && fields.TryGetPointArray(FixtureConsts.ARRAY_POINT, out Vector2[] pointArrayValue)
            && fields.TryGetEntityReferenceArray(FixtureConsts.ARRAY_ENTITY_REF, out LDtkReferenceToAnEntityInstance[] entityRefArrayValue)
            && fields.TryGetTileArray(FixtureConsts.ARRAY_TILE, out Sprite[] tileArrayValue))
            {
                _int = intValue;
                _float = floatValue;
                _bool = booleanValue;
                _string = stringValue;
                _multiline = multilinesValue;
                _enum = enumValue;
                _color = colorValue;
                _point = pointValue;
                _filePath = filePathValue;
                _entityRef = entityRefValue;
                _tile = tileValue;
                _ints = intArrayValue;
                _floats = floatArrayValue;
                _bools = boolArrayValue;
                _strings = stringArrayValue;
                _multilines = multilinesArrayValue;
                _enums = enumArrayValue;
                _colors = colorArrayValue;
                _points = pointArrayValue;
                _entityRefs = entityRefArrayValue;
                _tiles = tileArrayValue;

                return true;
            }
            return false;
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
    }
}
