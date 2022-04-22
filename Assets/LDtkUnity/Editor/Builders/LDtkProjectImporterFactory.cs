using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkProjectImporterFactory
    {
        private readonly LDtkProjectImporter _importer;

        public LDtkProjectImporterFactory(LDtkProjectImporter importer)
        {
            _importer = importer;
        }

        public void Import(LdtkJson json)
        {
            GameObject projectGameObject = GetProjectGameObject(json);

            if (projectGameObject == null)
            {
                _importer.ImportContext.LogImportError("LDtk: Project GameObject null, not building correctly");
                return;
            }
            
            _importer.ImportContext.AddObjectToAsset("rootGameObject", projectGameObject, LDtkIconUtility.LoadProjectFileIcon());
            _importer.ImportContext.SetMainObject(projectGameObject);
        }

        private GameObject GetProjectGameObject(LdtkJson json)
        {
            if (json.ExternalLevels)
            {
                //don't build levels if it's an external levels project
                
                GameObject rootObject = new GameObject(_importer.AssetName);
                LDtkComponentProject component = rootObject.AddComponent<LDtkComponentProject>();
                component.SetJson(_importer.JsonFile);
                component.FlagAsSeparateLevels();

                return rootObject;
            }
            
            //set the data class's levels correctly, regardless if they are external levels or not
            RestructureJson(json);

            LDtkProjectBuilder builder = new LDtkProjectBuilder(_importer, json);
            builder.BuildProject();
            GameObject projectGameObject = builder.RootObject;
            return projectGameObject;
        }

        private void RestructureJson(LdtkJson project)
        {
            //we wanna modify the json and serialize it back to that it's usable later with it's completeness regardless of external levels
            LDtkJsonRestructure.Restructure(project, _importer.assetPath);

            string newJson = "";
            try
            {
                newJson = project.ToJson();
            }
            finally
            {
                _importer.JsonFile.SetJson(newJson);
            }
        }
    }
}