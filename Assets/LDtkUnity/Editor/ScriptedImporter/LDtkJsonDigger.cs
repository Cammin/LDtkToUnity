using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    //todo make fixtures and tests for all these.
    internal static class LDtkJsonDigger
    {
        private delegate bool JsonDigAction<T>(ref Utf8JsonReader reader, ref T result);
        
        //todo all of the data digging could be merged into one big json sweep, so that we are not starting multiple streams and can still get everything necessary for performance
        
        public static bool GetTilesetRelPaths(string projectPath, ref HashSet<string> result) => 
            DigIntoJson(projectPath, GetTilesetRelPathsReader, ref result);
        
        public static bool GetUsedEntities(string path, ref HashSet<string> result) => 
            DigIntoJson(path, GetUsedEntitiesReader, ref result);
        public static bool GetUsedIntGridValues(string path, ref HashSet<string> result) => 
            DigIntoJson(path, GetUsedIntGridValuesReader, ref result);
        public static bool GetUsedProjectLevelBackgrounds(string path, ref HashSet<string> result) => 
            DigIntoJson(path, GetUsedProjectLevelBackgroundsReader, ref result);
        public static bool GetUsedSeparateLevelBackgrounds(string path, ref string result) => 
            DigIntoJson(path, GetUsedSeparateLevelBackgroundReader, ref result);
        public static bool GetUsedFieldTiles(string levelPath, ref List<FieldInstance> result) => 
            DigIntoJson(levelPath, GetUsedFieldTilesReader, ref result);
        public static bool GetUsedTilesetSprites(string levelPath, ref Dictionary<string, HashSet<int>> result) => 
            DigIntoJson(levelPath, GetUsedTilesetSpritesReader, ref result);
        
        
        public static bool GetIsExternalLevels(string projectPath, ref bool result) => 
            DigIntoJson(projectPath, GetIsExternalLevelsInReader, ref result);  //todo validate that this works from a test framework test
        public static bool GetDefaultGridSize(string projectPath, ref int result) => 
            DigIntoJson(projectPath, GetDefaultGridSizeInReader, ref result); //todo setup test framework function for this
        public static bool GetJsonVersion(string projectPath, ref string result) => 
            DigIntoJson(projectPath, GetJsonVersionReader, ref result);


        private static bool DigIntoJson<T>(string path, JsonDigAction<T> digAction, ref T result)
        {
            Profiler.BeginSample($"DigIntoJson {digAction.Method.Name}");
            
            if (!File.Exists(path))
            {
                LDtkDebug.LogError($"Couldn't locate the file to dig into the json for at path: \"{path}\"");
                return false;
            }

            Profiler.BeginSample($"Dig ReadAllText");
            string text = File.ReadAllText(path);
            Profiler.EndSample();
            
            
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(text, Converter.Settings);
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            bool success = digAction.Invoke(ref reader, ref result);
            
            Profiler.EndSample();

            if (success)
            {
                //Debug.Log($"Dug json and got {result} for {actionThing.Method.Name} at {path}");
                return true;
            }
            
            LDtkDebug.LogError($"Issue digging into the json for {path} during: {digAction.Method.Name}");
            return false;
        }

        private static bool GetUsedEntitiesReader(ref Utf8JsonReader reader, ref HashSet<string> entities)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "entityInstances")
                    continue;

                int entityInstancesDepth = reader.CurrentDepth;
                while (reader.CurrentDepth >= entityInstancesDepth && reader.Read())
                {
                    if (reader.CurrentDepth != entityInstancesDepth + 2 || reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "__identifier")
                        continue;

                    reader.Read();
                    entities.Add(reader.GetString());
                }
            }
            return true;
        }
        
        private static bool GetUsedIntGridValuesReader(ref Utf8JsonReader reader, ref HashSet<string> result)
        {
            string recentIdentifier = "";
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "__identifier")
                {
                    reader.Read();
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        recentIdentifier = reader.GetString();
                    }
                }

                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "intGridCsv")
                {
                    continue;
                }          
                
                //Debug.Log($"IntGridCsv property at {ReaderInfo(reader)}");

                reader.Read();
                Debug.Assert(reader.TokenType == JsonTokenType.StartArray, $"not start array at {ReaderInfo(ref reader)}");

                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    Debug.Assert(!string.IsNullOrEmpty(recentIdentifier), $"Didnt have a valid identifier when constructing the IntGrid value: {reader.TokenType}");
                    long intGridValue = reader.GetInt32();
                    string formattedString = LDtkKeyFormatUtil.IntGridValueFormat(recentIdentifier, intGridValue.ToString());
                    //Debug.Log($"Try Add {formattedString}");

                    result.Add(formattedString);
                }
                
                Debug.Assert(reader.TokenType == JsonTokenType.EndArray, $"not end array at {ReaderInfo(ref reader)}");
                //Debug.Log($"we hit end array at {ReaderInfo(reader)}");
            }
            
            return true;
        }
        
        private static bool GetUsedProjectLevelBackgroundsReader(ref Utf8JsonReader reader, ref HashSet<string> result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "bgRelPath")
                {
                    continue;
                }
                
                reader.Read();
                Debug.Assert(reader.TokenType == JsonTokenType.String || reader.TokenType == JsonTokenType.Null, $"Not expected value when getting level background path at {ReaderInfo(ref reader)}");

                string value = reader.GetString();
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }
            }
            return true;
        }
        private static bool GetUsedSeparateLevelBackgroundReader(ref Utf8JsonReader reader, ref string result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "bgRelPath")
                {
                    continue;
                }

                reader.Read();
                Debug.Assert(reader.TokenType == JsonTokenType.String || reader.TokenType == JsonTokenType.Null, $"Not expected value when getting level background path at {ReaderInfo(ref reader)}");

                result = reader.GetString();
                return true;
            }
            return false;
        }
        
        private static string ReaderInfo(ref Utf8JsonReader reader)
        {
            return $"{reader.Position}, TokenType:{reader.TokenType}, Value:{reader.ValueSpan.ToString()}";
        }
        
        private static bool GetTilesetRelPathsReader(ref Utf8JsonReader reader, ref HashSet<string> textures)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "tilesets")
                {
                    continue;
                }

                int depth = reader.CurrentDepth;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "relPath")
                    {
                        string value = reader.GetString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            textures.Add(value);
                        }
                    }

                    if (reader.CurrentDepth < depth)
                    {
                        return true; //there only one instance of the tilesets array in the definitions; we can return after we leave the depth of the tilesets 
                    }
                }
            }

            
            return true;
        }
        
        private static bool GetIsExternalLevelsInReader(ref Utf8JsonReader reader, ref bool result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "externalLevels")
                    continue;

                result = reader.GetBoolean();
                return true;
            }
            return false;
        }

        private static bool GetDefaultGridSizeInReader(ref Utf8JsonReader reader, ref int result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "defaultGridSize")
                    continue;
                
                result = reader.GetInt32();
                return true;
            }
            return false;
        }
        
        private static bool GetUsedFieldTilesReader(ref Utf8JsonReader reader, ref List<FieldInstance> result)
        {
            //a field instance: { "__identifier": "integer", "__value": 12345, "__type": "Int", "__tile": null, "defUid": 105, "realEditorValues": [{ "id": "V_Int", "params": [12345] }] },
            //"fieldInstances": [{ "__identifier": "LevelTile", "__value": { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }, "__type": "Tile", "__tile": { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }, "defUid": 164, "realEditorValues": [{"id": "V_String","params": ["96,32,32,16"]}] }]
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "fieldInstances")
                {
                    continue;
                }
                DigIntoFieldInstances(ref reader, result);
            }
            return true;
        }
        
        private static bool GetUsedTilesetSpritesReader(ref Utf8JsonReader reader, ref Dictionary<string, HashSet<int>> usedTileIds)
        {
            // In a layer, contains AutoLayerTiles and GridTiles.
            //TileInstance contains the rect, but also a T value that might be usable. 

            while (reader.Read())
            {
                //1. find a layer instance and record the name of the layer. we could encounter the same layer name again, so track the dictionary accordingly.
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "layerInstances")
                {
                    continue;
                }
                
                //at this point in time: We've just found the layer instances array.
                //need to see if the layer has start objects or if it's an empty array.
                int arrayDepth = reader.CurrentDepth;

                //Debug.Log($"found layer instances for {reader.Path}");
                reader.Read();
                

                //this while loop is looking for the end of the LayerInstances array
                Debug.Assert(reader.TokenType == JsonTokenType.StartArray, ReaderInfo(ref reader));

                bool brokeOutOfEndArray = false;
                //this while look is to look into the LayerInstances array. it is always either one of two things: en end array or start object.
                while (!brokeOutOfEndArray && reader.Read() && reader.TokenType != JsonTokenType.EndArray && reader.CurrentDepth != arrayDepth)
                {
                    //do doubt we should be encountering a start object of a layer instance
                    int objectDepth = reader.CurrentDepth;
                    
                    Debug.Assert(reader.TokenType == JsonTokenType.StartObject, ReaderInfo(ref reader));

                    //no doubt we are encountering the first item in the object, being __identifier
                    reader.Read();

                    //__identifier
                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "__identifier", ReaderInfo(ref reader));
                    string identifier = reader.GetString();
                    if (string.IsNullOrEmpty(identifier))
                    {
                        continue;
                    }

                    //2. Determine it's type.
                    //__type
                    reader.Read();
                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "__type", ReaderInfo(ref reader));
                    string type = reader.GetString();
                    
                    //3. Depending on type, go into the appropriate tile array. AutoLayer/IntGrid: AutoLayerTiles, TilesLayer: GridTiles
                    //(possible values: IntGrid, Entities, Tiles or AutoLayer)
                    if (type == "Entities")
                    {
                        SearchUntilEnd(ref reader, objectDepth, arrayDepth, ref brokeOutOfEndArray);
                        continue;
                    }
                    
                    HashSet<int> tileIds;
                    if (usedTileIds.ContainsKey(identifier))
                    {
                        tileIds = usedTileIds[identifier];
                    }
                    else
                    {
                        tileIds = new HashSet<int>();
                        usedTileIds.Add(identifier, tileIds);
                    }

                    string arrayToSearch = null;
                    switch (type)
                    {
                        case "IntGrid": arrayToSearch = "autoLayerTiles"; break;
                        case "Tiles": arrayToSearch = "gridTiles"; break;
                        case "AutoLayer": arrayToSearch = "autoLayerTiles"; break;
                        default:
                            LDtkDebug.LogError($"Expected type wasn't properly encountered {type}");
                            break;
                    }
                    if (string.IsNullOrEmpty(arrayToSearch))
                    {
                        break;
                    }
                    
                    while (reader.Read())
                    {
                        if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != arrayToSearch)
                        {
                            continue;
                        }
                        break;
                    }
                    int tileArrayDepth = reader.CurrentDepth;
                    
                    Debug.Assert(reader.TokenType == JsonTokenType.PropertyName); //property name of the tiles array
                    reader.Read();
                    Debug.Assert(reader.TokenType == JsonTokenType.StartArray);
                    
                    //4. Keep on iterating through the array, grabbing every "t" value and adding to the hashset for this layer.
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray && reader.CurrentDepth == tileArrayDepth)
                        {
                            break;
                        }
                        
                        if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "t")
                        {
                            int? readAsInt32 = reader.GetInt32();
                            tileIds.Add(readAsInt32.Value);
                        } 
                    }

                    //we will now look for the next endObject so that the while loop can attempt again in case we hit another start array.
                    SearchUntilEnd(ref reader,objectDepth, arrayDepth, ref brokeOutOfEndArray);
                    
                    
                    //when exiting out, we should be in a state of having been to the next array element, or we reached the end of the array and we can then exit out of the array and look for the next layerInstances somewhere.
                    //end object
                    //5. After reaching the end of the tiles array in this layer instance, try to find another object within this array, else exit out.
                }
            }
            
            return true;
        }
        
        private static void SearchUntilEnd(ref Utf8JsonReader reader, int i, int arrayDepth1, ref bool b)
        {
            while (reader.Read())
            {
                if (reader.CurrentDepth == i && reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.CurrentDepth == arrayDepth1 && reader.TokenType == JsonTokenType.EndArray)
                {
                    b = true;
                    break;
                }
            }
        }

        private static bool GetJsonVersionReader(ref Utf8JsonReader reader, ref string result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "jsonVersion")
                {
                    continue;
                }

                result = reader.GetString();
                return true;
            }
            
            return false;
        }

        private static void DigIntoFieldInstances(ref Utf8JsonReader reader, List<FieldInstance> result)
        {
            int arrayDepth = reader.CurrentDepth;
            while (reader.Read() && !(reader.CurrentDepth == arrayDepth && reader.TokenType == JsonTokenType.EndArray)) //ends when we reach the end of this entityInstances array
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    continue;

                //_identifier name
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "__identifier")
                    continue;

                FieldInstance field = new FieldInstance();

                //__identifier value
                field.Identifier = reader.GetString();
                Debug.Assert(reader.TokenType == JsonTokenType.String, ReaderInfo(ref reader));

                //__value name
                reader.Read();
                Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "__value", ReaderInfo(ref reader));

                
                // Example Possibilities at this point:
                // { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }
                // null
                // [ null, null ]
                // [ { "tilesetUid": 149, "x": 32, "y": 96, "w": 16, "h": 16 }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]
                
                //start object or start array. it's also possible it's null, and in which that case, then we're done digging in this one.
                reader.Read();
                if (reader.TokenType == JsonTokenType.Null)
                {
                    continue;
                }
                
                // Example Possibilities at this point:
                // { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }
                // [ null, null ]
                // [ { "tilesetUid": 149, "x": 32, "y": 96, "w": 16, "h": 16 }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]
                
                bool isArray = reader.TokenType == JsonTokenType.StartArray;
                int valuesArrayDepth = reader.CurrentDepth;
                if (isArray)
                {
                    //object begin
                    reader.Read();
                }

                //this is an easy exit to certify if this is a tile or not.
                if (!(reader.TokenType == JsonTokenType.StartObject || reader.TokenType == JsonTokenType.Null))
                {
                    //was a non-tile thing.
                    //Debug.Log($"Field not Tile: {ReaderInfo()}");
                    continue;
                }
                
                List<TilesetRectangle> rects = new List<TilesetRectangle>();

                while (GetTileObject(ref reader, rects, isArray, valuesArrayDepth))
                {
                    reader.Read();
                }

                if (isArray)
                {
                    //while (reader.TokenType != JsonTokenType.EndArray && reader.Read()) { }
                    
                    Debug.Assert(reader.TokenType == JsonTokenType.EndArray, ReaderInfo(ref reader));
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
                Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "__type", ReaderInfo(ref reader));

                //__type value
                field.Type = reader.GetString();
                Debug.Assert(reader.TokenType == JsonTokenType.String, ReaderInfo(ref reader));
                Debug.Assert(field.Type == "Tile" || field.Type == "Array<Tile>", ReaderInfo(ref reader));

                //continually skipping the tile field until finding the defUid.
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "defUid")
                    {
                        break;
                    }
                }

                //defUid name
                Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "defUid", ReaderInfo(ref reader));

                //defUid value
                field.DefUid = reader.GetInt32();

                //Debug.Log($"Created tile field instance! {field}");
                result.Add(field);
            }
        }

        private static bool GetTileObject(ref Utf8JsonReader reader, List<TilesetRectangle> rects, bool isArray, int endArrayDepth)
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
            
            
            if (reader.TokenType == JsonTokenType.Null) //if it's null, skip to the next one 
            {
                //Debug.Log($"This field was null, it's valid to possibly get next element! {ReaderInfo()}");
                return true;
            }
            
            //start object
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                //Debug.Log($"exit the array loop, this is not a start object. {ReaderInfo()}");
                
                //was not a start object, so it's definitely not a tile field here. just in case it was something completely different, we need to work all the way through until the next property name within the same depth
                //we not need to work until the end of the object or array before we return false
                WorkUntilEndOfArray(ref reader, isArray, endArrayDepth);
                return false;
            }
            
            //tilesetUid name
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "tilesetUid")
            {
                //Debug.Log($"expected tileset uid but was not, exit out of trying to get tileset rects. {ReaderInfo()}");
                WorkUntilEndOfArray(ref reader, isArray, endArrayDepth);
                return false;
            }

            TilesetRectangle rect = new TilesetRectangle();

            //tilesetUid value
            rect.TilesetUid = reader.GetInt32();

            //x name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "x", ReaderInfo(ref reader));
            //x value
            rect.X = reader.GetInt32();

            //y name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "y", ReaderInfo(ref reader));
            //y value
            rect.Y = reader.GetInt32();

            //w name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "w", ReaderInfo(ref reader));
            //w value
            rect.W = reader.GetInt32();

            //h name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "h", ReaderInfo(ref reader));
            //h value
            rect.H = reader.GetInt32();

            //end object
            reader.Read();
            Debug.Assert(reader.TokenType == JsonTokenType.EndObject, ReaderInfo(ref reader));
            
            // Example Possibilities at this point:
            // }
            // }, null, { "tilesetUid": 149, "x": 208, "y": 240, "w": 32, "h": 48 } ]

            //Debug.Log($"Success getting Tileset Rect: uid:{rect.TilesetUid} x:{rect.X} y:{rect.Y} w:{rect.W} h:{rect.H}");
            rects.Add(rect);
            
            return true;
        }
        
        private static void WorkUntilEndOfArray(ref Utf8JsonReader reader, bool isArray, int endArrayDepth)
        {
            if (!isArray || reader.TokenType == JsonTokenType.EndArray)
            {
                return;
            }
                
            //Debug.Log($"StartWorkUntilEndOfArray {ReaderInfo()}");
                

            while (!HasFoundArrayEnd(ref reader, endArrayDepth) && reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    //Debug.Log($"Found EndArray at {ReaderInfo()}. CurrentDepth: {reader.CurrentDepth}. TargetDepth is {endArrayDepth}");
                }
            }

            //Debug.Log($"WorkUntilEndOfArray {ReaderInfo()}");
        }

        private static bool HasFoundArrayEnd(ref Utf8JsonReader reader, int endArrayDepth)
        {
            return reader.CurrentDepth == endArrayDepth && reader.TokenType == JsonTokenType.EndArray;
        }

        private static void LogRemainingReader(ref Utf8JsonReader reader)
        {
            while (reader.Read())
            {
                object value = reader.ValueSpan.ToString();
                
                string msg = "";
                {
                    string tokenText = Colorize(reader.TokenType.ToString(), "blue");
                    string valueText = Colorize(value.ToString(), "navy");
                    msg += $"{GetDepthString(ref reader)} {tokenText} {valueText}";
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    reader.Read();
                    value = reader.GetString();
                    string tokenText = Colorize(reader.TokenType.ToString(), "blue");

                    string nulNull = value != null ? value.ToString() : "null";
                    string valueText = Colorize(nulNull, "navy");
                    
                    msg += $"{tokenText} {valueText}";
                }
                
                //msg = $"{GetDepthString(reader)} : {}";

                LDtkDebug.Log(msg);
                
            }
        }

        private static string GetDepthString(ref Utf8JsonReader reader)
        {
            string depth = Colorize(reader.CurrentDepth.ToString("00"), "teal");

            for (int i = 0; i < reader.CurrentDepth; i++)
            {
                depth += "|       ";
            }

            return depth;
        }

        private static string Colorize(string text, string color) => $"<color={color}>{text}</color>";
        
    }
}