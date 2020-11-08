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
            Debug.unityLogger.logEnabled = false;

            try
            {
                LDtkDataProject project = JsonConvert.DeserializeObject<LDtkDataProject>(json);
                Debug.unityLogger.logEnabled = true;
                return project;
            }
            catch (Exception e)
            {
                Debug.unityLogger.logEnabled = true;
                Debug.LogError(e);
                throw;
            }
        }
    }
}