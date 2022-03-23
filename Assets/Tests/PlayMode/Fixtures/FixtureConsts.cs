using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
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
        
        public static Dictionary<string, string> ExpectedValuesAsString = new Dictionary<string, string>()
        {
            { SINGLE_INT, "5" },
            { SINGLE_FLOAT, "1.2345" },
            { SINGLE_BOOL, "true" },
            { SINGLE_STRING, "string" },
            { SINGLE_MULTILINES, "test string\nline\nyes" },
            { SINGLE_COLOR, "(132, 38, 38)" },
            { SINGLE_ENUM, "Alpha" },
            { SINGLE_FILE_PATH, "filepath.txt" },
            { SINGLE_TILE, "e00d0db7c27cad447850c6725ff413f2" },
            { SINGLE_ENTITY_REF, "5" },
            { SINGLE_POINT, "(124.2, 123.45)" },

            { ARRAY_INT, "5, 0" },
            { ARRAY_FLOAT, "1.2345, 0" },
            { ARRAY_BOOL, "false, true" },
            { ARRAY_STRING, "test the string, " },
            { ARRAY_MULTILINES, "5" },
            { ARRAY_COLOR, "5" },
            { ARRAY_ENUM, "Omega" },
            { ARRAY_FILE_PATH, "5" },
            { ARRAY_TILE, "5" },
            { ARRAY_ENTITY_REF, "5" },
            { ARRAY_POINT, "5" },
        };
        public static Dictionary<string, object> ExpectedValues = new Dictionary<string, object>()
        {
            { SINGLE_INT, 5 },
            { SINGLE_FLOAT, 1.2345f },
            { SINGLE_BOOL, true },
            { SINGLE_STRING, "string" },
            { SINGLE_MULTILINES, "test string\nline\nyes" },
            { SINGLE_COLOR, new Color(132, 38, 38) },
            { SINGLE_ENUM, "Alpha" },
            { SINGLE_FILE_PATH, "filepath.txt" },
            { SINGLE_TILE, "" },
            { SINGLE_ENTITY_REF, "5" },
            { SINGLE_POINT, "(124.2, 123.45)" },

            { ARRAY_INT, "5, 0" },
            { ARRAY_FLOAT, "1.2345, 0" },
            { ARRAY_BOOL, "false, true" },
            { ARRAY_STRING, "test the string, " },
            { ARRAY_MULTILINES, "5" },
            { ARRAY_COLOR, "5" },
            { ARRAY_ENUM, "Omega" },
            { ARRAY_FILE_PATH, "5" },
            { ARRAY_TILE, "5" },
            { ARRAY_ENTITY_REF, "5" },
            { ARRAY_POINT, "5" },
        };

        private static object GetTileRef()
        {
            string guid = "e00d0db7c27cad447850c6725ff413f2";
            string guidToAssetPath = AssetDatabase.GUIDToAssetPath(guid);
            ObjectIdentifier objectIdentifier = new ObjectIdentifier();
            //ObjectIdentifier.TryGetObjectIdentifier(guid);
            Debug.Log(guidToAssetPath);
            return AssetDatabase.LoadAssetAtPath<Sprite>(guidToAssetPath);
        }
    }
}