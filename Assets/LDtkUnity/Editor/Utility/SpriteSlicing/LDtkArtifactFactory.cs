using UnityEngine;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkArtifactFactory
    {
        private readonly LDtkProjectImporter _importer;
        protected readonly string AssetName;
        
        protected readonly LDtkArtifactAssets Assets;

        protected LDtkArtifactFactory(LDtkProjectImporter importer, LDtkArtifactAssets assets, string assetName)
        {
            _importer = importer;
            Assets = assets;
            AssetName = assetName;
        }
        
        protected void AddAsset(Object obj)
        {
            if (obj == null)
            {
                _importer.ImportContext.LogImportError("LDtk: Could not create and add artifact. It will not be available in the importer");
                return;
            }
            _importer.AddArtifact(obj);
        }
    }
}