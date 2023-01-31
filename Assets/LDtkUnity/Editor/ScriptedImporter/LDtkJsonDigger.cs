using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;
using Utf8Json;
using Utf8Json.Internal;
using JsonReader = Utf8Json.JsonReader;
using JsonSerializer = Utf8Json.JsonSerializer;
using JsonToken = Newtonsoft.Json.JsonToken;
using JsonWriter = Utf8Json.JsonWriter;

namespace LDtkUnity.Editor
{
    internal static class LDtkJsonDigger
    {
        private delegate bool JsonDigAction<T>(ref JsonReader reader, ref T result);
        private delegate bool JsonReadAction<T>(JsonTextReader reader, ref T result);
        
        //todo all of the data digging could be merged into one big json sweep, so that we are not starting multiple streams and can still get everything necessary for performance
        //todo also we might be able to cache the loaded bytes for multiple operations perhaps
        public static bool GetTilesetRelPaths(string projectPath, ref HashSet<string> result) => 
            DigIntoJson(projectPath, GetTilesetRelPathsReader, ref result);
        
        public static bool GetUsedEntities(string path, ref HashSet<string> result) => 
            DigIntoJson(path, GetUsedEntitiesReader, ref result);
        public static bool GetUsedIntGridValues(string path, ref HashSet<string> result) => 
            DigIntoJsonDead(path, GetUsedIntGridValuesReader, ref result);
        public static bool GetUsedProjectLevelBackgrounds(string path, ref HashSet<string> result) => 
            DigIntoJsonDead(path, GetUsedProjectLevelBackgroundsReader, ref result);
        public static bool GetUsedSeparateLevelBackgrounds(string path, ref string result) => 
            DigIntoJsonDead(path, GetUsedSeparateLevelBackgroundReader, ref result);
        public static bool GetUsedFieldTiles(string levelPath, ref List<FieldInstance> result) => 
            DigIntoJsonDead(levelPath, GetUsedFieldTilesReader, ref result);
        public static bool GetUsedTilesetSprites(string levelPath, ref Dictionary<string, HashSet<int>> result) => 
            DigIntoJsonDead(levelPath, GetUsedTilesetSpritesReader, ref result);
        
        
        public static bool GetIsExternalLevels(string projectPath, ref bool result) => 
            DigIntoJson(projectPath, GetIsExternalLevelsInReader, ref result);
        public static bool GetDefaultGridSize(string projectPath, ref int result) => 
            DigIntoJson(projectPath, GetDefaultGridSizeInReader, ref result);
        public static bool GetJsonVersion(string projectPath, ref string result) => 
            DigIntoJson(projectPath, GetJsonVersionReader, ref result);


        private static bool DigIntoJsonDead<T>(string path, JsonReadAction<T> digAction, ref T result)
        {
            return false;
        }
        private static bool DigIntoJson<T>(string path, JsonDigAction<T> digAction, ref T result)
        {
            Profiler.BeginSample($"DigIntoJson {digAction.Method.Name}");
            
            if (!File.Exists(path))
            {
                LDtkDebug.LogError($"Couldn't locate the file to dig into the json for at path: \"{path}\"");
                return false;
            }
            
            Stream sr = File.OpenRead(path);
            bool success = false;
            try
            {
                byte[] bytes = LDtkJsonUtility.GetBytesFromStream(sr);
                JsonReader reader = new JsonReader(bytes);

                success = digAction.Invoke(ref reader, ref result);
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

        
        /// <summary>
        /// For entities example, we expect:
        /// "Button",
        /// "TriggerArea"
        /// "SpotLight",
        /// "Door",
        /// "Repeater",
        /// "MessagePopUp",
        /// "Exit",

        /// "Chest",
        /// "Enemy",
        /// "Teleporter",
        /// "PlayerStart",
        /// "Item",
        /// </summary>
        private static bool GetUsedEntitiesReader(ref JsonReader reader, ref HashSet<string> entities)
        {
            throw new NotImplementedException();
            
            while (reader.Read())
            {
                if (!reader.ReadIsPropertyName("entityInstances"))
                {
                    continue;
                }
                
                Debug.Log("Enter entity instances array");
                
                    
                InfiniteLoopInsurance arrayInsurance = new InfiniteLoopInsurance();
                Assert.IsTrue(reader.GetCurrentJsonToken() == Utf8Json.JsonToken.BeginArray);
                
                int arrayDepth = 0;
                while (reader.IsInArray(ref arrayDepth))
                {
                    arrayInsurance.Insure();


                    int objectDepth = 0;
                    InfiniteLoopInsurance objectInsurance = new InfiniteLoopInsurance();//
                        
                    LogToken(ref reader);
                    StringBuilder sb = new StringBuilder();
                        
                    
                    
                    /*Assert.IsTrue(reader.GetCurrentJsonToken() == Utf8Json.JsonToken.BeginObject);
                    while (reader.IsInObject(ref objectDepth))
                    {
                        sb.Append($"{objectDepth}: {reader.GetCurrentJsonToken()}\n");
                        objectInsurance.LogSb(sb);
                        objectInsurance.Insure();
                        
                        if (reader.ReadIsPropertyName("__identifier"))
                        {
                            string read = reader.ReadString();
                            Debug.Log($"Found __identifier as {read} at depth {objectDepth}");
                            entities.Add(read);
                                
                            //we continue because we need to read to the end of the object
                            //reader.ReadNextBlock();
                            //continue;
                        }
                        reader.ReadNext();
                    }
                    Debug.Log($"Exited object. while in it, did this:\n{sb}");*/
                    
                    //Debug.Log("Going to next array entry");
                    reader.ReadNext();
                }
                reader.ReadNext();

                Utf8Json.JsonToken token = reader.GetCurrentJsonToken();
                Debug.Assert(reader.GetCurrentJsonToken() == Utf8Json.JsonToken.EndArray);
                Debug.Log($"Exited array. Token is {token}");
            }
            
            //its possible to get none if the project uses separate level files
            return true;
            
            /*
                     the old code
                     int entityInstancesDepth = reader.Depth;
                    while (reader.Depth >= entityInstancesDepth && reader.Read())
                    {
                        if (reader.Depth != entityInstancesDepth + 2 || reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "__identifier")
                            continue;

                        reader.Read();
                        entities.Add((string)reader.Value);
                    }*/
        }
        
        private static bool GetUsedIntGridValuesReader(JsonTextReader reader, ref HashSet<string> result)
        {
            string recentIdentifier = "";
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__identifier")
                {
                    reader.Read();
                    if (reader.TokenType == JsonToken.String)
                    {
                        recentIdentifier = (string)reader.Value;
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

                    result.Add(formattedString);
                }
                
                Debug.Assert(reader.TokenType == JsonToken.EndArray, $"not end array at {ReaderInfo(reader)}");
                //Debug.Log($"we hit end array at {ReaderInfo(reader)}");
            }
            
            return true;
        }
        
        private static bool GetUsedProjectLevelBackgroundsReader(JsonTextReader reader, ref HashSet<string> result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "bgRelPath")
                {
                    continue;
                }
                
                reader.Read();
                Debug.Assert(reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Null, $"Not expected value when getting level background path at {ReaderInfo(reader)}");

                string value = (string)reader.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }
            }
            return true;
        }
        private static bool GetUsedSeparateLevelBackgroundReader(JsonTextReader reader, ref string result)
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "bgRelPath")
                {
                    continue;
                }

                reader.Read();
                Debug.Assert(reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Null, $"Not expected value when getting level background path at {ReaderInfo(reader)}");

                result = (string)reader.Value;
                return true;
            }
            return false;
        }
        
        private static string ReaderInfo(JsonTextReader reader)
        {
            return $"{reader.LineNumber}:{reader.LinePosition}, TokenType:{reader.TokenType}, Value:{reader.Value}";
        }
        
        private static bool GetTilesetRelPathsReader(ref JsonReader reader, ref HashSet<string> textures)
        {
            while (reader.Read())
            {
                if (!reader.ReadIsPropertyName("tilesets"))
                {
                    continue;
                }
                
                int depth = 0;
                InfiniteLoopInsurance insurance = new InfiniteLoopInsurance(1000000);
                
                while (reader.CanRead())
                {
                    insurance.Insure();

                    while (reader.IsInArray(ref depth))
                    {
                        if (reader.GetCurrentJsonToken() != Utf8Json.JsonToken.String)
                        {
                            reader.ReadNext();
                            continue;
                        }

                        if (reader.ReadString() == "relPath" && reader.ReadIsNameSeparator())
                        {
                            string value = reader.ReadString();
                            if (!string.IsNullOrEmpty(value)) //doing null check because the embedded icons atlas is null string
                            {
                                textures.Add(value);
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        
        private static bool GetIsExternalLevelsInReader(ref JsonReader reader, ref bool result)
        {
            while (reader.Read())
            {
                if (reader.ReadIsPropertyName("externalLevels"))
                {
                    result = reader.ReadBoolean();
                    return true;
                }
            }
            return false;
        }

        private static bool GetDefaultGridSizeInReader(ref JsonReader reader, ref int result)
        {
            while (reader.Read())
            {
                if (reader.ReadIsPropertyName("defaultGridSize"))
                {
                    result = reader.ReadInt32();
                    return true;
                }
            }
            return false;
        }
        
        private static bool GetUsedFieldTilesReader(JsonTextReader reader, ref List<FieldInstance> result)
        {
            //a field instance: { "__identifier": "integer", "__value": 12345, "__type": "Int", "__tile": null, "defUid": 105, "realEditorValues": [{ "id": "V_Int", "params": [12345] }] },
            //"fieldInstances": [{ "__identifier": "LevelTile", "__value": { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }, "__type": "Tile", "__tile": { "tilesetUid": 149, "x": 96, "y": 32, "w": 32, "h": 16 }, "defUid": 164, "realEditorValues": [{"id": "V_String","params": ["96,32,32,16"]}] }]
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
        
        private static bool GetUsedTilesetSpritesReader(JsonTextReader reader, ref Dictionary<string, HashSet<int>> usedTileIds)
        {
            // In a layer, contains AutoLayerTiles and GridTiles.
            //TileInstance contains the rect, but also a T value that might be usable. 

            while (reader.Read())
            {
                //1. find a layer instance and record the name of the layer. we could encounter the same layer name again, so track the dictionary accordingly.
                if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "layerInstances")
                {
                    continue;
                }
                
                //at this point in time: We've just found the layer instances array.
                //need to see if the layer has start objects or if it's an empty array.
                int arrayDepth = reader.Depth;

                //Debug.Log($"found layer instances for {reader.Path}");
                reader.Read();
                

                //this while loop is looking for the end of the layerinstances array
                Debug.Assert(reader.TokenType == JsonToken.StartArray, ReaderInfo(reader));

                bool brokeOutOfEndArray = false;
                //this while look is to look into the layerinstances array. it is always either one of two things: en end array or start object.
                while (!brokeOutOfEndArray && reader.Read() && reader.TokenType != JsonToken.EndArray && reader.Depth != arrayDepth)
                {
                    //do doubt we should be encountering a start object of a layer instance
                    int objectDepth = reader.Depth;
                    
                    Debug.Assert(reader.TokenType == JsonToken.StartObject, ReaderInfo(reader));

                    //no doubt we are encountering the first item in the object, being __identifier
                    reader.Read();

                    //__identifier
                    Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__identifier", ReaderInfo(reader));
                    string identifier = reader.ReadAsString();
                    if (string.IsNullOrEmpty(identifier))
                    {
                        continue;
                    }

                    //2. Determine it's type.
                    //__type
                    reader.Read();
                    Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__type", ReaderInfo(reader));
                    string type = reader.ReadAsString();
                    
                    //3. Depending on type, go into the appropriate tile array. AutoLayer/IntGrid: AutoLayerTiles, TilesLayer: GridTiles
                    //(possible values: IntGrid, Entities, Tiles or AutoLayer)
                    if (type == "Entities")
                    {
                        SearchUntilEnd(objectDepth, arrayDepth, ref brokeOutOfEndArray);
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
                        if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != arrayToSearch)
                        {
                            continue;
                        }
                        break;
                    }
                    int tileArrayDepth = reader.Depth;
                    
                    Debug.Assert(reader.TokenType == JsonToken.PropertyName); //property name of the tiles array
                    reader.Read();
                    Debug.Assert(reader.TokenType == JsonToken.StartArray);
                    
                    //4. Keep on iterating through the array, grabbing every "t" value and adding to the hashset for this layer.
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndArray && reader.Depth == tileArrayDepth)
                        {
                            break;
                        }
                        
                        if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "t")
                        {
                            int? readAsInt32 = reader.ReadAsInt32();
                            tileIds.Add(readAsInt32.Value);
                        } 
                    }

                    //we will now look for the next endobject so that the while loop can attempt again in case we hit another start array.
                    SearchUntilEnd(objectDepth, arrayDepth, ref brokeOutOfEndArray);
                    void SearchUntilEnd(int i, int arrayDepth1, ref bool b)
                    {
                        while (reader.Read())
                        {
                            if (reader.Depth == i && reader.TokenType == JsonToken.EndObject)
                            {
                                break;
                            }

                            if (reader.Depth == arrayDepth1 && reader.TokenType == JsonToken.EndArray)
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    
                    //when exiting out, we should be in a state of having been to the next array element, or we reached the end of the array and we can then exit out of the array and look for the next layerinstances somewhere.
                    //end object
                    //5. After reaching the end of the tiles array in this layer instance, try to find another object within this array, else exit out.
                }
            }
            
            return true;
        }
        
        private static bool GetJsonVersionReader(ref JsonReader reader, ref string result)
        {
            while (reader.Read())
            {
                if (reader.ReadIsPropertyName("jsonVersion"))
                {
                    result = reader.ReadString();
                    return true;
                }
            }
            return false;
        }

        
        
        public static void LogToken(ref JsonReader reader)
        {
            Debug.Log(reader.GetCurrentJsonToken());
        }
        public static void LogTokensAndValues(ref JsonReader reader)
        {
            Utf8Json.JsonToken token = reader.GetCurrentJsonToken();
            if (token == Utf8Json.JsonToken.String)
            {
                Debug.Log($"{token}:\t{reader.ReadString()}");
            }
            else if (token == Utf8Json.JsonToken.Number)
            {
                Debug.Log($"{token}:\t{reader.ReadDouble()}");
            }
            else
            {
                Debug.Log($"{token}");
            }
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
                Debug.Assert(reader.TokenType == JsonToken.String, ReaderInfo());

                //__value name
                reader.Read();
                Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__value", ReaderInfo());

                
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
                Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__type", ReaderInfo());

                //__type value
                field.Type = reader.ReadAsString();
                Debug.Assert(reader.TokenType == JsonToken.String, ReaderInfo());
                Debug.Assert(field.Type == "Tile" || field.Type == "Array<Tile>", ReaderInfo());

                //continually skipping the tile field until finding the defUid.
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "defUid")
                    {
                        break;
                    }
                }

                //defUid name
                Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "defUid", ReaderInfo());

                //defUid value
                field.DefUid = reader.ReadAsInt32().Value;
                Debug.Assert(reader.TokenType == JsonToken.Integer, ReaderInfo());

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
            Debug.Assert(reader.TokenType == JsonToken.Integer, ReaderInfo());

            //x name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "x", ReaderInfo());
            //x value
            rect.X = reader.ReadAsInt32().Value;
            Debug.Assert(reader.TokenType == JsonToken.Integer);

            //y name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "y", ReaderInfo());
            //y value
            rect.Y = reader.ReadAsInt32().Value;
            Debug.Assert(reader.TokenType == JsonToken.Integer);

            //w name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "w", ReaderInfo());
            //w value
            rect.W = reader.ReadAsInt32().Value;
            Debug.Assert(reader.TokenType == JsonToken.Integer);

            //h name
            reader.Read();
            Debug.Assert(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "h", ReaderInfo());
            //h value
            rect.H = reader.ReadAsInt32().Value;
            Debug.Assert(reader.TokenType == JsonToken.Integer, ReaderInfo());

            //end object
            reader.Read();
            Debug.Assert(reader.TokenType == JsonToken.EndObject, ReaderInfo());
            
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

                    LDtkDebug.Log(msg);
                }
                else
                {
                    string tokenText = Colorize(reader.TokenType.ToString(), "orange");
                    string msg = $"{GetDepthString(reader)} {tokenText}";
                    LDtkDebug.Log(msg);
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