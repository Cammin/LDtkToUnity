using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectImporterFactory
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

            LDtkProjectBuilder builder = new LDtkProjectBuilder(_importer, json);
            builder.BuildProject();
            return builder.RootObject;
        }
        
    }
}