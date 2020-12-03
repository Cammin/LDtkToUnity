// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Providers;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Level
{
    public struct LDtkDataLevelNeighbour
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public string dir { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty] public int levelUid { get; private set; }
        
        
        public LDtkDataLevel LevelReference => LDtkProviderUid.GetUidData<LDtkDataLevel>(levelUid);
    }
}