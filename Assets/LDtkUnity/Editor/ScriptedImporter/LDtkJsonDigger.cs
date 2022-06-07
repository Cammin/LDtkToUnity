using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    //todo make fixtures and tests for all these.
    public static class LDtkJsonDigger
    {
        private delegate bool JsonDigAction<T>(JsonTextReader reader, out T result);
        
        //todo all of the data digging could be merged into one big json sweep, so that we are not starting multiple streams and can still get everything necessary for performance
        
        public static bool GetUsedEntities(string path, out IEnumerable<string> result) => 
            DigIntoJson(path, GetUsedEntitiesReader, out result);
        public static bool GetUsedIntGridValues(string path, out IEnumerable<string> result) => 
            DigIntoJson(path, GetUsedIntGridValuesReader, out result);
        public static bool GetTilesetRelPaths(string projectPath, out IEnumerable<string> result) => 
            DigIntoJson(projectPath, GetTilesetRelPathsReader, out result);
        public static bool GetIsExternalLevels(string projectPath, out bool result) => 
            DigIntoJson(projectPath, GetIsExternalLevelsInReader, out result);  //todo validate that this works from a test framework test
        public static bool GetDefaultGridSize(string projectPath, out int result) => 
            DigIntoJson(projectPath, GetDefaultGridSizeInReader, out result); //todo setup test framework function for this
        public static bool GetUsedFieldTiles(string levelPath, out List<FieldInstance> result) => 
            DigIntoJson(levelPath, GetUsedFieldTilesReader, out result);
        public static bool GetUsedTilesetSprites(string levelPath, out List<FieldInstance> result) => 
            DigIntoJson(levelPath, GetUsedTilesetSpritesReader, out result); //todo for optimising how many sprites should be made and also for optimising spriteatlassize

        private static bool DigIntoJson<T>(string path, JsonDigAction<T> digAction, out T result)
        {
            Profiler.BeginSample($"DigIntoJson {digAction.Method.Name}");
            
            if (!File.Exists(path))
            {
                result = default;
                Debug.LogError("Couldn't locate the file to dig into the json for.");
                return false;
            }
            
            StreamReader sr = File.OpenText(path);
            bool success;
            result = default;
            try
            {
                JsonTextReader reader = new JsonTextReader(sr);
                success = digAction.Invoke(reader, out result);
            }
            finally
            {
                sr.Close();
            }
            
            Profiler.EndSample();

            if (success)
            {
                //Debug.Log($"Dug json and got {result} for {actionThing.Method.Name} at {path}");
                return true;
            }
            
            LDtkDebug.LogError($"Issue digging into the json for {path}");
            return false;
        }

        private static bool GetUsedEntitiesReader(JsonTextReader reader, out IEnumerable<string> result)
        {
            HashSet<string> entities = new HashSet<string>();
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "entityInstances")
                    continue;

                int entityInstancesDepth = reader.Depth;
                while (reader.Depth >= entityInstancesDepth && reader.Read())
                {
                    if (reader.Depth != entityInstancesDepth + 2 || reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "__identifier")
                        continue;

                    reader.Read();
                    entities.Add((string)reader.Value);
                }
            }

            result = entities;
            return true;
        }
        
        private static bool GetUsedIntGridValuesReader(JsonTextReader reader, out IEnumerable<string> result)
        {
            HashSet<string> intGridValues = new HashSet<string>();
            string recentIdentifier = "";
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__identifier")
                {
                    reader.Read();
                    if (reader.TokenType == JsonToken.String)
                    {
                        recentIdentifier = (string)reader.Value;
                        //Debug.Log($"Recent identifier as {recentIdentifier}");
                    }
                }

                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "intGridCsv")
                {
                    continue;
                }          
                
                //Debug.Log($"IntGridCsv property at {ReaderInfo(reader)}");

                reader.Read();
                Debug.Assert(reader.TokenType == JsonToken.StartArray, $"not start array at {ReaderInfo(reader)}");

                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    Debug.Assert(!string.IsNullOrEmpty(recentIdentifier), $"Didnt have a valid identifier when contructing the intgrid value: {reader.TokenType}");
                    long intGridValue = (long)reader.Value;
                    string formattedString = LDtkKeyFormatUtil.IntGridValueFormat(recentIdentifier, intGridValue.ToString());
                    //Debug.Log($"Try Add {formattedString}");

                    intGridValues.Add(formattedString);
                }
                
                Debug.Assert(reader.TokenType == JsonToken.EndArray, $"not end array at {ReaderInfo(reader)}");
                //Debug.Log($"we hit end array at {ReaderInfo(reader)}");
            }

            result = intGridValues;
            return true;
        }
        
        private static string ReaderInfo(JsonTextReader reader)
        {
            return $"{reader.LineNumber}:{reader.LinePosition}, TokenType:{reader.TokenType}, Value:{reader.Value}";
        }
        
        private static bool GetTilesetRelPathsReader(JsonTextReader reader, out IEnumerable<string> result)
        {
            HashSet<string> textures = new HashSet<string>();
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "tilesets")
                {
                    continue;
                }

                int depth = reader.Depth;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "relPath")
                    {
                        string value = reader.ReadAsString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            textures.Add(value);
                        }
                    }

                    if (reader.Depth < depth)
                    {
                        result = textures;
                        return true; //there only one instance of the tilesets array in the definitions; we can return after we leave the depth of the tilesets 
                    }
                }
            }

            
            result = textures;
            return true;
        }
        
        private static bool GetIsExternalLevelsInReader(JsonTextReader reader, out bool result)
        {
            result = false;
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "externalLevels")
                    continue;

                bool? value = reader.ReadAsBoolean();
                if (value == null)
                    break;
                
                result = value.Value;
                return true;
            }
            return false;
        }

        private static bool GetDefaultGridSizeInReader(JsonTextReader reader, out int result)
        {
            result = 0;
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "defaultGridSize")
                    continue;

                int? value = reader.ReadAsInt32();
                if (value == null)
                    break;
                
                result = value.Value;
                return true;
            }
            return false;
        }
        
        private static bool GetUsedFieldTilesReader(JsonTextReader reader, out List<FieldInstance> result)
        {
            //a field instance: { "__identifier": "integer", "__value": 12345, "__type": "Int", "__tile": null, "defUid": 105, "realEditorValues": [{ "id": "V_Int", "params": [12345] }] },
            //"fieldInstances": [{ "__identifier": "LevelTile", "__value": { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }, "__type": "Tile", "__tile": { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }, "defUid": 164, "realEditorValues": [{"id": "V_String","params": ["96,32,32,16"]}] }]
            result = new List<FieldInstance>();
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "fieldInstances")
                {
                    continue;
                }
                DigIntoFieldInstances(reader, result);
            }
            return true;
        }
        
        private static bool GetUsedTilesetSpritesReader(JsonTextReader reader, out List<FieldInstance> result)
        {
            result = new List<FieldInstance>();
            return true;
        }

        private static void DigIntoFieldInstances(JsonTextReader reader, List<FieldInstance> result)
        {
            string ReaderInfo()
            {
                return $"{reader.LineNumber}:{reader.LinePosition}, TokenType:{reader.TokenType}, Value:{reader.Value}";
            }
            
            int arrayDepth = reader.Depth;
            while (reader.Read() && !(reader.Depth == arrayDepth && reader.TokenType == JsonToken.EndArray)) //ends when we reach the end of this entityinstances array
            {
                if (reader.TokenType != JsonToken.StartObject)
                    continue;

                //_identifier name
                reader.Read();
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "__identifier")
                    continue;

                FieldInstance field = new FieldInstance();

                //__identifier value
                field.Identifier = reader.ReadAsString();
                Assert.IsTrue(reader.TokenType == JsonToken.String, ReaderInfo());

                //__value name
                reader.Read();
                Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__value", ReaderInfo());

                
                // Example Possibilities at this point:
                // { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }
                // null
                // [ null, null ]
                // [ { "tilesetUid": 149, "x": 32, "y": 96, "w": 16, "h": 16 }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]
                
                //start object or start array. it's also possible it's null, and in which that case, then we're done digging in this one.
                reader.Read();
                if (reader.TokenType == JsonToken.Null)
                {
                    continue;
                }
                
                // Example Possibilities at this point:
                // { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }
                // [ null, null ]
                // [ { "tilesetUid": 149, "x": 32, "y": 96, "w": 16, "h": 16 }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]
                
                bool isArray = reader.TokenType == JsonToken.StartArray;
                int valuesArrayDepth = reader.Depth;
                if (isArray)
                {
                    //object begin
                    reader.Read();
                }

                //this is an easy exit to certify if this is a tile or not.
                if (!(reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null))
                {
                    //was a non-tile thing.
                    //Debug.Log($"Field not Tile: {ReaderInfo()}");
                    continue;
                }
                
                List<TilesetRectangle> rects = new List<TilesetRectangle>();

                while (GetTileObject(reader, rects, isArray, valuesArrayDepth))
                {
                    reader.Read();
                }

                if (isArray)
                {
                    //while (reader.TokenType != JsonToken.EndArray && reader.Read()) { }
                    
                    Debug.Assert(reader.TokenType == JsonToken.EndArray, ReaderInfo());
                    reader.Read();
                }

                field.Value = rects.ToArray();

                //Debug.Log($"Got values: {rects.Count}");
                if (rects.Count == 0)
                {
                    //didn't get anything, not work parsing the rest basically
                    continue;
                }

                //__type name
                Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__type", ReaderInfo());

                //__type value
                field.Type = reader.ReadAsString();
                Assert.IsTrue(reader.TokenType == JsonToken.String, ReaderInfo());
                Assert.IsTrue(field.Type == "Tile" || field.Type == "Array<Tile>", ReaderInfo());

                //continually skipping the tile field until finding the defUid.
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "defUid")
                    {
                        break;
                    }
                }

                //defUid name
                Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "defUid", ReaderInfo());

                //defUid value
                field.DefUid = reader.ReadAsInt32().Value;
                Assert.IsTrue(reader.TokenType == JsonToken.Integer, ReaderInfo());

                //Debug.Log($"Created tile field instance! {field}");
                result.Add(field);
            }
        }

        private static bool GetTileObject(JsonTextReader reader, List<TilesetRectangle> rects, bool isArray, int endArrayDepth)
        {
            //Debug.Log("Get tile");
            //by this point in time we've already hit either null, or start object, or something else.
            
            // Example Possibilities at this point:
            // { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }
            // null, null ]
            // { "tilesetUid": 149, "x": 32, "y": 96, "w": 16, "h": 16 }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]
            // null, 0.435 ]
            // 1.2, null, 0.435 ]
            // 0.435 ]
            
            
            if (reader.TokenType == JsonToken.Null) //if it's null, skip to the next one 
            {
                //Debug.Log($"This field was null, it's valid to possibly get next element! {ReaderInfo()}");
                return true;
            }
            
            //start object
            if (reader.TokenType != JsonToken.StartObject)
            {
                //Debug.Log($"exit the array loop, this is not a start object. {ReaderInfo()}");
                
                //was not a start object, so it's definitely not a tile field here. just in case it was something completely different, we need to work all the way through until the next property name within the same depth
                //we not need to work until the end of the object or array before we return false
                WorkUntilEndOfArray();
                return false;
            }

            void WorkUntilEndOfArray()
            {
                if (!isArray || reader.TokenType == JsonToken.EndArray)
                {
                    return;
                }
                
                //Debug.Log($"StartWorkUntilEndOfArray {ReaderInfo()}");
                
                
                bool HasFoundArrayEnd() => reader.Depth == endArrayDepth && reader.TokenType == JsonToken.EndArray;
                while (!HasFoundArrayEnd() && reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndArray)
                    {
                        //Debug.Log($"Found EndArray at {ReaderInfo()}. Depth: {reader.Depth}. TargetDepth is {endArrayDepth}");
                    }
                }

                //Debug.Log($"WorkUntilEndOfArray {ReaderInfo()}");
            }

            //tilesetUid name
            reader.Read();
            if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "tilesetUid")
            {
                //Debug.Log($"expected tileset uid but was not, exit out of trying to get tileset rects. {ReaderInfo()}");
                WorkUntilEndOfArray();
                return false;
            }

            TilesetRectangle rect = new TilesetRectangle();

            //tilesetUid value
            rect.TilesetUid = reader.ReadAsInt32().Value;
            Assert.IsTrue(reader.TokenType == JsonToken.Integer, ReaderInfo());

            //x name
            reader.Read();
            Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "x", ReaderInfo());
            //x value
            rect.X = reader.ReadAsInt32().Value;
            Assert.IsTrue(reader.TokenType == JsonToken.Integer);

            //y name
            reader.Read();
            Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "y", ReaderInfo());
            //y value
            rect.Y = reader.ReadAsInt32().Value;
            Assert.IsTrue(reader.TokenType == JsonToken.Integer);

            //w name
            reader.Read();
            Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "w", ReaderInfo());
            //w value
            rect.W = reader.ReadAsInt32().Value;
            Assert.IsTrue(reader.TokenType == JsonToken.Integer);

            //h name
            reader.Read();
            Assert.IsTrue(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "h", ReaderInfo());
            //h value
            rect.H = reader.ReadAsInt32().Value;
            Assert.IsTrue(reader.TokenType == JsonToken.Integer, ReaderInfo());

            //end object
            reader.Read();
            Assert.IsTrue(reader.TokenType == JsonToken.EndObject, ReaderInfo());
            
            // Example Possibilities at this point:
            // }
            // }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]

            //Debug.Log($"Success getting Tileset Rect: uid:{rect.TilesetUid} x:{rect.X} y:{rect.Y} w:{rect.W} h:{rect.H}");
            rects.Add(rect);
            
            return true;
            
            string ReaderInfo()
            {
                return $"{reader.LineNumber}:{reader.LinePosition} {reader.TokenType} {reader.Value}";
            }
        }

        private static void LogRemainingReader(JsonTextReader reader)
        {
            while (reader.Read())
            {
                object value = reader.Value;
                if (value != null)
                {
                    string msg = "";
                    {
                        string tokenText = Colorize(reader.TokenType.ToString(), "blue");
                        string valueText = Colorize(value.ToString(), "navy");
                        msg += $"{GetDepthString(reader)} {tokenText} {valueText}";
                    }

                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        reader.Read();
                        value = reader.Value;
                        string tokenText = Colorize(reader.TokenType.ToString(), "blue");

                        string nulNull = value != null ? value.ToString() : "null";
                        string valueText = Colorize(nulNull, "navy");
                        
                        msg += $"{tokenText} {valueText}";
                    }
                    
                    //msg = $"{GetDepthString(reader)} : {}";

                    Debug.Log(msg);
                }
                else
                {
                    string tokenText = Colorize(reader.TokenType.ToString(), "orange");
                    string msg = $"{GetDepthString(reader)} {tokenText}";
                    Debug.Log(msg);
                }
            }
        }

        private static string GetDepthString(JsonTextReader reader)
        {
            string depth = Colorize(reader.Depth.ToString("00"), "teal");

            for (int i = 0; i < reader.Depth; i++)
            {
                depth += "|       ";
            }

            return depth;
        }

        private static string Colorize(string text, string color) => $"<color={color}>{text}</color>";

        
    }
}