using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity.Tests
{

    public static class FixtureConsts
    {
        public const string SINGLE_INT = "Integer";
        public const string SINGLE_FLOAT = "Float";
        public const string SINGLE_BOOL = "Boolean";
        public const string SINGLE_STRING = "String";
        public const string SINGLE_MULTILINES = "Multilines";
        public const string SINGLE_COLOR = "Color";
        public const string SINGLE_ENUM = "SomeEnum";
        public const string SINGLE_FILE_PATH = "FilePath";
        public const string SINGLE_TILE = "Tile";
        public const string SINGLE_ENTITY_REF = "EntityRef";
        public const string SINGLE_POINT = "Point";
        
        public const string ARRAY_INT = "IntegerArray";
        public const string ARRAY_FLOAT = "FloatArray";
        public const string ARRAY_BOOL = "BooleanArray";
        public const string ARRAY_STRING = "StringArray";
        public const string ARRAY_MULTILINES = "MultilinesArray";
        public const string ARRAY_COLOR = "ColorArray";
        public const string ARRAY_ENUM = "SomeEnumArray";
        public const string ARRAY_FILE_PATH = "FilePathArray";
        public const string ARRAY_TILE = "TileArray";
        public const string ARRAY_ENTITY_REF = "EntityRefArray";
        public const string ARRAY_POINT = "PointArray";

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
            { SINGLE_POINT, new Vector2(124.2f, 123.45f) },
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
            { ARRAY_POINT, new object[]{new Vector2(8.5f, -2.5f), new Vector2(4.5f, -2.5f)} },
        };
    }
}