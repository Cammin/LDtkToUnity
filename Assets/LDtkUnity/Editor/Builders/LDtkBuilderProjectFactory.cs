using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderProjectFactory
    {
        private readonly LDtkProjectImporter _importer;

        public LDtkBuilderProjectFactory(LDtkProjectImporter importer)
        {
            _importer = importer;
        }

        public void Import(LdtkJson json)
        {
            GameObject projectGameObject = GetProjectGameObject(json);

            if (projectGameObject == null)
            {
                _importer.Logger.LogError("LDtk: Project GameObject null, not building correctly");
                return;
            }
            
            _importer.ImportContext.AddObjectToAsset("rootGameObject", projectGameObject, LDtkIconUtility.LoadProjectFileIcon());
            _importer.ImportContext.SetMainObject(projectGameObject);
        }

        private GameObject GetProjectGameObject(LdtkJson json)
        {
            LDtkProjectBuilder builder = new LDtkProjectBuilder(_importer, json);
            builder.BuildProject();
            return builder.RootObject;
        }
        
    }
}