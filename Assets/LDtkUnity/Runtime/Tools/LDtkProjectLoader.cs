using System;
using LDtkUnity.Runtime.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkProjectLoader
    {
        public static LDtkDataProject LoadProject(string json)
        {
            try
            {
                LDtkDataProject project = JsonConvert.DeserializeObject<LDtkDataProject>(json);
                return project;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}