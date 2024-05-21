﻿using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// The imported level file. (.ldtkl)<br/>
    /// Available when saving levels as separate files in LDtk. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    public class LDtkLevelFile : LDtkJsonFile<Level>
    {
        /// <value>
        /// Gets the deserialized level.
        /// </value>
        public override Level FromJson
        {
            get
            {
                LDtkProfiler.BeginSample("Level.FromJson");
                Level json = Level.FromJson(_json);
                LDtkProfiler.EndSample();
                return json;
            }
        }
    }
}