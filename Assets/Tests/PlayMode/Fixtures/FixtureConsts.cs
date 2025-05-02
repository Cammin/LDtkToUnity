using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Tests
{

    public static class FixtureConsts
    {
        public const string SINGLE_INT = "integer";
        public const string SINGLE_FLOAT = "float";
        public const string SINGLE_BOOL = "boolean";
        public const string SINGLE_STRING = "string";
        public const string SINGLE_MULTILINES = "multilines";
        public const string SINGLE_COLOR = "color";
        public const string SINGLE_ENUM = "someenum";
        public const string SINGLE_FILE_PATH = "file_path";
        public const string SINGLE_TILE = "tile";
        public const string SINGLE_ENTITY_REF = "entity_ref";
        public const string SINGLE_POINT = "point";
        
        public const string ARRAY_INT = "integer_array";
        public const string ARRAY_FLOAT = "float_array";
        public const string ARRAY_BOOL = "boolean_array";
        public const string ARRAY_STRING = "string_array";
        public const string ARRAY_MULTILINES = "multilines_array";
        public const string ARRAY_COLOR = "color_array";
        public const string ARRAY_ENUM = "someenum_array";
        public const string ARRAY_FILE_PATH = "file_path_array";
        public const string ARRAY_TILE = "tile_array";
        public const string ARRAY_ENTITY_REF = "entity_ref_array";
        public const string ARRAY_POINT = "point_array";

        public static readonly string[] Singles = new[]
        {
            SINGLE_INT,
            SINGLE_FLOAT,
            SINGLE_BOOL,
            SINGLE_STRING,
            SINGLE_MULTILINES,
            SINGLE_COLOR,
            SINGLE_ENUM,
            SINGLE_FILE_PATH,
            SINGLE_TILE,
            SINGLE_ENTITY_REF,
            SINGLE_POINT,
        };

        public static readonly string[] Arrays = new[]
        {
            ARRAY_INT,
            ARRAY_FLOAT,
            ARRAY_BOOL,
            ARRAY_STRING,
            ARRAY_MULTILINES,
            ARRAY_COLOR,
            ARRAY_ENUM,
            ARRAY_FILE_PATH,
            ARRAY_TILE,
            ARRAY_ENTITY_REF,
            ARRAY_POINT,
        };
        
        public static readonly string[] All = new[]
        {
            SINGLE_INT,
            SINGLE_FLOAT,
            SINGLE_BOOL,
            SINGLE_STRING,
            SINGLE_MULTILINES,
            SINGLE_ENUM,
            SINGLE_COLOR,
            SINGLE_POINT,
            SINGLE_FILE_PATH,
            SINGLE_ENTITY_REF,
            SINGLE_TILE,
            ARRAY_INT,
            ARRAY_FLOAT,
            ARRAY_BOOL,
            ARRAY_STRING,
            ARRAY_MULTILINES,
            ARRAY_ENUM,
            ARRAY_COLOR,
            ARRAY_POINT,
            ARRAY_FILE_PATH,
            ARRAY_ENTITY_REF,
            ARRAY_TILE,
        };
        
        public static readonly Dictionary<string, string> ExpectedValuesAsString = new Dictionary<string, string>()
        {
            { SINGLE_INT, "5" },
            { SINGLE_FLOAT, "1.2345" },
            { SINGLE_BOOL, "true" },
            { SINGLE_STRING, "string" },
            { SINGLE_MULTILINES, "test string\nline\nyes" },
            { SINGLE_COLOR, "842626FF" },
            { SINGLE_ENUM, "Alpha" },
            { SINGLE_FILE_PATH, "filepath.txt" },
            { SINGLE_TILE, "FixtureSprite" },
            { SINGLE_ENTITY_REF, "cf43dc80-66b0-11ec-ad88-697b5103d010" },
            { SINGLE_POINT, "(124.2, 123.45)" },
            
            { ARRAY_INT, "0, 5" },
            { ARRAY_FLOAT, "0, 0.435" },
            { ARRAY_BOOL, "false, true" },
            { ARRAY_STRING, ", test the string" },
            { ARRAY_MULTILINES, ", dfjgvbkjdfgnd\ngfd\ngd\nfgfdgfdgf" },
            { ARRAY_COLOR, "000000FF, A92D2DFF" },
            { ARRAY_ENUM, "Null, Omega" },
            { ARRAY_FILE_PATH, ", TestAllFields.ldtk" },
            { ARRAY_TILE, ", FixtureSprite" },
            { ARRAY_ENTITY_REF, ", cf43dc80-66b0-11ec-ad88-697b5103d010"  },
            { ARRAY_POINT, "(8.5, -2.5), (4.5, -2.5)" },
        };
        
        public static readonly Dictionary<string, string> ExpectedValuesAsStringForToc = new Dictionary<string, string>()
        {
            { SINGLE_INT, "12345" },
            { SINGLE_FLOAT, "1.235" },
            { SINGLE_BOOL, "true" },
            { SINGLE_STRING, "test the string" },
            { SINGLE_MULTILINES, "test the  string" },
            { SINGLE_COLOR, "298DABFF" },
            { SINGLE_ENUM, "Omega" },
            { SINGLE_FILE_PATH, "TestAllFields.ldtk.meta" },
            { SINGLE_TILE, "SunnyLand_288_0_16_16" },
            { SINGLE_ENTITY_REF, "" },
            { SINGLE_POINT, "(2, 0)" },
            
            { ARRAY_INT, "123, 0, 456" },
            { ARRAY_FLOAT, "1912276, 0, -188157.1" },
            { ARRAY_BOOL, "true, false, false" },
            { ARRAY_STRING, "test string, , test test test" },
            { ARRAY_MULTILINES, "testing the string test  more test, , string test multi line paragraph" },
            { ARRAY_COLOR, "B40A0AFF, 000000FF, 0C33ADFF" },
            { ARRAY_ENUM, "Alpha, , Omega" },
            { ARRAY_FILE_PATH, "TestAllFields.ldtk, , TestAllFields.ldtk.meta" },
            { ARRAY_TILE, "SunnyLand_32_96_16_16, , SunnyLand_208_240_32_48, SunnyLand_128_0_16_16, SunnyLand_160_144_16_16, SunnyLand_160_144_16_16, SunnyLand_208_240_32_48" },
            { ARRAY_ENTITY_REF, ", , "  },
            { ARRAY_POINT, "(1, 0), (2, 1), (1, 2), (0, 1)" },
        };
        
        public static readonly Dictionary<string, object> ExpectedSingleValues = new Dictionary<string, object>()
        {
            { SINGLE_INT, 5 },
            { SINGLE_FLOAT, 1.2345f },
            { SINGLE_BOOL, true },
            { SINGLE_STRING, "string" },
            { SINGLE_MULTILINES, "test string\nline\nyes" },
            { SINGLE_COLOR, new Color(0.519f, 0.149f, 0.149f, 1.000f) },
            { SINGLE_ENUM, SomeEnum.Alpha },
            { SINGLE_FILE_PATH, "filepath.txt" },
            { SINGLE_TILE, FieldsFixture.LoadSprite() },
            { SINGLE_ENTITY_REF, null },
            { SINGLE_POINT, new Vector2(11f, 10f) },
        };
        public static readonly Dictionary<string, object[]> ExpectedArrayValues = new Dictionary<string, object[]>()
        {
            { ARRAY_INT, new object[]{0, 5} },
            { ARRAY_FLOAT, new object[]{0f , 0.435f} },
            { ARRAY_BOOL, new object[]{ false, true } },
            { ARRAY_STRING, new object[]{"", "test the string"} },
            { ARRAY_MULTILINES, new object[]{"", "dfjgvbkjdfgnd\ngfd\ngd\nfgfdgfdgf"} },
            { ARRAY_COLOR, new object[]{ new Color(0,0,0, 1), new Color(0.663f, 0.176f, 0.176f, 1.000f)} },
            { ARRAY_ENUM, new object[]{ SomeEnum.Null, SomeEnum.Omega} },
            { ARRAY_FILE_PATH, new object[]{"", "TestAllFields.ldtk"} },
            { ARRAY_TILE, new object[]{null, FieldsFixture.LoadSprite() } },
            { ARRAY_ENTITY_REF, new object[]{null, null} },
            { ARRAY_POINT, new object[]{new Vector2(9f, 9f), new Vector2(10f, 9f)} },
        };
    }
}