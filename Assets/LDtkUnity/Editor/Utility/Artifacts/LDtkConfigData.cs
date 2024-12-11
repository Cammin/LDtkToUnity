using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //todo: could be used to spawn levels in runtime potentially
    [Serializable]
    internal struct LDtkConfigData
    {
        public int PixelsPerUnit;
        public GameObject CustomLevelPrefab;
        public bool IntGridValueColorsVisible;
        public bool UseCompositeCollider;
        public CompositeCollider2D.GeometryType GeometryType;
        public bool CreateBackgroundColor;
        public bool CreateLevelBoundsTrigger;
        public bool UseParallax;
        public LDtkAssetIntGridValue[] IntGridValues;
        public LDtkAssetEntity[] Entities;
        
        internal string WriteJson(string projectAssetPath)
        {
            string writePath = GetPath(projectAssetPath);
            string json = EditorJsonUtility.ToJson(this, true);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            
            LDtkPathUtility.TryCreateDirectoryForFile(writePath);
            
            File.WriteAllBytes(writePath, byteArray);
            return writePath;
        }
        
        internal static LDtkConfigData ReadJson(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                return new LDtkConfigData();
            }

            byte[] bytes = File.ReadAllBytes(assetPath);
            string json = Encoding.UTF8.GetString(bytes);

            LDtkConfigData data = new LDtkConfigData();
            EditorJsonUtility.FromJsonOverwrite(json, data);
            return data;
        }
        
        internal static string GetPath(string projectAssetPath)
        {
            string dir = Path.GetDirectoryName(projectAssetPath);
            string importerAssetName = Path.GetFileNameWithoutExtension(projectAssetPath);
            return Path.Combine(dir, importerAssetName, $"{importerAssetName}_Config.{LDtkImporterConsts.CONFIG_EXT}");
        }
    }
}