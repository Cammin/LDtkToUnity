using UnityEngine;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkArtifactFactory
    {
        private readonly LDtkProjectImporter _importer;
        protected readonly string AssetName;
        
        protected readonly LDtkArtifactAssets Artifacts;

        protected LDtkArtifactFactory(LDtkProjectImporter importer, LDtkArtifactAssets assets, string assetName)
        {
            _importer = importer;
            Artifacts = assets;
            AssetName = assetName;
        }

        protected delegate T AssetGetter<out T>(string assetName);
        protected delegate Object AssetCreator();
        protected delegate bool HasIt(string assetName);
        protected T TryGetAsset<T>(AssetGetter<T> getter) where T : Object
        {
            if (Artifacts == null)
            {
                LDtkDebug.LogError("Null artifact assets. This needs to be instanced during an import");
                return null;
            }
            
            return getter.Invoke(AssetName);
        }
        protected bool TryCreateAsset(HasIt hasIt, AssetCreator creator)
        {
            if (Artifacts == null)
            {
                LDtkDebug.LogError("Null artifact assets. This needs to be instanced");
                return false;
            }

            if (hasIt.Invoke(AssetName))
            {
                LDtkDebug.Log("Already had this object cached");
                return false;
            }
            
            Object tile = creator.Invoke();
            AddArtifact(tile);
            return true;
        }
        
        protected void AddArtifact(Object obj)
        {
            if (obj == null)
            {
                LDtkDebug.LogError("Could not create and add artifact; was null. It will not be available in the importer");
                return;
            }
            _importer.AddArtifact(obj);
        }
    }
}