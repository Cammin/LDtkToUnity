using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class LDtkTextureImportModifier
    {
        protected Texture2D Texture { get; private set; }
        
        public void Modify(Texture2D texture)
        {
            Texture = texture;
            
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError("importer null");
                return;
            }

            AlterTexture(importer);
            
            importer.SaveAndReimport();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        protected abstract void AlterTexture(TextureImporter importer);
    }
}