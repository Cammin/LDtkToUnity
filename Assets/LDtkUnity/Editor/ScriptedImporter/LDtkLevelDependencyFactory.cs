using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal static class LDtkLevelDependencyFactory
    {
        public static string[] GatherLevelDependencies(string levelPath)
        {
            List<string> paths = new List<string>();
            
            string projectPath = new LDtkRelativeGetterProjectImporter().GetPath(levelPath, levelPath);
            if (!File.Exists(projectPath))
            {
                LDtkDebug.LogError($"The project cannot be found at \"{projectPath}\", Check that there are no broken paths. Most likely the project was renamed but not saved from LDtk yet. Save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }
            paths.Add(projectPath);

            string[] lines = LDtkProjectDependencyFactory.LoadMetaLinesAtProjectPath(projectPath);
            IEnumerable<string> entities = LDtkJsonDigger.GetUsedEntitiesInJsonLevel(levelPath);
            List<LDtkProjectDependencyFactory.ParsedMetaData> datas = LDtkProjectDependencyFactory.GetMetaDatas(lines);
            
            foreach (string entity in entities)
            {
                foreach (LDtkProjectDependencyFactory.ParsedMetaData data in datas)
                {
                    if (entity == data.Name)
                    {
                        paths.Add(data.GetAssetPath);        
                    }
                }
            }
            foreach (string path in paths)
            {
                LDtkBuilderDependencies.TestLogDependencySet("GatherLevelDependencies", levelPath, path);
            }
            return paths.ToArray();
        }
    }
}