using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal abstract class LDtkArtifactFactory
    {
        protected readonly AssetImportContext Ctx;
        
        protected readonly string AssetName;
        protected readonly LDtkArtifactAssets Artifacts;

        protected LDtkArtifactFactory(AssetImportContext ctx, LDtkArtifactAssets assets, string assetName)
        {
            Ctx = ctx;
            Artifacts = assets;
            AssetName = assetName;
        }
        
        protected delegate Object AssetCreator();
        protected delegate bool HasIt(string assetName);
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
        
        protected abstract bool AddArtifactAction(Object obj);
        private void AddArtifact(Object obj)
        {
            if (obj == null)
            {
                LDtkDebug.LogError("Could not create and add artifact; was null. It will not be available in the importer");
                return;
            }
            
            if (AddArtifactAction(obj))
            {
                Ctx.AddObjectToAsset(obj.name, obj);
            }
        }
    }
}