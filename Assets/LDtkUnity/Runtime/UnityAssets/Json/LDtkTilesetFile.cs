﻿using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// The imported level file. (.ldtkt)<br/>
    /// This is generated by the importer. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.JSON_TILESET)]
    public class LDtkTilesetFile : LDtkJsonFile<LDtkTilesetDefinitionWrapper>
    {
        /// <value>
        /// Gets the deserialized tileset definition.
        /// </value>
        public override LDtkTilesetDefinitionWrapper FromJson
        {
            get
            {
                LDtkProfiler.BeginSample("LDtkTilesetDefinition.FromJson");
                LDtkTilesetDefinitionWrapper json = LDtkTilesetDefinitionWrapper.FromJson(_json);
                LDtkProfiler.EndSample();
                return json;
            }
        }
    }
}