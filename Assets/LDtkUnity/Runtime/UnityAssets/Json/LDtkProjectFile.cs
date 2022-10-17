using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity
{
    /// <summary>
    /// The imported project file, available in the the import result. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.JSON_PROJECT)]
    public class LDtkProjectFile : LDtkJsonFile<LdtkJson>
    {
        /// <value>
        /// Gets the deserialized project root.
        /// </value>
        public override LdtkJson FromJson
        {
            get
            {
                Profiler.BeginSample("LdtkJson.FromJson");
                LdtkJson json = LdtkJson.FromJson(_json);
                Profiler.EndSample();
                return json;
            }
        }
    }
}